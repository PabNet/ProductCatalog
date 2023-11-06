using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Services.Abstractions;

namespace ProductCatalog.Web.Controllers.Templates
{
    public class BaseController : Controller
    {
        protected IUnitOfWork _unitOfWork { get; set; }

        public BaseController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    }
}
