using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using ToDoTnet.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication;

namespace ToDoTnet.Logic
{
    public class ToDoSignInManager : SignInManager<ToDoUser> 
    {
        public ToDoSignInManager(UserManager<ToDoUser> userManager, 
            IHttpContextAccessor contextAccessor, 
            IUserClaimsPrincipalFactory<ToDoUser> claimsFactory, 
            IOptions<IdentityOptions> optionsAccessor, 
            ILogger<SignInManager<ToDoUser>> logger,
            IAuthenticationSchemeProvider schemes) : 
            base(userManager, 
                contextAccessor, 
                claimsFactory, 
                optionsAccessor, 
                logger,schemes)
        {
            
        }

        public override async Task<SignInResult> PasswordSignInAsync(string userName, string password, bool isPersistent, bool lockoutOnFailure)
        {
            var user = await UserManager.FindByNameAsync(userName);
            if (user == null )
            {
                return SignInResult.Failed;
            }

            _ = new PasswordHasher<ToDoUser>();
            if (user.Password != password){
                return SignInResult.Failed;
            }


            //claimsFactory.CreateAsync();
            return SignInResult.Success;
        }

        public override async Task<ClaimsPrincipal> CreateUserPrincipalAsync(ToDoUser user)
        {
            var principal = await base.CreateUserPrincipalAsync(user);


            principal.AddIdentity(new ClaimsIdentity(new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName, ClaimValueTypes.String, "urn:ToDoTnet")
            }, "Cookie"));
            return principal;
        }

    }
}
