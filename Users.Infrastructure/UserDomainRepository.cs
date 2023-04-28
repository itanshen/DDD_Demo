using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Domain;
using Users.Domain.Enitties;
using Users.Domain.Events;
using Users.Domain.ValueObjects;

namespace Users.Infrastructure
{
    public class UserDomainRepository : IUserDomainRepository
    {
        private readonly UserDbContext dbContext;
        private readonly IDistributedCache distributedCache;
        private readonly IMediator mediator;

        public UserDomainRepository(UserDbContext dbContext, IDistributedCache distributedCache, IMediator mediator)
        {
            this.dbContext = dbContext;
            this.distributedCache = distributedCache;
            this.mediator = mediator;
        }

        public async Task AddNewLoginHistoryAsync(PhoneNumber phoneNumber, string msg)
        {
            var user = await FindOneAsync(phoneNumber);
            UserLoginHistory userLoginHistory = new UserLoginHistory(user?.Id, phoneNumber, msg);
            dbContext.UserLoginHistories.Add(userLoginHistory);//这里不保存
        }

        public async Task<User?> FindOneAsync(PhoneNumber phoneNumber)
        {
            User? user = await dbContext.Users.Include(u => u.AccessFail).FirstOrDefaultAsync(u => u.PhoneNumber.RegionCode.Equals(phoneNumber.RegionCode) && u.PhoneNumber.Number.Equals(phoneNumber.Number));
            return user;
        }

        public async Task<User?> FindOneAsync(Guid userId)
        {
            User? user = await dbContext.Users.Include(u => u.AccessFail).FirstOrDefaultAsync(u => u.Id == userId);
            return user;
        }

        public Task PublishEventAsync(UserAccessResultEvent eventData)
        {
            return mediator.Publish(eventData);
        }

        public async Task<string?> RetrievePhoneCodeAsync(PhoneNumber phoneNumber)
        {
            string fullNumber = $"{phoneNumber.RegionCode}{phoneNumber.Number}";
            string cacheKey = $"LoginByPhoneAndCode_Code_{fullNumber}";
            string? code = await distributedCache.GetStringAsync(cacheKey);
            await distributedCache.RemoveAsync(cacheKey);
            return code;
        }

        public async Task SavePhoneCodeAsync(PhoneNumber phoneNumber, string code)
        {
            string fullNumber = $"{phoneNumber.RegionCode}{phoneNumber.Number}";
            string cacheKey = $"LoginByPhoneAndCode_Code_{fullNumber}";
            var options = new DistributedCacheEntryOptions();
            options.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
            await distributedCache.SetStringAsync(cacheKey, code, options);
        }
    }
}
