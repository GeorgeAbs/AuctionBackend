namespace Domain.Entities.Addresses
{
    public class UserAddress : AddressBase
    {
        public Guid UserId { get; private set; }

        public string AddressTitle { get; private set; }

        public bool IsForShipment { get; private set; } = false;

        public bool IsDefaultForShipment { get; private set; } = false;

        public bool IsForReceiving { get; private set; } = false;

        public bool IsDefaultForReceiving { get; private set; } = false;

        public UserAddress(Guid userId,
            string addressTitle,
            string country,
            string city,
            string region,
            string district,
            string street,
            string building,
            string floor,
            string flat,
            string postIndex,
            bool isForShipment,
            bool isDefaultForShipment,
            bool isForReceiving,
            bool isDefaultForReceiving) : base(country, city, region, district, street, building, floor, flat, postIndex)
        {
            UserId = userId;
            AddressTitle = addressTitle;
            IsForShipment = isForShipment;
            IsDefaultForShipment = isDefaultForShipment;
            IsForReceiving = isForReceiving;
            IsDefaultForReceiving = isDefaultForReceiving;
        }

        public void ChangeAddress(string addressTitle,
            string country,
            string city,
            string region,
            string district,
            string street,
            string building,
            string floor,
            string flat,
            string postIndex,
            bool isForShipment,
            bool isDefaultForShipment,
            bool isForReceiving,
            bool isDefaultForReceiving)
        {
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
