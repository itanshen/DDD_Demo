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
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("T_Users");
            builder.Property("passwordHash").HasMaxLength(500).IsUnicode(false);
            builder.OwnsOne(u => u.PhoneNumber, pn =>
            {
                pn.Property(x => x.RegionCode).HasMaxLength(50).IsUnicode(false);
                pn.Property(x => x.Number).HasMaxLength(50).IsUnicode(false);
            });
            builder.HasOne(u => u.AccessFail).WithOne(u => u.User)
                .HasForeignKey<UserAccessFail>(u => u.UserId);
        }
    }
}
