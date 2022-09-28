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

        [Header("Resources")] [SerializeField] private SpawnPlacesView _resourcesSpawnPlaces;
        [SerializeField] private UnionResourcesConfig _unionResourcesConfig;
        [SerializeField] private Transform _resourcesPanelView;
        [SerializeField] private ImageLineElement _resourceLineElement;

        [Header("Tasks")] [SerializeField] private Transform _tasksPanelView;
        [SerializeField] private LineElementView _tasksLineElement;

        private Controllers _controllers;
        private ResourcesPlaceGeneratorLists _placeGeneratorLists;
        private NetworkSynchronizationController _networkSynchronizer;
        private UnionResourcesConfigParser _unionResourcesParser;
        private CharacterSpawnHandler _characterSpawner;

        private void Awake()
        {
            _unionResourcesParser = new UnionResourcesConfigParser(_unionResourcesConfig,
                _resourcesSpawnPlaces.ForestPlaces.Length, _resourcesSpawnPlaces.StonePlaces.Length);

            PhotonConnectionController photonConnectionController =
                gameObject.GetOrAddComponent<PhotonConnectionController>();
            photonConnectionController.Init(_loadingIndicator);

            _networkSynchronizer =
                gameObject.GetOrAddComponent<NetworkSynchronizationController>();
            _networkSynchronizer.Init(_loadingIndicator, _unionResourcesParser);

            var input = new InputInitialization(_inputConfig);
            var inputController = new InputController(input);

            _characterSpawner = new CharacterSpawnHandler(_playerConfig, input, _cameraConfig, _networkSynchronizer);

            _controllers = new Controllers();


            if (PhotonNetwork.IsMasterClient)
            {
                _loadingIndicator.UpdateFeedbackText("MASTER");
                _placeGeneratorLists = new ResourcesPlaceGeneratorLists(_resourcesSpawnPlaces, _unionResourcesParser);
                var dataSender = new StartingDataSender(_placeGeneratorLists, photonConnectionController);
                _controllers.Add(dataSender);

                // for (int i = 0; i < _placeGeneratorLists.AllFoodPlaces.Length; i++)
                // {
                //     PhotonNetwork.RaiseEvent(111, _placeGeneratorLists.AllFoodPlaces[i],
                //         new RaiseEventOptions() {Receivers = ReceiverGroup.All},
                //         new SendOptions() {Reliability = true});
                // }
                //
                // for (int i = 0; i < _placeGeneratorLists.AllWoodPlaces.Length; i++)
                // {
                //     PhotonNetwork.RaiseEvent(112,
                //         _placeGeneratorLists.AllWoodPlaces[i],
                //         new RaiseEventOptions() {Receivers = ReceiverGroup.All},
                //         new SendOptions() {Reliability = true});
                // }
                //
                // for (int i = 0; i < _placeGeneratorLists.AllStonePlaces.Length; i++)
                // {
                //     PhotonNetwork.RaiseEvent(113,
                //         _placeGeneratorLists.AllStonePlaces[i],
                //         new RaiseEventOptions() {Receivers = ReceiverGroup.All},
                //         new SendOptions() {Reliability = true});
                // }
                //
                var woodSpawner = new ResourcesSpawner(_placeGeneratorLists.AllWoodPlaces,
                    _unionResourcesParser.WoodConfig, _characterSpawner.Character.CharacterModel.CharacterID);
                var stoneSpawner = new ResourcesSpawner(_placeGeneratorLists.AllStonePlaces,
                    _unionResourcesParser.StoneConfig, _characterSpawner.Character.CharacterModel.CharacterID);
                var foodSpawner = new ResourcesSpawner(_placeGeneratorLists.AllFoodPlaces,
                    _unionResourcesParser.FoodConfig, _characterSpawner.Character.CharacterModel.CharacterID);

                // _controllers.Add(woodSpawner);
                // _controllers.Add(stoneSpawner);
                // _controllers.Add(foodSpawner);
            }
            else
            {
                _loadingIndicator.UpdateFeedbackText($"NOT MASTER");
                var resourcesSpawner = new ResourcesSpawnController(_placeGeneratorLists, _unionResourcesParser,
                    _characterSpawner, _loadingIndicator, _networkSynchronizer);

                // _controllers.Add(resourcesSpawner.WoodSpawner);
                // _controllers.Add(resourcesSpawner.FoodSpawner);
                // _controllers.Add(resourcesSpawner.StoneSpawner);
            }
            //
            // var resourcesSpawner = new ResourcesSpawnController(photonConnectionController, _placeGeneratorLists,
            //     _unionResourcesParser, _characterSpawner, _loadingIndicator, _networkSynchronizer);
            //     
            // _controllers.Add(resourcesSpawner.WoodSpawner);
            // _controllers.Add(resourcesSpawner.FoodSpawner);
            // _controllers.Add(resourcesSpawner.StoneSpawner);

            var viewController = new ViewController(_unionResourcesConfig, _resourcesPanelView, _resourceLineElement,
                _tasksPanelView, _tasksLineElement);

            _controllers.Add(new TimeRemainingController());
            _controllers.Add(inputController);
            _controllers.Add(_characterSpawner.Character);

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

    internal class ResourcesSpawnController
    {
        private ResourcesSpawner _woodSpawner;
        private ResourcesSpawner _foodSpawner;
        private ResourcesSpawner _stoneSpawner;
        private readonly ResourcesPlaceGeneratorLists _placeGeneratorLists;
        private readonly NetworkSynchronizationController _networkSynchronizer;
        private readonly UnionResourcesConfigParser _unionResourcesParser;
        private readonly CharacterSpawnHandler _characterSpawner;
        private readonly LoadingIndicatorView _loadingIndicator;
        public ResourcesSpawner WoodSpawner => _woodSpawner;
        public ResourcesSpawner FoodSpawner => _foodSpawner;
        public ResourcesSpawner StoneSpawner => _stoneSpawner;

        private int flagCounter = 0;
        private int flagmaxcount = 3;

        public ResourcesSpawnController(ResourcesPlaceGeneratorLists placeGeneratorLists,
            UnionResourcesConfigParser unionResourcesParser,
            CharacterSpawnHandler characterSpawner, LoadingIndicatorView loadingIndicator,
            NetworkSynchronizationController networkSynchronizer)
        {
            _placeGeneratorLists = placeGeneratorLists;
            _unionResourcesParser = unionResourcesParser;
            _characterSpawner = characterSpawner;
            _loadingIndicator = loadingIndicator;
            _networkSynchronizer = networkSynchronizer;
            _networkSynchronizer.AllPointsReceived += CreateResources;
        }

        private void CreateResources(int code)
        {
            switch (code)
            {
                // поставить не счетчик, а ограничитель. Чтобы контроллировать по отдельности.
                case 111:
                    //_placeGeneratorLists.SetPlaces(ResourcesType.Food, _networkSynchronizer.AllFoodPlaces.ToArray());
                    flagCounter++;
                    break;
                case 112:
                    _woodSpawner = new ResourcesSpawner(_networkSynchronizer.AllWoodPlaces.ToArray(),
                        _unionResourcesParser.WoodConfig, _characterSpawner.Character.CharacterModel.CharacterID);
                    flagCounter++;
                    break;
                case 113:
                    //_placeGeneratorLists.SetPlaces(ResourcesType.Stone, _networkSynchronizer.AllStonePlaces.ToArray());
                    flagCounter++;
                    break;
                default:
                    break;
            }

            if (flagCounter == flagmaxcount)
            {
                _networkSynchronizer.AllPointsReceived -= CreateResources;
            }
        }
    }
}
