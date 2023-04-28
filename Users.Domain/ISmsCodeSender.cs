using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Domain.ValueObjects;

namespace Users.Domain
{
    public interface ISmsCodeSender
    {
        Task SendCodeAsync(PhoneNumber phoneNumber, string code);
    }
}
