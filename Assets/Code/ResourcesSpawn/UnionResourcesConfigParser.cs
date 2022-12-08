using System;
using Code.Configs;


namespace Code.ResourcesSpawn
{
    public class UnionResourcesConfigParser
    {
        public ResourcesConfig WoodConfig { get; private set; }
        public ResourcesConfig StoneConfig { get; private set; }
        public ResourcesConfig FoodConfig { get; private set; }
        public ResourcesConfig GoldConfig { get; private set; }
        public ResourcesConfig HappyPointsConfig { get; private set; }
        public int WoodTotalCount { get; private set; }
        public int FoodTotalCount { get; private set; }
        public int StoneTotalCount { get; private set; }


        public UnionResourcesConfigParser(UnionConfig unionConfig, int forestPlaceArrayCount,
            int rockPlaceArrayCount)
        {
            ConfigParse(unionConfig, forestPlaceArrayCount, rockPlaceArrayCount);
        }

        private void ConfigParse(UnionConfig unionConfig, int forestPlaceArrayCount,
            int rockPlaceArrayCount)
        {
            for (int i = 0; i < unionConfig.AllResourcesConfigs.Length; i++)
            {
                var config = unionConfig.AllResourcesConfigs[i];
                switch (unionConfig.AllResourcesConfigs[i].Type)
                {
                    case ResourcesType.Wood:
                        WoodConfig = config;
                        WoodTotalCount = forestPlaceArrayCount * WoodConfig.SpawnCount;
                        break;
                    case ResourcesType.Stone:
                        StoneConfig = config;
                        StoneTotalCount = rockPlaceArrayCount * StoneConfig.SpawnCount;
                        break;
                    case ResourcesType.Food:
                        FoodConfig = config;
                        FoodTotalCount = forestPlaceArrayCount * FoodConfig.SpawnCount;
                        break;
                    case ResourcesType.Gold:
                        GoldConfig = config;
                        break;                    
                    case ResourcesType.Happiness:
                        HappyPointsConfig = config;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}
