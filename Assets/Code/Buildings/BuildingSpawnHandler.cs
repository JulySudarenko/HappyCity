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
        private readonly AudioSource _audioSource;
        private readonly AudioClip _audioClip;
        private ITimeRemaining _timeRemaining;
        private Transform _buildingTransform;
        private readonly Transform _folder;

        public BuildingSpawnHandler(BuildingConfig config, IQuestState questState, Transform buildingPlace,
            Transform folder, CameraController cameraController, AudioSource audioSource, AudioClip audioClip)
        {
            _buildingConfig = config;
            _questState = questState;
            _buildingPlace = buildingPlace;
            _folder = folder;
            _cameraController = cameraController;
            _audioSource = audioSource;
            _audioClip = audioClip;
            _questState.OnStateChange += StartConstruction;
        }
        
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
                _cameraController.ChangeTarget(2.0f);
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
            _audioSource.clip = _audioClip;
            _audioSource.Play();
            foreach (Transform child in _buildingTransform)
            {
                child.TryGetComponent<IHit>(out var hit);
                if (hit != null)
                {
                    BuildingIsDone?.Invoke(hit, child.position);
                }
            }
            _timeRemaining.RemoveTimeRemaining();
        }
    }
}
