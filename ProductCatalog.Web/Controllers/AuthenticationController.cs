using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Domain.Models.Authentication;
using ProductCatalog.Services.Abstractions;
using ProductCatalog.Utility.Helpers;
using ProductCatalog.Web.Controllers.Templates;

namespace ProductCatalog.Web.Controllers
{
    public class AuthenticationController : BaseCookieController
    {
        private ConfigurationUtility _configurationUtility { get; set; }
        public AuthenticationController(ConfigurationUtility configurationutility, IUnitOfWork unitOfWork, CookieUtility cookieUtility) : base(cookieUtility, unitOfWork) 
        {
            this._configurationUtility = configurationutility;
        }

        [HttpGet]
        public IActionResult Authorization()
        {
            if(this._cookieUtility.CookieExists(CurrentRoleCookieTitle))
            {
                return RedirectToAction("Main", "DataTable");
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Authorization(User user)
        {
            var fullUser = await this._unitOfWork.UserService.GetFullEntry(user);
            if(fullUser != null)
            {
                if(fullUser.IsLocked)
                {
                    ViewData["ErrorMessage"] = "This user is blocked";

                    return View();
                }

                var lifetimeInMinutesStr = this._configurationUtility.GetValue("Cookies:LifetimeInMinutes");
                int.TryParse(lifetimeInMinutesStr, out var lifetimeInMinute);
                this._cookieUtility.UpdateCookie(CurrentRoleCookieTitle, fullUser.Role.Name, lifetimeInMinute);

                return RedirectToAction("Main", "DataTable");
            }
            else
            {
                ViewData["ErrorMessage"] = "This user does not exist";

                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Registration(User user)
        {
            var isAddingSuccess = await this._unitOfWork.UserService.AddEntryAsync(user);

            if(isAddingSuccess)
            {
                return RedirectToAction("Authorization", "Authentication");
            }
            else
            {
                ViewData["ErrorMessage"] = "This user already exists";

                return View();
            }
        }
    }
}
