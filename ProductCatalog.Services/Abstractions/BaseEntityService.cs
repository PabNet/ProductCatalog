using Microsoft.Extensions.Logging;
using ProductCatalog.Domain.Abstractions;

namespace ProductCatalog.Services.Abstractions
{
    public class BaseEntityService
    {
        protected ProductCatalog.Domain.Abstractions.IUnitOfWork _unitOfWork { get; set; }
        protected ILogger _logger { get; set; }

        public BaseEntityService(ProductCatalog.Domain.Abstractions.IUnitOfWork unitOfWork, ILogger<BaseEntityService> logger)
        {
            this._unitOfWork = unitOfWork;
            this._logger = logger;
        }
    }
}
