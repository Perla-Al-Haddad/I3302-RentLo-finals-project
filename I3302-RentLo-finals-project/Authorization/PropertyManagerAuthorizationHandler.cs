using System.Threading.Tasks;
using I3302_RentLo_finals_project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;

namespace I3302_RentLo_finals_project.Authorization
{
    public class PropertyManagerAuthorizationHandler :
    AuthorizationHandler<OperationAuthorizationRequirement, Property>
    {
        protected override Task
            HandleRequirementAsync(AuthorizationHandlerContext context,
                                   OperationAuthorizationRequirement requirement,
                                   Property resource)
        {
            if (context.User == null || resource == null)
            {
                return Task.CompletedTask;
            }

            // If not asking for approval/reject, return.
            if (requirement.Name == Constants.CreateOperationName ||
                requirement.Name == Constants.DeleteOperationName || 
                requirement.Name == Constants.ReadOperationName ||
                requirement.Name == Constants.UpdateOperationName)
            {
                return Task.CompletedTask;
            }

            // Managers can approve or reject.
            if (context.User.IsInRole(Constants.PropertyManagersRole))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
