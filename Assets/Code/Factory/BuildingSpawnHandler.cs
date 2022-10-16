using Code.Configs;
using Code.Controllers;
using Code.Interfaces;
using UnityEngine;

namespace Code.Factory
{
    internal class BuildingSpawnHandler : IInitialization, ICleanup
    {
        private IQuestState _questState;
        private readonly BuildingConfig _buildingConfig;

        public BuildingSpawnHandler(BuildingConfig config, IQuestState questState)
        {
            _buildingConfig = config;
            _questState = questState;
        }

        public void Build(QuestState state)
        {
            if (state == QuestState.Done)
            {
                var prefab = _buildingConfig.Prefab[Random.Range(0, _buildingConfig.Prefab.Length)];
                var place = _buildingConfig.Places[Random.Range(0, _buildingConfig.Prefab.Length)];
                var build = Object.Instantiate(prefab, place.position, place.rotation);
            }
        }

        public void Initialize()
        {
            _questState.OnStateChange += Build;
        }

        public void Cleanup()
        {
            _questState.OnStateChange -= Build;
        }
    }
}
