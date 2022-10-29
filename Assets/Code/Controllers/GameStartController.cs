using Code.Assistance;
using Code.Configs;
using Code.Factory;
using Code.Network;
using Code.ResourcesC;
using Code.ResourcesSpawn;
using Code.Timer;
using Code.UserInput;
using Code.View;
using Code.ViewHandlers;
using Photon.Pun;
using UnityEngine;


namespace Code.Controllers
{
    public class GameStartController : MonoBehaviour
    {
        [SerializeField] private LoadingIndicatorView _loadingIndicator;
        [SerializeField] private Canvas _canvas;

        [Header("Configs")] [SerializeField] private InputConfig _inputConfig;
        [SerializeField] private PlayerConfig _playerConfig;
        [SerializeField] private CameraConfig _cameraConfig;

        [Header("Quests")] [SerializeField] private QuestNpcConfig _houseQuest;
        [SerializeField] private QuestNpcConfig _farmQuest;

        [Header("Resources")] [SerializeField] private SpawnPlacesView _resourcesSpawnPlaces;
        [SerializeField] private UnionResourcesConfig _unionResourcesConfig;
        [SerializeField] private Transform _resourcesPanelView;
        [SerializeField] private ImageLineElement _resourceLineElement;

        [Header("Tasks")] [SerializeField] private Transform _tasksPanelView;
        [SerializeField] private LineElementView _tasksLineElement;
        [SerializeField] private LineElementView _messagePanelView;

        private Controllers _controllers;

        private void Awake()
        {
            _controllers = new Controllers();
            var camera = Camera.main;

            var unionResourcesParser = new UnionResourcesConfigParser(_unionResourcesConfig,
                _resourcesSpawnPlaces.ForestPlaces.Length, _resourcesSpawnPlaces.StonePlaces.Length);

            var photonConnectionController =
                gameObject.GetOrAddComponent<PhotonConnectionController>();
            photonConnectionController.Init(_loadingIndicator);

            var networkSynchronizer =
                gameObject.GetOrAddComponent<NetworkSynchronizationController>();
            networkSynchronizer.Init(_loadingIndicator, unionResourcesParser);

            var input = new InputInitialization(_inputConfig);
            var inputController = new InputController(input);

            var characterSpawner = new CharacterSpawnHandler(_playerConfig, networkSynchronizer);

            var playerController =
                new PlayerController(input, _playerConfig, characterSpawner.Character, camera);
            
            var cameraController = new CameraController(camera.transform, _cameraConfig,
                characterSpawner.Character.Transform);

            var playerView = new PlayerViewHandler(characterSpawner.Character, _playerConfig, _canvas, camera);
            
            var woodCounter = new ResourceCounterController(unionResourcesParser.WoodConfig, characterSpawner.Character,
                ResourcesType.Wood);
            var foodCounter = new ResourceCounterController(unionResourcesParser.FoodConfig, characterSpawner.Character,
                ResourcesType.Food);
            var stoneCounter = new ResourceCounterController(unionResourcesParser.StoneConfig,
                characterSpawner.Character, ResourcesType.Stone);
            var goldCounter = new ResourceCounterController(unionResourcesParser.GoldConfig, characterSpawner.Character,
                ResourcesType.Gold);

            var resourceUnionController = new ResourcesCheckUnionController(woodCounter, foodCounter,
                stoneCounter, goldCounter);

            var questSystemController = new QuestSystemController(_houseQuest, _farmQuest,
                characterSpawner.Character.ColliderID, resourceUnionController, _messagePanelView, _canvas, camera);

            if (PhotonNetwork.IsMasterClient)
            {
                _loadingIndicator.UpdateFeedbackText("MASTER");
                var placeGeneratorLists = new ResourcesPlaceGeneratorLists(_resourcesSpawnPlaces, unionResourcesParser);
                var dataSender = new StartingDataSender(placeGeneratorLists, photonConnectionController);
                _controllers.Add(dataSender);

                var woodSpawner = new ResourcesSpawner(placeGeneratorLists.AllWoodPlaces,
                    unionResourcesParser.WoodConfig, characterSpawner.Character.CharacterID,
                    networkSynchronizer);
                var stoneSpawner = new ResourcesSpawner(placeGeneratorLists.AllStonePlaces,
                    unionResourcesParser.StoneConfig, characterSpawner.Character.CharacterID,
                    networkSynchronizer);
                var foodSpawner = new ResourcesSpawner(placeGeneratorLists.AllFoodPlaces,
                    unionResourcesParser.FoodConfig, characterSpawner.Character.CharacterID,
                    networkSynchronizer);
                
                woodCounter.Init(woodSpawner);
                foodCounter.Init(foodSpawner);
                stoneCounter.Init(stoneSpawner);
                
                _controllers.Add(woodCounter);
                _controllers.Add(foodCounter);
                _controllers.Add(stoneCounter);
            }
            else
            {
                _loadingIndicator.UpdateFeedbackText($"NOT MASTER");
                var resourcesSpawner =
                    new ResourcesSpawnController(unionResourcesParser, characterSpawner, networkSynchronizer, woodCounter, foodCounter, stoneCounter);

                // woodCounter.Init(resourcesSpawner.WoodSpawner);
                // foodCounter.Init(resourcesSpawner.FoodSpawner);
                // stoneCounter.Init(resourcesSpawner.StoneSpawner);
                _controllers.Add(resourcesSpawner.WoodController);
                // _controllers.Add(foodCounter);
                // _controllers.Add(stoneCounter);
            }

            
            var viewController = new ViewController(_unionResourcesConfig, _resourcesPanelView, _resourceLineElement,
                _tasksPanelView, _tasksLineElement, woodCounter, foodCounter, stoneCounter, goldCounter,
                questSystemController);

            _controllers.Add(playerController);
            _controllers.Add(cameraController);
            _controllers.Add(playerView);

            _controllers.Add(questSystemController);

            // _controllers.Add(woodCounter);
            // _controllers.Add(foodCounter);
            // _controllers.Add(stoneCounter);
            _controllers.Add(goldCounter);

            _controllers.Add(new TimeRemainingController());
            _controllers.Add(inputController);

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
            _controllers.LateExecute(Time.deltaTime);
        }

        private void OnDestroy()
        {
            _controllers.Cleanup();
        }
    }
}
