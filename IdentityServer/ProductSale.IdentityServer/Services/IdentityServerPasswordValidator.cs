using Duende.IdentityServer.Validation;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using ProductSale.IdentityServer.Models;

namespace ProductSale.IdentityServer.Services
{
    public class IdentityServerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public IdentityServerPasswordValidator(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            ApplicationUser applicationUser = await _userManager.FindByEmailAsync(context.UserName);

            if (applicationUser == null)
            {
                context.Result.Error = "Email or password is wrong";

                return;
            }

            bool isTrue = await _userManager.CheckPasswordAsync(applicationUser, context.Password);

            if (isTrue is false)
            {
                context.Result.Error = "Email or password is wrong";

                return;
            }

            context.Result = new GrantValidationResult(applicationUser.Id,
                                                       OidcConstants.AuthenticationMethods.Password);
        }
    }
}
