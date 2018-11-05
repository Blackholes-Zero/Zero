using IdentityModel;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using IdentityServer4.Events;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Passport.AspnetCore.com.Models;
using Auth.AspnetCore.DataAccess.Models;
using Auth.AspnetCore.DataAccess.Dappers;
using IdentityServer4;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Passport.AspnetCore.com.Controllers
{
    public class AccountController : Controller
    {
        private readonly DapperAdminUsers _dapper;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IClientStore _clientStore;
        private readonly IAuthenticationSchemeProvider _schemeProvider;
        private readonly IEventService _events;

        public AccountController(
           IIdentityServerInteractionService interaction,
           IClientStore clientStore,
           IAuthenticationSchemeProvider schemeProvider,
           IEventService events,
           DapperAdminUsers dapper)
        {
            // if the TestUserStore is not in DI, then we'll just use the global users collection
            // this is where you would plug in your own custom identity management library (e.g. ASP.NET Identity)
            _dapper = dapper;
            _interaction = interaction;
            _clientStore = clientStore;
            _schemeProvider = schemeProvider;
            _events = events;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> login(string returnUrl)
        {
            await HttpContext.SignOutAsync();
            if (User.Identity.IsAuthenticated)
            {
                return Redirect(returnUrl ?? "/");
            }

            var vmodel = new LoginViewModel()
            {
                ReturnUrl = returnUrl
            };
            return View(vmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                AdminUsers user = await _dapper.FindByLoginAsync(model.UserName, model.Password);
                if (user != null)
                {
                    AuthenticationProperties props = new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTimeOffset.UtcNow.Add(TimeSpan.FromDays(1)),
                        AllowRefresh = true
                    };
                    //var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                    //                identity.AddClaim(new Claim(ClaimTypes.Sid, userName));
                    //                identity.AddClaim(new Claim(ClaimTypes.Name, user.Name));
                    //                 identity.AddClaim(new Claim(ClaimTypes.Role, user.Role));

                    //var Claims = new List<Claim>() { new Claim(JwtClaimTypes.Role, "admin") };
                    await HttpContext.SignInAsync(user.Id.ToString(), user.LoginName, props, new Claim(JwtClaimTypes.Role, "admin"));
                    return Redirect(model.ReturnUrl ?? "/");
                }
                else
                {
                    View(model.ReturnUrl);
                }
            }
            return View(model.ReturnUrl);
        }

        [HttpGet]
        public async Task<IActionResult> logout(string logoutId)
        {
            //if (User?.Identity.IsAuthenticated == true)
            //{
            await HttpContext.SignOutAsync();
            var logout = await _interaction.GetLogoutContextAsync(logoutId);
            if (!string.IsNullOrWhiteSpace(logout?.PostLogoutRedirectUri))
            {
                return Redirect(logout.PostLogoutRedirectUri);
            }
            //}
            return Redirect("/account/Logouted");
        }

        [HttpGet]
        public IActionResult Logouted()
        {
            return View();
        }
    }
}