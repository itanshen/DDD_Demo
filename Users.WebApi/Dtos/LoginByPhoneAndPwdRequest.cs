using Users.Domain.ValueObjects;

namespace Users.WebAPI.Dtos
{
    public record LoginByPhoneAndPwdRequest(PhoneNumber PhoneNumber, string Password);
}
