using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Users.Domain;
using Users.Infrastructure;
using Users.WebAPI.Dtos;

namespace Users.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [UnitOfWorkAttribute(typeof(UserDbContext))]
    public class LoginController : ControllerBase
    {
        private readonly UserDomainService domainService;

        public LoginController(UserDomainService domainService)
        {
            this.domainService = domainService;
        }

        [HttpPost]
        public async Task<IActionResult> LoginByPhoneAndPwd(LoginByPhoneAndPwdRequest dto)
        {
            if (dto.Password.Length < 3)
                return BadRequest("密码长度不能小于3");
            var phoneNum = dto.PhoneNumber;
            var result = await domainService.CheckLoginAsync(phoneNum, dto.Password);
            switch (result)
            {
                case UserAccessResult.OK:
                    return Ok("登录成功");
                case UserAccessResult.PhoneNumberNotFound:
                case UserAccessResult.NoPassword:
                case UserAccessResult.PasswordError:
                    return BadRequest("手机号或密码错误");
                case UserAccessResult.Lockout:
                    return BadRequest("用户被锁定，请稍后再试");
                default:
                    throw new NotImplementedException();
            }
        }

        [HttpPost("phonecode")]
        public async Task<IActionResult> SendCodeByPhone(SendLoginByPhoneAndCodeRequest dto)
        {
            var result = await domainService.SendCodeAsync(dto.PhoneNumber);
            switch (result)
            {
                case UserAccessResult.OK:
                    return Ok("验证码已发出");
                case UserAccessResult.Lockout:
                    return BadRequest("用户被锁定，请稍后再试");
                default:
                    return BadRequest("请求错误");
            }
        }

        [HttpPost("phonecode/check")]
        public async Task<IActionResult> CheckCode(CheckLoginByPhoneAndCodeRequest dto)
        {
            var result = await domainService.CheckCodeAsync(dto.PhoneNumber, dto.Code);
            switch (result)
            {
                case CheckCodeResult.OK:
                    return Ok("登录成功");
                case CheckCodeResult.PhoneNumberNotFound:
                    return BadRequest("请求错误");
                case CheckCodeResult.Lockout:
                    return BadRequest("用户被锁定，请稍后再试");
                case CheckCodeResult.CodeError:
                    return BadRequest("验证码错误");
                default:
                    throw new NotImplementedException();
            }
        }

    }
}
