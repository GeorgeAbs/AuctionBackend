using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Services.ItemService.ItemTradingService.Dto
{
    public class AdressTitleResponse
    {
        public Guid AdressId { get; }

        public AdressTitleResponse(Guid adressId, string adressName)
        {
            AdressId = adressId;
        }
    }
}
