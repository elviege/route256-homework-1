using MainProject.Contracts.Entities;
using MainProject.Contracts.Entities.ValueObjects;
using MainProject.Contracts.Externals;

namespace MainProject.Services
{
    internal sealed class ItemConverter
    {
        public Item[] ConvertItems(ItemDto[] dtoItems)
        {
            var items = new Item[dtoItems.Length];

            for (var i = 0; i< dtoItems.Length; i++)
            {
                items[i] = new Item
                {
                    Id = dtoItems[i].Id,
                    Price = new Price
                    {
                        Currency = dtoItems[i].PriceCurrency,
                        Value = dtoItems[i].PriceValue
                    },
                    Sellers = new Seller[dtoItems[i].SellerIds.Length],
                    VolumeWeight = new VolumeWeightData
                    {
                        Height = dtoItems[i].Height,
                        Length = dtoItems[i].Length,
                        Weight = dtoItems[i].Weight,
                        Width = dtoItems[i].Width,
                        PackagedHeight = dtoItems[i].PackagedHeight,
                        PackagedLength = dtoItems[i].PackagedLength,
                        PackagedWeight = dtoItems[i].PackagedWeight,
                        PackagedWidth = dtoItems[i].PackagedWidth
                    },
                    SaleInfo = new SaleInfo
                    {
                        Rating = dtoItems[i].Rating,
                        IsActive = dtoItems[i].IsActive,
                        IsBestSeller = dtoItems[i].IsBestSeller
                    }
                };
                for (var j = 0; j < dtoItems[i].SellerIds.Length; j++)
                {
                    items[i].Sellers[j] = new Seller { Id = dtoItems[i].SellerIds[j]};
                }
            }

            return items;
        }
    }
}