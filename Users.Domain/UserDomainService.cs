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
    public class UserDomainService
    {
        private readonly IUserDomainRepository repository;
        private readonly ISmsCodeSender smsCodeSender;

        public UserDomainService(IUserDomainRepository repository, ISmsCodeSender smsCodeSender)
        {
            this.repository = repository;
            this.smsCodeSender = smsCodeSender;
        }
        public async Task<UserAccessResult> CheckLoginAsync(PhoneNumber phoneNumber, string password)
        {
            User? user = await repository.FindOneAsync(phoneNumber);
            UserAccessResult result;
            if (user == null)
            {
                result = UserAccessResult.PhoneNumberNotFound;
            }
            else if (IsLockOut(user))
            {
                result = UserAccessResult.Lockout;
            }
            else if (!user.HasPassword())
            {
                result = UserAccessResult.NoPassword;
            }
            else if (user.CheckPassword(password))
            {
                result = UserAccessResult.OK;
            }
            else
            {
                result = UserAccessResult.PasswordError;
            }
            if (user != null)
            {
                if (result == UserAccessResult.OK)
                {
                    ResetAccessFail(user);
                }
                else
                {
                    AccessFail(user);
                }
            }
            UserAccessResultEvent eventItem = new UserAccessResultEvent(phoneNumber, result);
            await repository.PublishEventAsync(eventItem);
            return result;

        }

        public async Task<UserAccessResult> SendCodeAsync(PhoneNumber phoneNumber)
        {
            var user = await repository.FindOneAsync(phoneNumber);
            if (user == null)
            {
                return UserAccessResult.PhoneNumberNotFound;
            }
            if (IsLockOut(user))
            {
                return UserAccessResult.Lockout;
            }
            string code = Random.Shared.Next(1000, 9999).ToString();
            await repository.SavePhoneCodeAsync(phoneNumber, code);
            await smsCodeSender.SendCodeAsync(phoneNumber, code);
            return UserAccessResult.OK;
        }

        public async Task<CheckCodeResult> CheckCodeAsync(PhoneNumber phoneNumber, string code)
        {
            User? user = await repository.FindOneAsync(phoneNumber);
            if (user == null)
            {
                return CheckCodeResult.PhoneNumberNotFound;
            }
            if (IsLockOut(user))
            {
                return CheckCodeResult.Lockout;
            }
            string? codeInServer = await repository.RetrievePhoneCodeAsync(phoneNumber);
            if (string.IsNullOrEmpty(codeInServer))
            {
                return CheckCodeResult.CodeError;
            }
            if (code == codeInServer)
            {
                return CheckCodeResult.OK;
            }
            else
            {
                AccessFail(user);
                return CheckCodeResult.CodeError;
            }
        }

        public void ResetAccessFail(User user)
        {
            user.AccessFail.Reset();
        }

        public bool IsLockOut(User user)
        {
            return user.AccessFail.IsLockOut();
        }

        public void AccessFail(User user)
        {
            user.AccessFail.Fail();
        }

    }
}

