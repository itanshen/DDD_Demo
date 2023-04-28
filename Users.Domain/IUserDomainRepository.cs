using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Domain.Enitties;
using Users.Domain.Events;
using Users.Domain.ValueObjects;

namespace Users.Domain
{
    public interface IUserDomainRepository
    {
        Task<User?> FindOneAsync(PhoneNumber phoneNumber);
        Task<User?> FindOneAsync(Guid userId);
        Task AddNewLoginHistoryAsync(PhoneNumber phoneNumber, string msg);
        Task SavePhoneCodeAsync(PhoneNumber phoneNumber, string code);
        Task<string?> RetrievePhoneCodeAsync(PhoneNumber phoneNumber);
        Task PublishEventAsync(UserAccessResultEvent eventData);

    }
}
