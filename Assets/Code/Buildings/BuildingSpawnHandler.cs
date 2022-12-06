using System;
using Code.Configs;
using Code.Controllers;
using Code.Hit;
using Code.Quest;
using Code.Timer;
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
        private readonly CameraController _cameraController;
        private ITimeRemaining _timeRemaining;
        private Transform _buildingTransform;
        private readonly Transform _folder;

        public BuildingSpawnHandler(BuildingConfig config, IQuestState questState, Transform buildingPlace,
            Transform folder, CameraController cameraController)
        {
            _buildingConfig = config;
            _questState = questState;
            _buildingPlace = buildingPlace;
            _folder = folder;
            _cameraController = cameraController;

            _questState.OnStateChange += StartConstruction;
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

        private void StartConstruction(QuestState state)
        {
            if (state == QuestState.Done)
            {
                _cameraController.ChangeTarget(_buildingPlace);
                
                _timeRemaining = new TimeRemaining(Build, 1.0f);
                _timeRemaining.AddTimeRemaining();
                _questState.OnStateChange -= StartConstruction;
            }
        }

        private void Build()
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
            _timeRemaining.RemoveTimeRemaining();
            //BuildingIsDone?.Invoke();
        }
    }
}
