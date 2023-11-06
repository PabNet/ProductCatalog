using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Services.Abstractions;

namespace ProductCatalog.Web.Controllers.Templates
{
    public class BaseCookieController : BaseController
    {
        protected const string CurrentRoleCookieTitle = "CurrentUserRole";

        protected CookieUtility _cookieUtility { get; set; }

        public BaseCookieController(CookieUtility cookieUtility, IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            this._cookieUtility = cookieUtility;
        }
    }
}
