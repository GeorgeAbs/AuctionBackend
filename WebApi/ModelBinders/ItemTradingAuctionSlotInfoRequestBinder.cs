using Domain.Interfaces.Services.ItemService.ItemTradingService.Dto.AuctionItem;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json;
using WebApi.DTO.Item.Request;

namespace WebApi.ModelBinders
{
    public class ItemTradingAuctionSlotInfoRequestBinder : IModelBinder
    {
        private static JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions
        { 
            WriteIndented = true,
            PropertyNameCaseInsensitive = true
        };
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            CreateUpdateAuctionItemRequest result = new ();

            var formFiles = bindingContext.HttpContext.Request.Form.Files;

            foreach (var file in formFiles)
            {
                //result.Images.Add(file);
            }

            var dtoValue = bindingContext.ValueProvider.GetValue("ItemInfo");

            if (dtoValue == ValueProviderResult.None || string.IsNullOrEmpty(dtoValue.FirstValue) ) 
            {
                bindingContext.Result = ModelBindingResult.Failed();
                return Task.CompletedTask;
            }

            Console.WriteLine(dtoValue.FirstValue);
            ItemTradingAuctionInfoRequest? itemInfo = JsonSerializer.Deserialize<ItemTradingAuctionInfoRequest>(dtoValue.FirstValue, _jsonSerializerOptions);
            Console.WriteLine("res \n" + JsonSerializer.Serialize(itemInfo, _jsonSerializerOptions));

            if (itemInfo is null)
            {
                bindingContext.Result = ModelBindingResult.Failed();
                return Task.CompletedTask;
            }

            //result.ItemInfo = itemInfo;

            bindingContext.Result = ModelBindingResult.Success(result);
            return Task.CompletedTask;
        }
    }
}
