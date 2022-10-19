using MainProject.Contracts.Entities.ValueObjects;

namespace MainProject.Contracts.Entities
{
    public sealed class Item
    {
        public long Id { get; set; }
        public VolumeWeightData VolumeWeight { get; set; }
        public Price Price { get; set; }
        public Seller[] Sellers { get; set; }
        public SaleInfo SaleInfo { get; set; }
    }
}