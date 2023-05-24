using Autofac;
using System.Reflection;

namespace Users.WebAPI
{
    // 继承AutoFac下的Module
    public class AutoFacManager : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var apiService = Assembly.Load("Users.Domain");
            var repository = Assembly.Load("Users.Infrastructure");
            // 注入Service程序集
            builder.RegisterAssemblyTypes(apiService)
                //.Where(x=>x.Name.EndsWith("Service") && x.IsClass)
                .PublicOnly()
                .AsSelf()
                .AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(repository).PublicOnly().AsImplementedInterfaces().InstancePerDependency();

            base.Load(builder);
        }
    }
}
