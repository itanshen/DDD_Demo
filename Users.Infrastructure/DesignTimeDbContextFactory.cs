using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Users.Infrastructure
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<UserDbContext>
    {
        public UserDbContext CreateDbContext(string[] args)
        {
            var options = new DbContextOptionsBuilder<UserDbContext>();
            string connStr = "Data Source=.;Initial Catalog=DDD_Demo;Integrated Security=true;User ID=sa; Password=sa;Encrypt=True;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true";
            options.UseSqlServer(connStr);
            return new UserDbContext(options.Options);
        }
    }
}
