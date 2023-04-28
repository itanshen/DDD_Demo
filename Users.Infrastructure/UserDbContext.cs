using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Users.Domain.Enitties;

namespace Users.Infrastructure
{
    public class UserDbContext : DbContext
    {
        public DbSet<User> Users { get; private set; }
        public DbSet<UserLoginHistory> UserLoginHistories { get; private set; }

        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
        }
    }
}
