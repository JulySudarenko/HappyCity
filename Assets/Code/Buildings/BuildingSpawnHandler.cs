using System;
using Code.Configs;
using Code.Hit;
using Code.Quest;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Code.Buildings
{
    internal class BuildingSpawnHandler
    {
        public Action<IHit, Vector3> BuildingIsDone;
        private readonly IQuestState _questState;
        private readonly BuildingConfig _buildingConfig;
        private readonly Transform _buildingPlace;
        private Transform _buildingTransform;
        private readonly Transform _folder;

        public BuildingSpawnHandler(BuildingConfig config, IQuestState questState, Transform buildingPlace,
            Transform folder)
        {
            _buildingConfig = config;
            _questState = questState;
            _buildingPlace = buildingPlace;
            _folder = folder;
            
            _questState.OnStateChange += Build;
        }

        //public IHit BuildingEnter => _buildingTransform.GetComponentInChildren<IHit>();

        //public Vector3 BuildingEnterPosition => _buildingTransform.position;
        
        public Vector3 BuildingEnterPosition()
        {
            Vector3 position = Vector3.zero;
            foreach (Transform child in _buildingTransform)
            {
                child.TryGetComponent<IHit>(out var hit);
                if (hit != null)
                    position = child.position;
            }
        
            return position;
        }

        private void Build(QuestState state)
        {
            if (state == QuestState.Done)
            {
                var prefab = _buildingConfig.Prefab[Random.Range(0, _buildingConfig.Prefab.Length)];
                _buildingTransform = Object.Instantiate(prefab, _buildingPlace.position, _buildingPlace.rotation);
                _buildingTransform.SetParent(_folder);

                foreach (Transform child in _buildingTransform)
                {
                    //Debug.Log(child.gameObject.name);
                    child.TryGetComponent<IHit>(out var hit);
                    if (hit != null)
                    {
                        // Debug.Log(hit);
                        // Debug.Log(child.position);
                    }
                }

                //BuildingIsDone?.Invoke();
                _questState.OnStateChange -= Build;
            }
        }
    }
}
