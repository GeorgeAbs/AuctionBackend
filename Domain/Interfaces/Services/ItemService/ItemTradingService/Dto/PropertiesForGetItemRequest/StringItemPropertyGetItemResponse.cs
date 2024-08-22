using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Common.Dto;

namespace Domain.Interfaces.Services.ItemService.ItemTradingService.Dto.PropertiesForGetItemRequest
{
    public class StringItemPropertyGetItemResponse : TextItemPropertyGetItemResponse
    {
        public StringItemPropertyGetItemResponse(string value, string propertyName) : base(value, propertyName)
        {
        }
    }
}
