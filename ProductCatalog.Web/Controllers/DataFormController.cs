using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Domain.Models.Authentication;
using ProductCatalog.Domain.Models.Core;
using ProductCatalog.Services.Abstractions;
using ProductCatalog.Utility.Helpers;
using ProductCatalog.Web.Controllers.Templates;

namespace ProductCatalog.Web.Controllers
{
    public class DataFormController : BaseController
    {
        public DataFormController(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public void PrepareViewForShow(Guid Id = default)
        {
            ViewData["ButtonLabel"] = (Id == default) ? "Add" : "Update";
        }

        [HttpGet, Route("/DataForm/Category")]
        public IActionResult Category()
        {
            PrepareViewForShow();

            return View();
        }

        [HttpGet, Route("/DataForm/Category/{Id}")]
        public async Task<IActionResult> Category(Guid Id)
        {
            PrepareViewForShow(Id);

            var category = await this._unitOfWork.ProductCategoryService.GetFullEntry(new ProductCategory { Id = Id});

            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> Category(ProductCategory category)
        {
            var isAddMode = category.Id == default;

            var isOperationSuccess = isAddMode ? await this._unitOfWork.ProductCategoryService.AddEntryAsync(category)
                                               : await this._unitOfWork.ProductCategoryService.UpdateEntry(category);


            if (isOperationSuccess)
            {
                return RedirectToAction("Categories", "DataTable");
            }
            else
            {
                ViewData["ErrorMessage"] = $"Failed to {(isAddMode ? "add" : "update")} product category. Check the entered data and try again";

                PrepareViewForShow(category.Id);

                return View(category);
            }
        }

        [HttpGet, Route("/DataForm/Product")]
        public async Task<IActionResult> Product()
        {
            PrepareViewForShow();

            ViewData["Categories"] = await this._unitOfWork.ProductCategoryService.GetEntries();

            return View();
        }

        [HttpGet, Route("/DataForm/Product/{Id}")]
        public async Task<IActionResult> Product(Guid Id)
        {
            PrepareViewForShow(Id);

            ViewData["Categories"] = await this._unitOfWork.ProductCategoryService.GetEntries();

            var product = await this._unitOfWork.ProductService.GetFullEntry(new Product { Id = Id });

            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Product(Product product)
        {
            var isAddMode = product.Id == default;

            var isOperationSuccess = isAddMode ? await this._unitOfWork.ProductService.AddEntryAsync(product)
                                               : await this._unitOfWork.ProductService.UpdateEntry(product);


            if (isOperationSuccess)
            {
                return RedirectToAction("Main", "DataTable");
            }
            else
            {
                ViewData["ErrorMessage"] = $"Failed to {(isAddMode ? "add" : "update")} product. Check the entered data and try again";

                PrepareViewForShow(product.Id);

                return View(product);
            }
        }

        [HttpGet, Route("/DataForm/User")]
        public async Task<IActionResult> User()
        {
            PrepareViewForShow();

            ViewData["Roles"] = await this._unitOfWork.UserRoleService.GetEntries();

            return View();
        }

        [HttpGet, Route("/DataForm/User/{Id}")]
        public async Task<IActionResult> User(Guid Id)
        {
            PrepareViewForShow(Id);

            ViewData["Roles"] = await this._unitOfWork.UserRoleService.GetEntries();

            var user = await this._unitOfWork.UserService.GetFullEntry(new User { Id = Id });

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> User(User user)
        {
            var isAddMode = user.Id == default;

            var isOperationSuccess = isAddMode ? await this._unitOfWork.UserService.AddEntryAsync(user) 
                                               : await this._unitOfWork.UserService.UpdateEntry(user);


            if (isOperationSuccess)
            {
                return RedirectToAction("Users", "DataTable");
            }
            else
            {
                ViewData["ErrorMessage"] = $"Failed to {(isAddMode ? "add" : "update")} user. Check the entered data and try again";

                PrepareViewForShow(user.Id);

                return View(user);
            }
        }
    }
}
