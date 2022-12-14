using System.Collections.Generic;
using Code.Configs;
using Code.Network;
using Code.View;
using UnityEngine;


namespace Code.ResourcesSpawn
{
    internal class ResourcesPlaceGeneratorLists
    {
        private readonly List<Vector3> _allWoodPlaces = new List<Vector3>();

        private readonly List<Vector3> _allStonePlaces = new List<Vector3>();

        private readonly List<Vector3> _allFoodPlaces = new List<Vector3>();

        public ResourcesPlaceGeneratorLists(SpawnPlacesView resourcesSpawnPlaces,
            UnionResourcesConfigParser unionResourcesConfigParser)
        {
            ConfigParse(resourcesSpawnPlaces, unionResourcesConfigParser);
        }

        public ResourcesPlaceGeneratorLists(NetworkSynchronizationController networkSynchronizationController)
        {
            _allWoodPlaces = networkSynchronizationController.AllWoodPlaces;
            _allStonePlaces = networkSynchronizationController.AllStonePlaces;
            _allFoodPlaces = networkSynchronizationController.AllFoodPlaces;
        }

        public Vector3[] AllFoodPlaces => _allFoodPlaces.ToArray();
        public Vector3[] AllWoodPlaces => _allWoodPlaces.ToArray();
        public Vector3[] AllStonePlaces => _allStonePlaces.ToArray();

        private void ConfigParse(SpawnPlacesView resourcesSpawnPlaces,
            UnionResourcesConfigParser unionResourcesConfigParser)
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
