using Users.Domain.ValueObjects;

namespace Users.WebAPI.Dtos
{
    public record SendLoginByPhoneAndCodeRequest(PhoneNumber PhoneNumber);
}
