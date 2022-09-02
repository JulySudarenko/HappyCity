using Code.Configs;
using Code.Factory;
using Code.Interfaces;
using Code.UserInput;
using Code.View;
using UnityEngine;

namespace Code.Controllers
{
    public class GameStartController : MonoBehaviour
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

            var viewController = new ViewController(_characterSelectedPanel, _lineElement, characterSpawner);
            
            var resourcesPlaceGenerator = new ResourcesSpawnPlacesGenerator(_resourcesSpawnPlaces.ForestPlaces[0].position, _resourcesConfig); 
            
            var gameController = new GameProcessController(characterSpawner, _resourcesConfig, resourcesPlaceGenerator);


            _controllers = new Controllers();
            _controllers.Add(inputController);
            _controllers.Add(characterSpawner);
            _controllers.Add(viewController);
            _controllers.Add(gameController);
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
    
    internal class GameProcessController : IExecute, ICleanup
    {
        private readonly Controllers _controllers;
        private readonly CharacterSpawnHandler _characterSpawner;
        private readonly ResourcesConfig _resourcesConfig;
        private readonly ResourcesSpawnPlacesGenerator _placesGenerator;
        
        public GameProcessController(CharacterSpawnHandler characterSpawner, ResourcesConfig resourcesConfig, ResourcesSpawnPlacesGenerator placesGenerator)
        {
            _characterSpawner = characterSpawner;
            _resourcesConfig = resourcesConfig;
            _placesGenerator = placesGenerator;
            _controllers = new Controllers();
            characterSpawner.IsCharacterCreated += Init;
        }
        
        private void Init()
        {
            var character = _characterSpawner.Character;
            var resourcesSpawner = new ResourcesSpawnHandler(_placesGenerator, _resourcesConfig, character.ColliderID);
            _controllers.Add(resourcesSpawner);
            _controllers.Initialize();
        }
        
        public void Execute(float deltaTime)
        {
            _controllers.Execute(Time.deltaTime);
        }

        public void Cleanup()
        {
            //characterSpawner.IsCharacterCreated -= Init;
            _controllers.Cleanup();
        }
    }
}
