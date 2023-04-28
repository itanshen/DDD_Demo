using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Users.Domain.ValueObjects
{
    public record PhoneNumber(int RegionCode=86, string Number="13121111111");
}
