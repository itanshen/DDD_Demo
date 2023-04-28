using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Domain.Enitties;

namespace Users.Infrastructure.Configs
{
    public class UserLoginHistoryConfig : IEntityTypeConfiguration<UserLoginHistory>
    {
        public void Configure(EntityTypeBuilder<UserLoginHistory> builder)
        {
            builder.ToTable("T_UserLoginHistories");
            builder.OwnsOne(u => u.PhoneNumber, pn =>
            {
                pn.Property(x => x.RegionCode).HasMaxLength(50).IsUnicode(false);
                pn.Property(x => x.Number).HasMaxLength(50).IsUnicode(false);
            });
        }
    }
}
