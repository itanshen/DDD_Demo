using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Domain.ValueObjects;
using Zack.Commons;

namespace Users.Domain.Enitties
{
    public record User : IAggregateRoot
    {
        public Guid Id { get; init; }
        public PhoneNumber PhoneNumber { get; private set; }
        private string? passwordHash;
        public UserAccessFail AccessFail { get; private set; }

        /// <summary>
        /// EFCore加载数据使用
        /// </summary>
        private User() { }

        public User(PhoneNumber phoneNumber)
        {
            Id = Guid.NewGuid();
            PhoneNumber = phoneNumber;
            AccessFail = new UserAccessFail(this);
        }

        public bool HasPassword()
        {
            return !string.IsNullOrEmpty(passwordHash);
        }
        
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void ChangePassword(string value)
        {
            if (value.Length < 5 && value.Length > 15)
            {
                throw new ArgumentOutOfRangeException("密码长度应为5至15位");
            }
            passwordHash = HashHelper.ComputeMd5Hash(value);
        }

        /// <summary>
        /// 校验密码
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool CheckPassword(string password)
        {
            return passwordHash == HashHelper.ComputeMd5Hash(password);
        }

        /// <summary>
        /// 修改手机号
        /// </summary>
        /// <param name="phoneNumber"></param>
        public void ChangePhoneNumber(PhoneNumber phoneNumber)
        {
            PhoneNumber = phoneNumber;
        }

    }
}
