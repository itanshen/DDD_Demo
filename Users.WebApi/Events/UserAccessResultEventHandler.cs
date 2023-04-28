using MediatR;
using Users.Domain;
using Users.Domain.Events;

namespace Users.WebAPI.Events
{
    public class UserAccessResultEventHandler : INotificationHandler<UserAccessResultEvent>
    {
        private readonly IUserDomainRepository repository;

        public UserAccessResultEventHandler(IUserDomainRepository repository)
        {
            this.repository = repository;
        }

        public async Task Handle(UserAccessResultEvent notification, CancellationToken cancellationToken)
        {
            var phoneNumber = notification.PhoneNumber;
            var result = notification.Result;
            string msg;
            switch (result)
            {
                case UserAccessResult.OK:
                    msg = $"{phoneNumber}登录成功";
                    break;
                case UserAccessResult.PhoneNumberNotFound:
                    msg = $"{phoneNumber}登录失败，用户不存在";
                    break;
                case UserAccessResult.NoPassword:
                    msg = $"{phoneNumber}登录失败，未设置密码";
                    break;
                case UserAccessResult.PasswordError:
                    msg = $"{phoneNumber}登录失败，密码错误";
                    break;
                case UserAccessResult.Lockout:
                    msg = $"{phoneNumber}登录失败，用户被锁定";
                    break;
                default:
                    throw new NotImplementedException();
            }
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("EventHandler" + msg);
            Console.ForegroundColor = ConsoleColor.White;
            await repository.AddNewLoginHistoryAsync(phoneNumber, msg);
        }
    }
}
