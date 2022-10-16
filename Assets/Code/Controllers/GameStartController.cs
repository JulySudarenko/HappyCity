using Code.Assistance;
using Code.Configs;
using Code.Factory;
using Code.ResourcesSpawn;
using Code.Timer;
using Code.UserInput;
using Code.View;
using Photon.Pun;
using UnityEngine;


namespace Code.Controllers
{
    public class GameStartController : MonoBehaviour
    {
        [SerializeField] private LoadingIndicatorView _loadingIndicator;

        [Header("Configs")] [SerializeField] private InputConfig _inputConfig;
        [SerializeField] private PlayerConfig _playerConfig;
        [SerializeField] private CameraConfig _cameraConfig;
        [SerializeField] private NonPlayerCharacterConfig _npcConfig;

        [Header("Buildings")] [SerializeField] private BuildingConfig _houseConfig;
        [SerializeField] private BuildingConfig _farmConfig;

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

            var woodCounter = new ResourceCounterController(unionResourcesParser.WoodConfig, characterSpawner.Character,
                ResourcesType.Wood);
            var foodCounter = new ResourceCounterController(unionResourcesParser.FoodConfig, characterSpawner.Character,
                ResourcesType.Food);
            var stoneCounter = new ResourceCounterController(unionResourcesParser.StoneConfig,
                characterSpawner.Character, ResourcesType.Stone);
            var goldCounter = new ResourceCounterController(unionResourcesParser.GoldConfig, characterSpawner.Character,
                ResourcesType.Gold);

            var resourceChecker = new QuestResourcesChecker(woodCounter, foodCounter, stoneCounter, goldCounter);

            var npc = new NpcSpawnHandler(_npcConfig);
            var quest = new QuestController(npc, characterSpawner.Character.ColliderID, _messagePanelView,
                _houseConfig, resourceChecker);
            var houseSpawner = new BuildingSpawnHandler(_houseConfig, quest);
            var npcController = new NpcController(_npcConfig, npc);

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
            }
            else
            {
                _loadingIndicator.UpdateFeedbackText($"NOT MASTER");
                var resourcesSpawner =
                    new ResourcesSpawnController(unionResourcesParser, characterSpawner, networkSynchronizer);

                woodCounter.Init(resourcesSpawner.WoodSpawner);
                foodCounter.Init(resourcesSpawner.FoodSpawner);
                stoneCounter.Init(resourcesSpawner.StoneSpawner);
            }

            var viewController = new ViewController(_unionResourcesConfig, _resourcesPanelView, _resourceLineElement,
                _tasksPanelView, _tasksLineElement, woodCounter, foodCounter, stoneCounter, quest);

            _controllers.Add(playerController);
            _controllers.Add(cameraController);
            
            _controllers.Add(npcController);
            _controllers.Add(quest);
            _controllers.Add(houseSpawner);

            _controllers.Add(woodCounter);
            _controllers.Add(foodCounter);
            _controllers.Add(stoneCounter);

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
