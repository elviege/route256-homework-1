using MainProject.Contracts.Entities.ValueObjects;
using MainProject.Enums;

namespace MainProject.Services
{
    internal static class ItemSizeManager
    {
        public static SizeType GetSizeType(ref VolumeWeightData data)
        {
            var allSize = new long[]
                { data.Height, data.Length, data.Width, data.PackagedHeight, data.PackagedLength, data.PackagedWidth };

            long maxSize = 0;
            for(var i = 0; i < allSize.Length; i++)
            {
                if (allSize[i] > maxSize)
                    maxSize = allSize[i];
            }

            if (maxSize > 100)
            {
                return SizeType.Max;
            }

            var maxWeight = data.PackagedWeight >= data.Weight ? data.PackagedWeight : data.Weight;
            if (maxWeight > 10000)
            {
                return SizeType.Max;
            }

            long packagedVolume = data.PackagedHeight * data.PackagedLength * data.PackagedWidth;
            long volume = data.Height * data.Length * data.Width;

            long minVolume = packagedVolume <= volume ? packagedVolume : volume;

            if (minVolume < 10000)
            {
                return SizeType.Small;
            }

            if (minVolume < 30000)
            {
                return SizeType.Medium;
            }

            return SizeType.Max;
        }
    }
}