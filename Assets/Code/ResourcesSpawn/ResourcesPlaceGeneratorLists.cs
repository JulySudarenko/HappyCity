﻿using System;
using System.Collections.Generic;
using System.Linq;
using Code.Configs;
using Code.Factory;
using Code.View;
using UnityEngine;


namespace Code.ResourcesSpawn
{
    internal class ResourcesPlaceGeneratorLists
    {
        private List<Vector3> _allWoodPlaces = new List<Vector3>();

        private List<Vector3> _allStonePlaces = new List<Vector3>();

        private List<Vector3> _allFoodPlaces = new List<Vector3>();

        public ResourcesPlaceGeneratorLists(SpawnPlacesView resourcesSpawnPlaces,
            UnionResourcesConfigParser unionResourcesConfigParser)
        {
            ConfigParse(resourcesSpawnPlaces, unionResourcesConfigParser);
        }

        public Vector3[] AllFoodPlaces => _allFoodPlaces.ToArray();
        public Vector3[] AllWoodPlaces => _allWoodPlaces.ToArray();
        public Vector3[] AllStonePlaces => _allStonePlaces.ToArray();

        // public void SetPlaces(ResourcesType type, Vector3[] reslist)
        // {
        //     switch (type)
        //     {
        //         case ResourcesType.Wood:
        //             _allWoodPlaces = reslist.ToList();
        //             break;
        //         case ResourcesType.Stone:
        //             _allStonePlaces = reslist.ToList();
        //             break;
        //         case ResourcesType.Food:
        //             _allFoodPlaces = reslist.ToList();
        //             break;
        //         default:
        //             throw new ArgumentOutOfRangeException(nameof(type), type, null);
        //     }
        // }
        
        private void ConfigParse(SpawnPlacesView resourcesSpawnPlaces, UnionResourcesConfigParser unionResourcesConfigParser)
        {
            GeneratePlaces(resourcesSpawnPlaces.ForestPlaces, unionResourcesConfigParser.WoodConfig, _allWoodPlaces);
            GeneratePlaces(resourcesSpawnPlaces.StonePlaces, unionResourcesConfigParser.StoneConfig, _allStonePlaces);
            GeneratePlaces(resourcesSpawnPlaces.ForestPlaces, unionResourcesConfigParser.FoodConfig, _allFoodPlaces);
        }

        private void GeneratePlaces(Transform[] resourcesSpawnPlaces, ResourcesConfig config, List<Vector3> placesList)
        {
            for (int i = 0; i < resourcesSpawnPlaces.Length; i++)
            {
                var resourcesPlaces =
                    new ResourcesSpawnPlacesGenerator(resourcesSpawnPlaces[i].position, config);
                for (int j = 0; j < resourcesPlaces.SpawnPlaces.Length; j++)
                {
                    placesList.Add(resourcesPlaces.SpawnPlaces[j]);
                }
            }
        }
    }
}