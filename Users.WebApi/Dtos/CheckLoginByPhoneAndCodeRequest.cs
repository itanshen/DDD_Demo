using Users.Domain.ValueObjects;

namespace Users.WebAPI.Dtos
{
    public record CheckLoginByPhoneAndCodeRequest(PhoneNumber PhoneNumber, string Code);
}
