using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Domain;
using Users.Domain.ValueObjects;

namespace Users.Infrastructure
{
    public class MockSmsCodeSender : ISmsCodeSender
    {
        private readonly ILogger<MockSmsCodeSender> logger;

        public MockSmsCodeSender(ILogger<MockSmsCodeSender> logger)
        {
            this.logger = logger;
        }

        public Task SendCodeAsync(PhoneNumber phoneNumber, string code)
        {
            //真实的短信发送可以用领域事件+微服务实现。
            logger.LogInformation($"向{phoneNumber}发验证码{code}");
            return Task.CompletedTask;
        }
    }
}
