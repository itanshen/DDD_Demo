using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Domain.ValueObjects;

namespace Users.Domain.Enitties
{
    public record UserLoginHistory : IAggregateRoot
    {
        public long Id { get; init; }
        public Guid? UserId { get; init; }
        public PhoneNumber PhoneNumber { get; init; }
        public DateTime CreatedDateTime { get; init; }
        public string Message { get; init; }
        private UserLoginHistory() { }
        public UserLoginHistory(Guid? userId, PhoneNumber phoneNumber, string message)
        {

            UserId = userId;
            PhoneNumber = phoneNumber;
            CreatedDateTime = DateTime.Now;
            Message = message;
        }
    }
}
