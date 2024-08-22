namespace Domain.Entities.Addresses
{
    public abstract class AddressBase : EntityBase
    {

        public string Country { get; protected set; }

        public string City { get; protected set; }

        public string Region { get; protected set; }

        public string District { get; protected set; }

        public string Street { get; protected set; }

        public string Building { get; protected set; }

        public string Floor { get; protected set; }

        public string Flat { get; protected set; }

        public string PostIndex { get; protected set; }

        public AddressBase(string country, string city, string region, string district, string street, string building, string floor, string flat, string postIndex)
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
        }
    }
}
