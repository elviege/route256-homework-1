using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;

using MainProject.Contracts.Entities;
using MainProject.Contracts.Externals;
using MainProject.Enums;
using MainProject.Services;

namespace MainProject
{
    public class Program
    {
        [MemoryDiagnoser]
        public class IntroSetupCleanupGlobal
        {
            private IEnumerable<ItemDto> _dataFromExternalService;

            private static ItemDto GetByIndex(int value)
            {
                int index = value % 5;

                ItemDto itemDto = index switch
                {
                    0 => JsonSerializer.Deserialize<ItemDto>(ItemDtoGenerator.RawDataItem0),
                    1 => JsonSerializer.Deserialize<ItemDto>(ItemDtoGenerator.RawDataItem1),
                    2 => JsonSerializer.Deserialize<ItemDto>(ItemDtoGenerator.RawDataItem2),
                    3 => JsonSerializer.Deserialize<ItemDto>(ItemDtoGenerator.RawDataItem3),
                    4 => JsonSerializer.Deserialize<ItemDto>(ItemDtoGenerator.RawDataItem4),
                    _=> throw new Exception("Unknown index")
                };

                itemDto!.Id = value;

                return itemDto;
            }

            [GlobalSetup]
            public void GlobalSetup()
            {
                var n = 5000;
                var arr = new ItemDto[n];
                for (var i = 1; i <= n; i++)
                {
                    arr[i-1] = GetByIndex(i);
                }
                _dataFromExternalService = arr;
            }

            [Benchmark]
            public void Process()
            {
                var converter = new ItemConverter();

                var dtoArr = _dataFromExternalService.ToArray();
                Item[] items = converter.ConvertItems(dtoArr);

                var itemsBySize = new Dictionary<SizeType, List<Item>>
                {
                    { SizeType.Max, new List<Item>() },
                    { SizeType.Medium, new List<Item>() },
                    { SizeType.Small, new List<Item>() }
                };
                for (var i = 0; i < items.Length; i++)
                {
                    var volumeWeight = items[i].VolumeWeight;
                    var size = ItemSizeManager.GetSizeType(ref volumeWeight);
                    itemsBySize[size].Add(items[i]);
                }

                ItemSaver.SaveBySize(itemsBySize);

                var itemsForMainPage = new List<Item>();
                for (var i = 0; i < items.Length; i++)
                {
                    if (!MainPageManager.IsForMainPage(items[i])) continue;
                    itemsForMainPage.Add(items[i]);
                }

                ItemSaver.SendItemsForMainPage(itemsForMainPage);
            }
        }

        private static void Main(string[] args)
        {
            Summary[] summary = BenchmarkRunner.Run(typeof(Program).Assembly);
        }
    }
}