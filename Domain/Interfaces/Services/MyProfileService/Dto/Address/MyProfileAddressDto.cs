namespace Domain.Interfaces.Services.MyProfileService.Dto.Address
{
    public class MyProfileAddressDto
    {
        public Guid AddressId { get; private set; }

        public string AddressTitle { get; private set; } = string.Empty;

        public string Country { get; private set; } = string.Empty;

        public string City { get; private set; } = string.Empty;

        public string Region { get; private set; } = string.Empty;

        public string District { get; private set; } = string.Empty;

        public string Street { get; private set; } = string.Empty;

        public string Building { get; private set; } = string.Empty;

        public string Floor { get; private set; } = string.Empty;

        public string Flat { get; private set; } = string.Empty;

        public string PostIndex { get; private set; } = string.Empty;

        public bool IsForShipment { get; private set; } = false;

        public bool IsDefaultForShipment { get; private set; } = false;

        public bool IsForReceiving { get; private set; } = false;

        public bool IsDefaultForReceiving { get; private set; } = false;

        public MyProfileAddressDto(Guid addressId, string addressTitle, string country, string city, string region, string district, string street, string building, string floor, string flat, string postIndex, bool isForShipment, bool isDefaultForShipment, bool isForReceiving, bool isDefaultForReceiving)
        {
            AddressId = addressId;
            AddressTitle = addressTitle;
            Country = country;
            City = city;
            Region = region;
            District = district;
            Street = street;
            Building = building;
            Floor = floor;
            Flat = flat;
            PostIndex = postIndex;
            IsForShipment = isForShipment;
            IsDefaultForShipment = isDefaultForShipment;
            IsForReceiving = isForReceiving;
            IsDefaultForReceiving = isDefaultForReceiving;
        }
    }
}
