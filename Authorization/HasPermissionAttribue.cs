using Microsoft.AspNetCore.Authorization;
using TokenGenerationApi.Models;

namespace TokenGenerationApi.Authorization
{
    public class HasPermissionAttribue:AuthorizeAttribute
    {
        public HasPermissionAttribue(Permission permission):base(policy:permission.ToString())
        {
            
        }
    }
}
