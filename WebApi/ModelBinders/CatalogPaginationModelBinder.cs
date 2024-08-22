using Application;
using Domain.Interfaces.Services.CatalogPaginationService.Dto.Filtration.PropsNames.Request;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using WebApi.DTO.CatalogPagination.Request;
using static Domain.CoreEnums.Enums;
namespace WebApi.ModelBinders
{
    public class CatalogPaginationModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            int pageNumber;

            int pageSize;

            SortingMethods sorting;

            float minPrice;

            float maxPrice;

            List<FilterFloatNameRequest> filterFloatNames = [];

            List<FilterIntNameRequest> filterIntNames = [];

            List<FilterStringNameRequest> filterStringNames = [];

            List<FilterBoolNameRequest> filterBoolNames = [];

            SellingTypes sellingType;

            var pageNumberValue = bindingContext.ValueProvider.GetValue("page");
            var pageSizeValue = bindingContext.ValueProvider.GetValue("size");
            var sortingValue = bindingContext.ValueProvider.GetValue("sort");
            var categoryNameValue = bindingContext.ValueProvider.GetValue("category");
            var minPriceValue = bindingContext.ValueProvider.GetValue("min-price");
            var maxPriceValue = bindingContext.ValueProvider.GetValue("max-price");
            var filterFloatNamesValue = bindingContext.ValueProvider.GetValue("f");
            var filterIntNamesValue = bindingContext.ValueProvider.GetValue("i");
            var filterStringNamesValue = bindingContext.ValueProvider.GetValue("s");
            var filterBoolNamesValue = bindingContext.ValueProvider.GetValue("b");
            var sellingTypeValue = bindingContext.ValueProvider.GetValue("sell");

            var filter = new CatalogPagingFilterRequest()
            {
                MinPrice = 0,
                MaxPrice = 99999999999f,
                PageNumber = 1,
                PageSize = Settings.HISTORY_PAGE_SIZE
            };

            if (pageNumberValue != ValueProviderResult.None && int.TryParse(pageNumberValue.FirstValue, out pageNumber))
            {
                filter.PageNumber = pageNumber;
            }

            if (pageSizeValue != ValueProviderResult.None && int.TryParse(pageSizeValue.FirstValue, out pageSize))
            {
                filter.PageSize = pageSize;
            }

            if (sortingValue != ValueProviderResult.None && Enum.TryParse(sortingValue.FirstValue, out sorting))
            {
                filter.Sorting = sorting;
            }

            if (categoryNameValue != ValueProviderResult.None)
            {
                if(categoryNameValue.FirstValue is not null)
                    filter.CategoryName = categoryNameValue.FirstValue;
            }

            if (minPriceValue != ValueProviderResult.None && float.TryParse(minPriceValue.FirstValue, out minPrice))
            {
                filter.MinPrice = minPrice;
            }

            if (maxPriceValue != ValueProviderResult.None && float.TryParse(maxPriceValue.FirstValue, out maxPrice))
            {
                filter.MaxPrice = maxPrice;
            }

            if (filterFloatNamesValue != ValueProviderResult.None && filterFloatNamesValue.FirstValue is not null)
            {
                var splittedNames = filterFloatNamesValue.FirstValue.Split('~', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                foreach (var splittedName in splittedNames)
                {
                    var splittedValues = splittedName.Split('-'); // dont remove empty entries cos max or min value might not be specified

                    if (splittedValues.Length != 3) continue;

                    var propName = splittedValues[0];

                    float minValue = float.MinValue;
                    float maxValue = float.MaxValue;

                    float.TryParse(splittedValues[1], out minValue);
                    float.TryParse(splittedValues[2], out maxValue);

                    if (maxValue < minValue) continue;

                    filterFloatNames.Add(new FilterFloatNameRequest(propName, minValue, maxValue));
                }

                filter.FilterFloatNames = filterFloatNames;
            }

            if (filterIntNamesValue != ValueProviderResult.None && filterIntNamesValue.FirstValue is not null)
            {
                var splittedNames = filterIntNamesValue.FirstValue.Split('~', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                foreach (var splittedName in splittedNames)
                {
                    var splittedValues = splittedName.Split('-');// dont remove empty entries cos max or min value might not be specified

                    if (splittedValues.Length != 3) continue;

                    var propName = splittedValues[0];

                    int minValue = int.MinValue;
                    int maxValue = int.MaxValue;

                    int.TryParse(splittedValues[1], out minValue);
                    int.TryParse(splittedValues[2], out maxValue);

                    if (maxValue < minValue) continue;

                    filterIntNames.Add(new FilterIntNameRequest(propName, minValue, maxValue));
                }

                filter.FilterIntNames = filterIntNames;
            }

            if (filterStringNamesValue != ValueProviderResult.None && filterStringNamesValue.FirstValue is not null)
            {
                var splittedNames = filterStringNamesValue.FirstValue.Split('~', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                foreach (var splittedName in splittedNames)
                {
                    var splittedValues = splittedName.Split('-', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                    if (splittedValues.Length == 1) continue;

                    var propName = splittedValues[0];

                    List<string> values = [];

                     //from 1
                    for (int i = 1; i < splittedValues.Length; i++)
                    {
                        values.Add(splittedValues[i]);
                    }

                    filterStringNames.Add(new FilterStringNameRequest(propName, values));
                }

                filter.FilterStringNames = filterStringNames;
            }

            if (filterBoolNamesValue != ValueProviderResult.None && filterBoolNamesValue.FirstValue is not null)
            {
                var splittedNames = filterBoolNamesValue.FirstValue.Split('~', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                foreach (var splittedName in splittedNames)
                {
                    var splittedValues = splittedName.Split('-', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                    if (splittedValues.Length == 1 || splittedValues.Length > 3) continue;

                    var propName = splittedValues[0];

                    List<string> values = [];

                    //from 1
                    for (int i = 1; i < splittedValues.Length; i++)
                    {
                        values.Add(splittedValues[i]);
                    }

                    filterBoolNames.Add(new FilterBoolNameRequest(propName, values));
                }

                filter.FilterBoolNames = filterBoolNames;
            }

            if (sellingTypeValue != ValueProviderResult.None && Enum.TryParse(sellingTypeValue.FirstValue, out sellingType))
            {
                filter.SellingType = sellingType;
            }

            bindingContext.Result = ModelBindingResult.Success(filter);
            return Task.CompletedTask;
        }
    }
}
