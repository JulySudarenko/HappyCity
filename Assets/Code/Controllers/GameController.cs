using System;
using Code.Configs;
using Code.Factory;
using Code.UserInput;
using Code.View;
using UnityEngine;

namespace Code.Controllers
{
    public class GameController : MonoBehaviour
    {
        [Header("Configs")] 
        [SerializeField] private InputConfig _inputConfig;
        [SerializeField] private PlayerActionsNameConfig _playerActionsNameConfig;
        [SerializeField] private PlayerConfig _playerConfig;
        [SerializeField] private CameraConfig _cameraConfig;
        [SerializeField] private ResourcesConfig _resourcesConfig;

        [Header("View elements")] 
        [SerializeField] private SpawnPlacesView _resourcesSpawnPlaces;
        [SerializeField] private Transform _characterSelectedPanel;
        [SerializeField] private LineElementView _lineElement;

        private Controllers _controllers;

        private void Awake()
        {
            var input = new InputInitialization(_inputConfig);
            var inputController = new InputController(input);

            var characterSpawner = new CharacterSpawnHandler(_playerConfig, input);
            
            var resourcesSpawner = new ResourcesSpawnHandler(_resourcesSpawnPlaces, _resourcesConfig);
            var viewController = new ViewController(_characterSelectedPanel, _lineElement, characterSpawner);
            
            _controllers = new Controllers();
            _controllers.Add(inputController);
            _controllers.Add(characterSpawner);
            _controllers.Add(viewController);
        }

        private void Start()
        {
            _controllers.Initialize();
        }

        private void Update()
        {
            _controllers.Execute(Time.deltaTime);
        }

        private void FixedUpdate()
        {
            _controllers.FixedExecute(Time.fixedDeltaTime);
        }

        private void LateUpdate()
        {
            
        }

        private void OnDestroy()
        {
            _controllers.Cleanup();
        }
    }
}
