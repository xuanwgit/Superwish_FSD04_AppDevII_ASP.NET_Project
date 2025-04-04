using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Superwish_FSD04_AppDevII_ASP.NET_Project.Filters
{
    public class RequireAdminAttribute : Attribute, IPageFilter
    {
        public string[] OnlyForHandlers { get; set; } = Array.Empty<string>();

        public void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            var handler = context.HandlerMethod?.MethodInfo.Name?.Replace("async", "", StringComparison.OrdinalIgnoreCase);
            if (handler == null) return;

            // Extract the handler name without the "OnPost" or "OnGet" prefix
            var handlerName = handler.StartsWith("OnPost") ? handler[6..] : 
                            handler.StartsWith("OnGet") ? handler[5..] : 
                            handler;

            if (OnlyForHandlers.Contains(handlerName) && !context.HttpContext.User.IsInRole("Admin"))
            {
                context.Result = new ForbidResult();
            }
        }

        public void OnPageHandlerSelected(PageHandlerSelectedContext context) { }
        public void OnPageHandlerExecuted(PageHandlerExecutedContext context) { }
    }
} 