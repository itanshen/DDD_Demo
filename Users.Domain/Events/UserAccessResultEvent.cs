using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Domain.ValueObjects;

namespace Users.Domain.Events
{
    public record UserAccessResultEvent(PhoneNumber PhoneNumber, UserAccessResult Result) : INotification;
}
