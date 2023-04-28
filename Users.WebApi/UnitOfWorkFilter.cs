using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Users.WebAPI
{

    public class UnitOfWorkFilter : IAsyncActionFilter
    {
        private static UnitOfWorkAttribute? GetUnitOfWorkAttribute(ActionDescriptor actionDescriptor)
        {
            var caDesc = actionDescriptor as ControllerActionDescriptor;
            if (caDesc == null)
            {
                return null;
            }
            var uowAttr = caDesc.ControllerTypeInfo.GetCustomAttribute<UnitOfWorkAttribute>();
            if (uowAttr != null)
            {
                return uowAttr;
            }
            else
            {
                return caDesc.MethodInfo.GetCustomAttribute<UnitOfWorkAttribute>();
            }
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var uowAttr = GetUnitOfWorkAttribute(context.ActionDescriptor);
            if (uowAttr == null)
            {
                await next();
                return;
            }
            List<DbContext> dbCtxs = new List<DbContext>();
            foreach (var dbCtxType in uowAttr.DbContextTypes)
            {
                // 用HttpContext的RequestServices，确保获取的是和请求相关的Scope实例
                var sp = context.HttpContext.RequestServices;
                DbContext dbCtx = (DbContext)sp.GetRequiredService(dbCtxType);
                dbCtxs.Add(dbCtx);
            }
            var result = await next();
            if (result.Exception == null)
            {
                foreach (var dbCtx in dbCtxs)
                {
                    await dbCtx.SaveChangesAsync();
                }
            }
        }
    }
}
