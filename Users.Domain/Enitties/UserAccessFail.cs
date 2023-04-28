using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Users.Domain.Enitties
{
    public class UserAccessFail
    {
        public Guid Id { get; init; }
        public Guid UserId { get; init; }
        public User User { get; init; }
        private bool lockOut;
        public DateTime? LockoutEnd { get; private set; }
        public int AccessFailedCount { get; private set; }

        /// <summary>
        /// EFCore加载数据使用
        /// </summary>
        private UserAccessFail()
        {

        }

        public UserAccessFail(User user)
        {
            Id = Guid.NewGuid();
            User = user;
        }

        public void Reset()
        {
            lockOut = false;
            LockoutEnd = null;
            AccessFailedCount = 0;
        }

        public bool IsLockOut()
        {
            if (lockOut)
            {
                if (LockoutEnd >= DateTime.Now)
                {
                    return true;
                }
                // 锁定期已到，重置
                Reset();
                return false;
            }
            return false;
        }

        /// <summary>
        /// 处理一次“登录失败”
        /// </summary>
        /// <param name="lockOutMinutes">锁定时间，单位分钟，默认5分钟</param>
        public void Fail(int lockOutMinutes = 5)
        {
            this.AccessFailedCount++;
            if (AccessFailedCount >= 3)
            {
                lockOut = true;
                LockoutEnd = DateTime.Now.AddMinutes(lockOutMinutes);
            }
        }

    }
}
