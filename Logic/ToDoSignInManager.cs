using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;
using ToDoTnet.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Collections.Generic;
using ToDoTnet.DataEntities;

namespace ToDoTnet.Logic
{
    public class ToDoSignInManager : SignInManager<ToDoUser> 
    {
        public ToDoSignInManager(UserManager<ToDoUser> userManager, 
            IHttpContextAccessor contextAccessor, 
            IUserClaimsPrincipalFactory<ToDoUser> claimsFactory, 
            IOptions<IdentityOptions> optionsAccessor, 
            ILogger<SignInManager<ToDoUser>> logger) : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger)
        {
            
        }

        public override async Task<SignInResult> PasswordSignInAsync(string userName, string password, bool isPersistent, bool lockoutOnFailure)
        {
            var user = await UserManager.FindByNameAsync(userName);
            if (user == null )
            {
                return SignInResult.Failed;
            }

            //claimsFactory.CreateAsync();
            return SignInResult.Success;
        }

        public override async Task<ClaimsPrincipal> CreateUserPrincipalAsync(ToDoUser user)
        {
            var principal = await base.CreateUserPrincipalAsync(user); 
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, user.UserName, ClaimValueTypes.String, "urn:ToDoTnet"));

            var identity = new ClaimsIdentity(claims, "Cookie");

            principal.AddIdentity(identity);
            return principal;
        }

    }
}
