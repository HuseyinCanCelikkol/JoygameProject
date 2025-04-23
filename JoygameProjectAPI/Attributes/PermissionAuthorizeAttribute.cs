using JoygameProject.Application.DTOs;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace JoygameProject.API.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class PermissionAuthorizeAttribute : Attribute, IAsyncAuthorizationFilter
    {
        private readonly string _pageCode;
        private readonly string _permissionType;

        public PermissionAuthorizeAttribute(string pageCode, string permissionType)
        {
            _pageCode = pageCode.ToLower();
            _permissionType = permissionType.ToLower(); // canview, canupdate...
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            if (!user.Identity?.IsAuthenticated ?? true)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var claim = user.FindFirst($"perm:{_pageCode}");
            if (claim == null)
            {
                context.Result = new ForbidResult();
                return;
            }

            var permission = JsonSerializer.Deserialize<PermissionDto>(claim.Value);
            if (permission == null)
            {
                context.Result = new ForbidResult();
                return;
            }

            var hasPermission = _permissionType switch
            {
                "canview" => permission.CanView,
                "caninsert" => permission.CanInsert,
                "canupdate" => permission.CanUpdate,
                "candelete" => permission.CanDelete,
                _ => false
            };

            if (!hasPermission)
            {
                context.Result = new ForbidResult();
            }
        }
    }

}
