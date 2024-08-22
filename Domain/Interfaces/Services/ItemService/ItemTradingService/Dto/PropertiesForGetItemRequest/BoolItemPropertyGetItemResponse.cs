using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Common.Dto;

namespace Domain.Interfaces.Services.ItemService.ItemTradingService.Dto.PropertiesForGetItemRequest
{
    public class BoolItemPropertyGetItemResponse : TextItemPropertyGetItemResponse
    {
        public BoolItemPropertyGetItemResponse(string value, string propertyName) : base(value, propertyName)
        {
        }
    }
}
