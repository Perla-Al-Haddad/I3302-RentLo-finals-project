using System.Threading.Tasks;
using I3302_RentLo_finals_project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace I3302_RentLo_finals_project.Authorization
{
    public class PropertyAdministratorsAuthorizationHandler
                    : AuthorizationHandler<OperationAuthorizationRequirement, Property>
    {
        protected override Task HandleRequirementAsync(
                                              AuthorizationHandlerContext context,
                                    OperationAuthorizationRequirement requirement,
                                     Property resource)
        {
            if (context.User == null)
            {
                return Task.CompletedTask;
            }

            // Administrators can do anything.
            if (context.User.IsInRole(Constants.PropertyAdministratorsRole))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}