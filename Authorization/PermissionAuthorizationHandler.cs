using Microsoft.AspNetCore.Authorization;
using TokenGenerationApi.Models;

namespace TokenGenerationApi.Authorization
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly UserContext userContext;
        private readonly TokenClass _tokenClass;
        public PermissionAuthorizationHandler(UserContext userContext, TokenClass tokenClass)
        {
            this.userContext = userContext;
            _tokenClass = tokenClass;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            //var permissions = context.User.Claims.Where(c => c.Type == "permissions").Select(c => c.Value).ToList();
            
            var names = context.User.Claims.Where(c => c.Type== "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Select(c=>c.Value).ToList();
            foreach(var name in names)
            {
                var permissions = userContext.LoginModels?.Where(u => u.UserName==name).Select(u => u.Permission).ToList();
                if (permissions != null)
                {
                    if (permissions.Contains( requirement.Permission))
                    {
                        context.Succeed(requirement);
                        break;
                    }
                }
            }
           
            
            //if (permissions.Contains(requirement.Permission))
            //{
            //    context.Succeed(requirement);
            //}
            return Task.CompletedTask;
        }
    }
}
