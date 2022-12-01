﻿using Code.Factory;
using Code.Network;
using Code.ResourcesSpawn;

namespace Code.ResourcesC
{
    internal class ResourcesSpawnController
    {
        private ResourcesSpawner _woodSpawner;
        private ResourcesSpawner _foodSpawner;
        private ResourcesSpawner _stoneSpawner;
        private readonly NetworkSynchronizationController _networkSynchronizer;
        private readonly UnionResourcesConfigParser _unionResourcesParser;
        private readonly CharacterSpawnHandler _characterSpawner;
        public ResourceCounterController WoodController { get; }
        public ResourceCounterController FoodController { get; }
        public ResourceCounterController StoneController { get; }

        private bool _isWood;
        private bool _isFood;
        private bool _isStone;

        public ResourcesSpawnController(UnionResourcesConfigParser unionResourcesParser,
            CharacterSpawnHandler characterSpawner, NetworkSynchronizationController networkSynchronizer,
            ResourceCounterController woodController, ResourceCounterController foodController,
            ResourceCounterController stoneController)
        {
            _unionResourcesParser = unionResourcesParser;
            _characterSpawner = characterSpawner;
            _networkSynchronizer = networkSynchronizer;
            WoodController = woodController;
            FoodController = foodController;
            StoneController = stoneController;
            _networkSynchronizer.AllPointsReceived += CreateResources;
        }

        private void CreateResources(int code)
        {
            switch (code)
            {
                case 111:
                    if (!_isFood)
                    {
                        _foodSpawner = new ResourcesSpawner(_networkSynchronizer.AllFoodPlaces.ToArray(),
                            _unionResourcesParser.FoodConfig, _characterSpawner.Character.CharacterID,
                            _networkSynchronizer);
                        _isFood = true;
                        FoodController.Init(_foodSpawner);
                    }

                    break;
                case 112:
                    if (!_isWood)
                    {
                        _woodSpawner = new ResourcesSpawner(_networkSynchronizer.AllWoodPlaces.ToArray(),
                            _unionResourcesParser.WoodConfig, _characterSpawner.Character.CharacterID,
                            _networkSynchronizer);
                        _isWood = true;
                        WoodController.Init(_woodSpawner);
                    }

                    break;
                case 113:
                    if (!_isStone)
                    {
                        _stoneSpawner = new ResourcesSpawner(_networkSynchronizer.AllStonePlaces.ToArray(),
                            _unionResourcesParser.StoneConfig, _characterSpawner.Character.CharacterID,
                            _networkSynchronizer);
                        _isStone = true;
                        StoneController.Init(_stoneSpawner);
                    }

                    break;
                default:
                    break;
            }

            if (_isFood && _isWood && _isStone)
            {
                _networkSynchronizer.AllPointsReceived -= CreateResources;
            }
        }
    }
}
