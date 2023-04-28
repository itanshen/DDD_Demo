using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Users.Domain;
using Users.Infrastructure;
namespace Users.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<UserDbContext>(opt =>
            {
                string connStr = builder.Configuration.GetConnectionString("Default");
                //connStr = "Data Source=.;Initial Catalog=DDD_Demo;Integrated Security=true;User ID=sa; Password=sa;Encrypt=True;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true";
                opt.UseSqlServer(connStr);
            });
            builder.Services.AddStackExchangeRedisCache(opt =>
            {
                //opt.Configuration = "localhost";

                opt.InstanceName = "UsersDemo_";//¼üÃûÇ°×º
                opt.ConfigurationOptions = new StackExchange.Redis.ConfigurationOptions
                {
                    EndPoints = { { "127.0.0.1", 6379 } },
                    Password = "99fangxinyong"
                };
            });
            builder.Services.Configure<MvcOptions>(opt =>
            {
                opt.Filters.Add<UnitOfWorkFilter>();
            });
            builder.Services.AddScoped<UserDomainService>();
            builder.Services.AddScoped<ISmsCodeSender, MockSmsCodeSender>();
            builder.Services.AddScoped<IUserDomainRepository, UserDomainRepository>();
            builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}