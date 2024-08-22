namespace Domain.Entities.Addresses
{
    public class SlotPreorderAddress : AddressBase
    {
        public SlotPreorderAddress(string country, string city, string region, string district, string street, string building, string floor, string flat, string postIndex) 
            : base(country, city, region, district, street, building, floor, flat, postIndex)
        {
        }
    }
}
