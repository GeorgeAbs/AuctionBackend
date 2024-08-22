using Domain.BackendResponses;
using Domain.Entities.Catalog;
using Domain.Entities.Catalog.CatalogProperty;
using Domain.Entities.Catalog.CatalogProperty.CatalogPropertiesNames;
using Domain.Interfaces.DbContexts;
using Domain.Interfaces.Services.CatalogService;
using Domain.Interfaces.Services.CatalogService.Dto.Catalog;
using Domain.Interfaces.Services.CatalogService.Dto.CatalogFilter.PropsEditing;
using Domain.Interfaces.Services.CatalogService.Dto.CatalogFilter.PropsNamesEditing;
using Domain.Interfaces.Services.CatalogService.Dto.PropsCreation;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly ICatalogDbContext _context;
        public CatalogService(ICatalogDbContext context) => _context = context;

        async public Task<MethodResult> CreateCatalogCategoryAsync(CatalogCategoryCreationRequest catalogCategoryDto)
        {
            var catalogCategory = await _context.CatalogCategories
                .FirstOrDefaultAsync(x => x.SystemName == catalogCategoryDto.SystemName);

            if (catalogCategory is not null) 
            {
                return new MethodResult(["Такая категория уже существует"], Domain.CoreEnums.Enums.MethodResults.Conflict);
            }

            if (!string.IsNullOrEmpty(catalogCategoryDto.ParentCatalogCategorySystemName))
            {
                var parentCategory = await _context.CatalogCategories
                    .Where(x => x.SystemName == catalogCategoryDto.ParentCatalogCategorySystemName)
                    .FirstOrDefaultAsync();

                if (parentCategory is null)
                {
                    return new MethodResult(["Родительская категория не найдена"], Domain.CoreEnums.Enums.MethodResults.Conflict);
                }

                CatalogCategory newCatalogCategory = new(catalogCategoryDto.Name, catalogCategoryDto.SystemName, parentCategory);

                parentCategory.AddChildrenCategory(newCatalogCategory);

                _context.CatalogCategories.Add(newCatalogCategory);
            }

            else
            {
                CatalogCategory newCatalogCategory = new(catalogCategoryDto.Name, catalogCategoryDto.SystemName);

                _context.CatalogCategories.Add(newCatalogCategory);
            }

            await _context.SaveChangesAsync();

            return new MethodResult(["Успех"], Domain.CoreEnums.Enums.MethodResults.Ok);
        }

        public async Task<MethodResult> CreateCatalogPropertyNameAsync(CatalogPropertyNameCreationRequest catalogPropertyNameDto)
        {

            var catalogCategory = await _context.CatalogCategories.FirstOrDefaultAsync(x => x.SystemName == catalogPropertyNameDto.CatalogCategorySystemName);

            if (catalogCategory is null)
            {
                return new MethodResult(["Категория каталога не найдена"], Domain.CoreEnums.Enums.MethodResults.Conflict);
            }

            switch (catalogPropertyNameDto.CatalogPropertyType)
            {
                case Domain.CoreEnums.Enums.CatalogPropertyTypes.String:
                    var count = await _context.CatalogStringPropertyNames.CountAsync(x => x.SystemName == catalogPropertyNameDto.SystemName);
                    if (count > 0)
                    {
                        return new MethodResult(["Такое имя свойства уже существует"], Domain.CoreEnums.Enums.MethodResults.Conflict);
                    }
                    _context.CatalogStringPropertyNames.Add(new CatalogStringPropertyName(catalogCategory, catalogPropertyNameDto.Name, catalogPropertyNameDto.SystemName));
                    break;

                case Domain.CoreEnums.Enums.CatalogPropertyTypes.Bool:
                    count = await _context.CatalogBoolPropertyNames.CountAsync(x => x.SystemName == catalogPropertyNameDto.SystemName);
                    if (count > 0)
                    {
                        return new MethodResult(["Такое имя свойства уже существует"], Domain.CoreEnums.Enums.MethodResults.Conflict);
                    }
                    _context.CatalogBoolPropertyNames.Add(new CatalogBoolPropertyName(catalogCategory, catalogPropertyNameDto.Name, catalogPropertyNameDto.SystemName));
                    break;

                case Domain.CoreEnums.Enums.CatalogPropertyTypes.Float:
                    count = await _context.CatalogFloatPropertyNames.CountAsync(x => x.SystemName == catalogPropertyNameDto.SystemName);
                    if (count > 0)
                    {
                        return new MethodResult(["Такое имя свойства уже существует"], Domain.CoreEnums.Enums.MethodResults.Conflict);
                    }
                    _context.CatalogFloatPropertyNames.Add(new CatalogFloatPropertyName(catalogCategory, catalogPropertyNameDto.Name, catalogPropertyNameDto.SystemName));
                    break;

                case Domain.CoreEnums.Enums.CatalogPropertyTypes.Int:
                    count = await _context.CatalogIntPropertyNames.CountAsync(x => x.SystemName == catalogPropertyNameDto.SystemName);
                    if (count > 0)
                    {
                        return new MethodResult(["Такое имя свойства уже существует"], Domain.CoreEnums.Enums.MethodResults.Conflict);
                    }
                    _context.CatalogIntPropertyNames.Add(new CatalogIntPropertyName(catalogCategory, catalogPropertyNameDto.Name, catalogPropertyNameDto.SystemName));
                    break;
            }

            await _context.SaveChangesAsync();
            return new MethodResult(["Успех"], Domain.CoreEnums.Enums.MethodResults.Ok);
        }

        public async Task<CatalogResponse?> GetCatalogAsync()
        {
            List<CatalogCategory> catalogCategories = await _context.CatalogCategories.ToListAsync();

            if (catalogCategories.Count() == 0) return null; //нет категорий

            var zeroLevels = catalogCategories.Where(x => x.ParentCatalogCategory == null);

            if (zeroLevels.Count() == 0) return null; //нет нулевого уровня

            List<CatalogCategoryResponse> categories = [];

            foreach(var zeroLevel in zeroLevels) 
            {
                var rootCatalogCategory = new CatalogCategoryResponse()
                {
                    Name = zeroLevel.Name,
                    SystemName = zeroLevel.SystemName,
                    ChildrenCategories = CreateChildrenCatalogCategoriesDto(zeroLevel.ChildrenCatalogCategories)
                };

                categories.Add(rootCatalogCategory);
            }

            CatalogResponse catalog = new(categories);

            return catalog;
        }

        private List<CatalogCategoryResponse> CreateChildrenCatalogCategoriesDto(IEnumerable<CatalogCategory> categories)
        {
            List<CatalogCategoryResponse> categoriesDto = new();

            foreach (var category in categories)
            {
                categoriesDto.Add(new CatalogCategoryResponse()
                {
                    Name = category.Name,
                    SystemName= category.SystemName,
                    ChildrenCategories = CreateChildrenCatalogCategoriesDto(category.ChildrenCatalogCategories)
                });
            }

            return categoriesDto;
        }

        public async Task<MethodResult> CreateCatalogPropertyAsync(CatalogStringPropertyCreationRequest? stringPropertyDto, CatalogBoolPropertyCreationRequest? boolPropertyDto)
        {
            if (stringPropertyDto is not null)
            {
                var propName = await _context.CatalogStringPropertyNames
                    .Include(x => x.Properties)
                    .FirstOrDefaultAsync(x => x.Id == stringPropertyDto.CatalogStringPropNameId);

                if (propName is not null && !propName.Properties.Any(x => x.PropertyValue == stringPropertyDto.Value))
                {
                    var newProp = new CatalogStringProperty(stringPropertyDto.Value, propName, stringPropertyDto.SystemValue);
                    _context.CatalogItemStringProperties.Add(newProp);
                }

                else
                {
                    return new MethodResult(["Не найдено имя характеристки или хараткеристика с таким значением уже существует"], Domain.CoreEnums.Enums.MethodResults.Conflict);
                }
            }

            if (boolPropertyDto is not null)
            {
                var propName = await _context.CatalogBoolPropertyNames
                    .Include(x => x.Properties)
                    .FirstOrDefaultAsync(x => x.Id == boolPropertyDto.CatalogStringPropNameId);

                if (propName is not null && propName.Properties.Count() == 0 && boolPropertyDto.TrueValue.Length > 0 && boolPropertyDto.FalseValue.Length > 0)
                {
                    var newPropTrue = new CatalogBoolProperty(boolPropertyDto.TrueValue, propName, boolPropertyDto.SystemTrueValue);
                    var newPropFalse = new CatalogBoolProperty(boolPropertyDto.FalseValue, propName, boolPropertyDto.SystemFalseValue);
                    _context.CatalogItemBoolProperties.Add(newPropTrue);
                    _context.CatalogItemBoolProperties.Add(newPropFalse);
                }

                else
                {
                    return new MethodResult(["Не найдено имя характеристки или булева хараткеристика уже заполнена или одно из значений пустое"], Domain.CoreEnums.Enums.MethodResults.Conflict);
                }
            }

            await _context.SaveChangesAsync();

            return new MethodResult(["Успешно"], Domain.CoreEnums.Enums.MethodResults.Ok);
        }

        public async Task<CatalogCategoryFullFilter?> GetCatalogFilterForEditingAsync(Guid catalogCategoryId)
        {
            var catalog = await _context.CatalogCategories
                .Select(c => new
                {
                    c.Id,
                    BoolPropNames = c.CatalogBoolPropertyNames
                                    .Select(p => new EditingBoolPropNameResponse(
                                                p.Id, p.Name, p.Properties.Select(pp => new EditingBoolProp(pp.Id, pp.PropertyValue)))),

                    StringPropNames = c.CatalogStringPropertyNames
                                    .Select(p => new EditingStringPropNameResponse(
                                                p.Id, p.Name, p.Properties.Select(pp => new EditingStringProp(pp.Id, pp.PropertyValue)))),

                    IntPropNames = c.CatalogIntPropertyNames
                                    .Select(p => new EditingIntPropNameResponse(
                                         p.Id, p.Name)),

                    FloatPropNames = c.CatalogFloatPropertyNames
                                    .Select(p => new EditingFloatPropNameResponse(
                                         p.Id, p.Name))
                })
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == catalogCategoryId);

            return new CatalogCategoryFullFilter(catalogCategoryId,
                catalog?.StringPropNames,
                catalog?.BoolPropNames,
                catalog?.FloatPropNames,
                catalog?.IntPropNames);
        }
    }
}
