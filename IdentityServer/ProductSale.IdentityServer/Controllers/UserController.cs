using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using ProductSale.IdentityServer.Dtos;
using ProductSale.IdentityServer.Models;
using System.Security.Claims;
using static Duende.IdentityServer.IdentityServerConstants;

namespace ProductSale.IdentityServer.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize(LocalApi.PolicyName)]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Signup(SignupDto signupDto)
        {
            if (signupDto is null)
                return BadRequest();

            ApplicationUser applicationUser = new ApplicationUser()
            {
                Email = signupDto.Email,
                UserName = signupDto.Username
            };

            IdentityResult identityResult = await _userManager.CreateAsync(applicationUser);

            if (identityResult.Succeeded is false)
            {
                return BadRequest();
            }

            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            Claim claim = User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub);

            if (claim is null)
                return BadRequest();

            ApplicationUser applicationUser = await _userManager.FindByIdAsync(claim.Value);

            if (applicationUser is null) return BadRequest();

            return Ok(new
            {
                applicationUser.UserName,
                applicationUser.Email,
                applicationUser.Id
            });
        }
    }
}
