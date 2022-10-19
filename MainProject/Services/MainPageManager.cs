using System;

using MainProject.Contracts.Entities;
using MainProject.Contracts.Entities.ValueObjects;

namespace MainProject.Services
{
    internal static class MainPageManager
    {
        private static readonly SaleInfo BestSaleInfo = new()
        {
            Rating = 10,
            IsActive = true,
            IsBestSeller = true
        };

        private static readonly SaleInfo TrashSaleInfo = new()
        {
            Rating = 0,
            IsActive = false,
            IsBestSeller = false
        };

        public static bool IsForMainPage(Item item)
        {
            if (item.Sellers.Length == 0 || item.SaleInfo.Equals(TrashSaleInfo))
            {
                return false;
            }

            var price = item.Price;
            if (item.SaleInfo.Equals(BestSaleInfo) && CheckMinThreshold(ref price))
            {
                return true;
            }

            return false;
        }

        private static bool CheckMinThreshold(ref Price price)
        {
            return price.Currency switch
            {
                "RUB" => price.Value > 1000,
                "EUR" => price.Value > 10,
                "USD" => price.Value > 10,
                _=> throw new Exception("Unknown type")
            };
        }
    }
}