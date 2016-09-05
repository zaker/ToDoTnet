using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDoTnet.Models;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using TodoApi.Controllers;
using Microsoft.AspNetCore.Http;

namespace ToDoTnet.Controllers
{

    public class UserController : Controller
    {
        private readonly UserManager<ToDoUser> _userManager;
        private readonly SignInManager<ToDoUser> _signInManager;
        //private readonly IEmailSender _emailSender;
        //private readonly ISmsSender _smsSender;
        private readonly ILogger _logger;

        public UserController(
            UserManager<ToDoUser> userManager,
            SignInManager<ToDoUser> signInManager,
            ILoggerFactory loggerFactory)
        {
            _userManager = userManager;

            _signInManager = signInManager;
            _logger = loggerFactory.CreateLogger<UserController>();
        }


        //[HttpGet("{id:string}")]
        public async Task<IActionResult> Get(string id)
        {
            return OkOrNotFound(await _userManager.FindByIdAsync(id));
        }

        //
        // GET: /User/Login
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            return null;
        }

        //
        // POST: /User/Login
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody]LoginModel model, string returnUrl = null)
        {

            if (ModelState.IsValid)
            {

                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(model.User, model.Password, isPersistent: false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation(1, "User logged in.");
                    const string Issuer = "ToDoTnet";
                    var claims = new List<Claim>();
                    claims.Add(new Claim(ClaimTypes.Name, model.User, ClaimValueTypes.String, Issuer));
                    var userIdentity = new ClaimsIdentity("ToDoLogin");
                    userIdentity.AddClaims(claims);
                    var userPrincipal = new ClaimsPrincipal(userIdentity);

                    await HttpContext.Authentication.SignInAsync("Cookie", userPrincipal,
                        new AuthenticationProperties
                        {
                            ExpiresUtc = DateTime.UtcNow.AddMinutes(30),
                            IsPersistent = false,
                            AllowRefresh = false
                        });

                    return RedirectToLocal(returnUrl);
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning(2, "User account locked out.");
                    return View("Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /User/Register
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody]LoginModel userModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ToDoUser user = new ToDoUser(new DataEntities.User())
            {
                UserName = userModel.User,
                Email = userModel.Email,
                Password = userModel.Password
            };
            
            var result = await _userManager.CreateAsync(user, userModel.Password);

            return CreatedAtAction(nameof(Get), new { id = user.Id }, user);

        }


        //
        // POST: /User/LogOff
        [HttpPost]
        [Authorize]

        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation(4, "User logged out.");
            return RedirectToAction(nameof(TodoController.Get), "ToDo");
        }

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private Task<ToDoUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(TodoController.Get), "ToDo");
            }
        }
        private IActionResult OkOrNotFound(object result)
        {
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }


        #endregion
    }
}
