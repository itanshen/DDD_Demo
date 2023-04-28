using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Users.Domain;
using Users.Domain.Enitties;
using Users.Domain.ValueObjects;
using Users.Infrastructure;
using Users.WebAPI.Dtos;

namespace Users.WebAPI
{
    [Route("api/[controller]")]
    [ApiController]
    [UnitOfWork(typeof(UserDbContext))]
    public class UserMgrController : ControllerBase
    {
        private readonly UserDbContext dbContext;
        private readonly UserDomainService domainService;
        private readonly IUserDomainRepository repository;

        public UserMgrController(UserDbContext dbContext, UserDomainService domainService, IUserDomainRepository repository)
        {
            this.dbContext = dbContext;
            this.domainService = domainService;
            this.repository = repository;
        }

        [HttpPost]
        [Route("user")]
        public async Task<IActionResult> AddNew(PhoneNumber dto)
        {
            if ((await repository.FindOneAsync(dto)) != null)
            {
                return BadRequest("手机号已存在");
            }
            User user = new User(dto);
            dbContext.Users.Add(user);
            return Ok("添加成功");
        }

        [HttpPut]
        [Route("password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequest dto)
        {
            var user = await repository.FindOneAsync(dto.Id);
            if (user == null)
            {
                return NotFound();
            }
            user.ChangePassword(dto.Password);
            return Ok("成功");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Unlock(Guid id)
        {
            var user = await repository.FindOneAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            domainService.ResetAccessFail(user);
            return Ok("成功");
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() {
            var users = await dbContext.Users.ToListAsync();
            return Ok(users);
        }


    }
}
