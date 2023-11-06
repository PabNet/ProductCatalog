using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IIS.Core;
using ProductCatalog.Domain.Models.Authentication;
using ProductCatalog.Domain.Models.Core;
using ProductCatalog.Services.Abstractions;
using ProductCatalog.Services.Models.NationalBank;
using ProductCatalog.Utility.Enums;
using ProductCatalog.Utility.Extensions;
using ProductCatalog.Utility.Helpers;
using ProductCatalog.Web.Controllers.Templates;

namespace ProductCatalog.Web.Controllers
{
    public class DataTableController : BaseCookieController
    {
        private IAccessControlService _accessControlService { get; set; }
        private IApiService _apiService { get; set; }
        private ConfigurationUtility _configurationUtility { get; set; }
        public DataTableController(ConfigurationUtility configurationUtility,
                                   IApiService apiService,
                                   IAccessControlService accessControlService,
                                   CookieUtility cookieUtility,
                                   IUnitOfWork unitOfWork) : base(cookieUtility, unitOfWork)
        {
            this._configurationUtility = configurationUtility;
            this._apiService = apiService;
            this._accessControlService = accessControlService;
        }

        public async Task PrepareViewForShow(string entityName)
        {
            var currentRole = this._cookieUtility.GetCookie(CurrentRoleCookieTitle);

            ViewData[CurrentRoleCookieTitle] = currentRole;

            ViewData["ShowAddAction"] = await this._accessControlService.HasAccess(AccessAction.SpecificPermission, currentRole, $"Add{entityName}");
            ViewData["ShowUpdateAction"] = await this._accessControlService.HasAccess(AccessAction.SpecificPermission, currentRole, $"Edit{entityName}");
            ViewData["ShowDeleteAction"] = await this._accessControlService.HasAccess(AccessAction.SpecificPermission, currentRole, $"Delete{entityName}");

            var menuList = new List<Tuple<string, string>>
            {
                Tuple.Create("Products", "/DataTable/Main")
            };

            if (await this._accessControlService.HasAccess(AccessAction.CheckAtLeastOnePermission, currentRole, "Category"))
            {
                ViewData["IsCellLink"] = true;
                menuList.Add(Tuple.Create("Categories", "/DataTable/Categories"));
            }

            if (await this._accessControlService.HasAccess(AccessAction.CheckAtLeastOnePermission, currentRole, "User"))
            {
                menuList.Add(Tuple.Create("Users", "/DataTable/Users"));
            }

            ViewData["MenuItems"] = menuList;
        }

        [HttpGet]
        public async Task<IActionResult> Categories()
        {
            await PrepareViewForShow("Category");

            var categories = await this._unitOfWork.ProductCategoryService.GetEntries();

            return View(categories);
        }

        [HttpGet]
        public async Task<IActionResult> Users()
        {
            await PrepareViewForShow("User");

            var users = await this._unitOfWork.UserService.GetEntries(u => u.Role.Name != ViewData[CurrentRoleCookieTitle].ToString());

            return View(users);
        }

        private async Task<IActionResult> GetMainPage(List<Product> products)
        {
            await PrepareViewForShow("Product");

            ViewData["ShowSpecialNoteColumn"] = (ViewData[CurrentRoleCookieTitle] != null) ? await this._accessControlService.HasAccess(AccessAction.SpecificPermission,
                                                                                                                                        ViewData[CurrentRoleCookieTitle].ToString(),
                                                                                                                                        "ViewSpecialNoteColumn")
                                                                                           : true;

            var currencies = await this._apiService.ExecuteRequest<List<Currency>>(NationalBankAction.GetCurrencies);
            ViewData["Currencies"] = currencies.GroupBy(currency => currency.Cur_Name_Eng)
                                               .Select(group => group.First())
                                               .Select(currency => Tuple.Create(currency.Cur_Name_Eng, currency.Cur_Abbreviation))
                                               .ToList();

            ViewData["DefaultCurrencyToConvert"] = this._configurationUtility.GetValue("NBRB:DefaultCurrencyToConvert");

            if ((await this._unitOfWork.ProductCategoryService.GetEntries()).Count == 0)
            {
                ViewData["ShowAddAction"] = false;
            }

            return View("Main", products);
        }

        [HttpGet]
        public async Task<IActionResult> Main()
        {
            var products = await this._unitOfWork.ProductService.GetEntries();

            return await GetMainPage(products);
        }

        [HttpGet, Route("/DataTable/RemoveCategory/{Id}")]
        public async Task<IActionResult> RemoveCategory(Guid Id)
        {
            await this._unitOfWork.ProductCategoryService.RemoveEntry(new ProductCategory { Id = Id });

            return RedirectToAction("Categories", "DataTable");
        }

        [HttpGet, Route("/DataTable/RemoveUser/{Id}")]
        public async Task<IActionResult> RemoveUser(Guid Id)
        {
            await this._unitOfWork.UserService.RemoveEntry(new User { Id = Id });

            return RedirectToAction("Users", "DataTable");
        }

        [HttpGet, Route("/DataTable/RemoveProduct/{Id}")]
        public async Task<IActionResult> RemoveProduct(Guid Id)
        {
            await this._unitOfWork.ProductService.RemoveEntry(new Product { Id = Id });
            
            return RedirectToAction("Main", "DataTable");
        }

        [HttpGet]
        public IActionResult Exit()
        {
            this._cookieUtility.RemoveCookie("CurrentUserRole");

            return RedirectToAction("Authorization", "Authentication");
        }

        [HttpGet]
        public async Task<IActionResult> ExchangeCurrency(decimal price, string selectedCurrencyAbbreviation)
        {
            var selectedCurrencyRate = await this._apiService.ExecuteRequest<Rate>(NationalBankAction.GetCurrency, selectedCurrencyAbbreviation);

            return Content(price.ConvertToCurrencyString(selectedCurrencyRate.Cur_OfficialRate, selectedCurrencyRate.Cur_Abbreviation));
        }

        [HttpGet]
        public async Task<IActionResult> FindProducts(string searchText)
        {
            var products = await this._unitOfWork.ProductService.GetEntries(p => (searchText == null) ||
                                                                                 p.Name.Contains(searchText) ||
                                                                                 p.Description!.Contains(searchText) ||
                                                                                 p.Category.Name.Contains(searchText) ||
                                                                                 p.GeneralNote!.Contains(searchText) ||
                                                                                 p.SpecialNote!.Contains(searchText) ||
                                                                                 p.Price.ToString().Contains(searchText));

            ViewData["SearchText"] = searchText;

            return await GetMainPage(products);
        }

        [HttpGet]
        public async Task<IActionResult> FilterProducts(string MinPrice, string MaxPrice)
        {
            var minPrice = Convert.ToDecimal(MinPrice.Replace(".", ","));
            var maxPrice = Convert.ToDecimal(MaxPrice.Replace(".", ","));

            var products = await this._unitOfWork.ProductService.GetEntries(p => (minPrice <= maxPrice) &&
                                                                                 (minPrice == default || p.Price >= minPrice) &&
                                                                                 (maxPrice == default || p.Price <= maxPrice)
                                                                            );

            ViewData["MinPriceFilter"] = minPrice;
            ViewData["MaxPriceFilter"] = maxPrice;

            return await GetMainPage(products);
        }

        [HttpGet, Route("/DataTable/CategoryProducts/{Id}")]
        public async Task<IActionResult> CategoryProducts(Guid Id)
        {
            var category = await this._unitOfWork.ProductCategoryService.GetFullEntry(new ProductCategory { Id = Id });

            await PrepareViewForShow("Category");

            return View(category);
        }
    }
}
