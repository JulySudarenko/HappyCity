﻿using System;
using Code.Configs;
using Code.Factory;


namespace Code.ResourcesSpawn
{
    public class UnionResourcesConfigParser
    {
        public ResourcesConfig WoodConfig { get; private set; }
        public ResourcesConfig StoneConfig { get; private set; }
        public ResourcesConfig FoodConfig { get; private set; }
        public ResourcesConfig GoldConfig { get; private set; }
        public int WoodTotalCount { get; private set; }
        public int FoodTotalCount { get; private set; }
        public int StoneTotalCount { get; private set; }


        public UnionResourcesConfigParser(UnionResourcesConfig unionResourcesConfig, int forestPlaceArrayCount,
            int rockPlaceArrayCount)
        {
            ConfigParse(unionResourcesConfig, forestPlaceArrayCount, rockPlaceArrayCount);
        }

        private void ConfigParse(UnionResourcesConfig unionResourcesConfig, int forestPlaceArrayCount,
            int rockPlaceArrayCount)
        {
            for (int i = 0; i < unionResourcesConfig.AllResourcesConfigs.Length; i++)
            {
                var config = unionResourcesConfig.AllResourcesConfigs[i];
                switch (unionResourcesConfig.AllResourcesConfigs[i].Type)
                {
                    case ResourcesType.Wood:
                        WoodConfig = config;
                        WoodTotalCount = forestPlaceArrayCount * WoodConfig.Count;
                        break;
                    case ResourcesType.Stone:
                        StoneConfig = config;
                        StoneTotalCount = rockPlaceArrayCount * StoneConfig.Count;
                        break;
                    case ResourcesType.Food:
                        FoodConfig = config;
                        FoodTotalCount = forestPlaceArrayCount * FoodConfig.Count;
                        break;
                    case ResourcesType.Gold:
                        GoldConfig = config;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}