using Domain.Entities.Items.ItemTrading;

namespace Domain.Entities.Addresses
{
    public class ItemAddress : AddressBase
    {
        public ItemTrading Item { get; private set; }

        public string AddressTitle { get; private set; }

        private ItemAddress(string addressTitle,
            string country,
            string city,
            string region,
            string district,
            string street,
            string building,
            string floor,
            string flat,
            string postIndex) : base(country, city, region, district, street, building, floor, flat, postIndex) { }

        public ItemAddress(ItemTrading item,
            string addressTitle,
            string country,
            string city,
            string region,
            string district,
            string street,
            string building,
            string floor,
            string flat,
            string postIndex) : base(country, city, region, district, street, building, floor, flat, postIndex)
        {
            Item = item;
            AddressTitle = addressTitle;
        }
    }
}
