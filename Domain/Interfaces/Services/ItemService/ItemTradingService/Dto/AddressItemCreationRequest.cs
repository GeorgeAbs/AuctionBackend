namespace Domain.Interfaces.Services.ItemService.ItemTradingService.Dto
{
    public class AddressItemCreationRequest
    {
        public string AddressTitle { get; private set; }

        public string Country { get; private set; }

        public string City { get; private set; }

        public string Region { get; private set; }

        public string District { get; private set; }

        public string Street { get; private set; }

        public string Building { get; private set; }

        public string Floor { get; private set; }

        public string Flat { get; private set; }

        public string PostIndex { get; private set; }

        public AddressItemCreationRequest(string country, string city, string region, string district, string street, string building, string floor, string flat, string postIndex, string addressTitle)
        {
            Country = country;
            City = city;
            Region = region;
            District = district;
            Street = street;
            Building = building;
            Floor = floor;
            Flat = flat;
            PostIndex = postIndex;
            AddressTitle = addressTitle;
        }
    }
}
