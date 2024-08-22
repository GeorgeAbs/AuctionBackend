using Domain.BackendResponses;
using Domain.Interfaces.Services.CatalogService.Dto.Catalog;
using Domain.Interfaces.Services.CatalogService.Dto.PropsCreation;

namespace Domain.Interfaces.Services.CatalogService
{
    public interface ICatalogService
    {
        public Task<MethodResult> CreateCatalogCategoryAsync(CatalogCategoryCreationRequest catalogCategoryDto);

        public Task<MethodResult> CreateCatalogPropertyNameAsync(CatalogPropertyNameCreationRequest catalogPropertyNameDto);

        public Task<MethodResult> CreateCatalogPropertyAsync(CatalogStringPropertyCreationRequest? stringPropertyDto, CatalogBoolPropertyCreationRequest? boolPropertyDto);

        public Task<CatalogResponse?> GetCatalogAsync();

        public Task<CatalogCategoryFullFilter?> GetCatalogFilterForEditingAsync(Guid catalogCategoryId);
    }
}
