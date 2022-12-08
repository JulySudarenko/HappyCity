using System;
using System.Collections.Generic;
using System.Linq;
using Code.Buildings;
using Code.Configs;
using Code.Interfaces;
using Code.Network;
using Code.NPC;
using Code.Quest;
using Code.ResourcesC;
using Code.ResourcesSpawn;
using Code.View;
using Code.ViewHandlers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.Controllers
{
    internal class QuestSystemController : IFixedExecute, IExecute, ILateExecute, ICleanup
    {
        public Action<IQuestState> QuestAdd;

        private readonly Controllers _controllers = new Controllers();
        private readonly ResourcesCheckUnionController _resourceCheckUnionController;
        private readonly NetworkSynchronizationController _networkSynchronization;
        private readonly HappyLineController _happyLineController = new HappyLineController();
        private readonly List<QuestNpcConfig> _questConfigs;
        private readonly List<QuestController> _questList = new List<QuestController>();
        private readonly BuildingPlacesConfig _placesConfigs;
        private readonly List<Vector3> _buildingPlaces = new List<Vector3>();
        private readonly List<Vector3> _startPointsList = new List<Vector3>();
        private readonly LineElementView _messagePanelView;
        private readonly Canvas _canvas;
        private readonly Camera _camera;
        private readonly CameraController _cameraController;
        private readonly AudioSource _audioSource;
        private readonly MusicConfig _musicConfig;
        private readonly Transform _player;
        private readonly Transform _characterFolder;
        private readonly Transform _buildingFolder;
        private readonly string _nickName;
        private readonly int _characterID;

        private Vector3[] _questFirstQueue;
        private QuestState[] _questStateList;
        private int _questCounter = 0;
        private bool _isGamePlay;

        public QuestSystemController(UnionConfig configs, int characterID, string nickName,
            ResourcesCheckUnionController resourceCheckUnionController, LineElementView messagePanelView, Canvas canvas,
            Camera camera, Transform player, NetworkSynchronizationController networkSynchronization,
            CameraController cameraController, AudioSource audioSource, MusicConfig musicConfig)
        {
            _characterFolder = new GameObject("Characters").transform;
            _buildingFolder = new GameObject("Buildings").transform;
            _questConfigs = configs.AllQuestNpcConfigs.ToList();
            _placesConfigs = configs.AllBuildingPlacesConfigs[0];

            foreach (Transform childPoint in configs.AllQuestNpcConfigs[0].NpcConfig.SpawnPoints)
            {
                _startPointsList.Add(childPoint.position);
            }

            _resourceCheckUnionController = resourceCheckUnionController;
            _messagePanelView = messagePanelView;
            _canvas = canvas;
            _camera = camera;
            _player = player;
            _networkSynchronization = networkSynchronization;
            _cameraController = cameraController;
            _audioSource = audioSource;
            _musicConfig = musicConfig;
            _characterID = characterID;
            _nickName = nickName;
            _buildingPlaces.Add(new Vector3(0.0f, 0.0f, -85.0f));
            _buildingPlaces.Add(new Vector3(20.0f, 0.0f, -85.0f));
            _buildingPlaces.Add(new Vector3(40.0f, 0.0f, -85.0f));

            _controllers.Add(_happyLineController);
            _happyLineController.StartNewQuest += StartQuest;
            _networkSynchronization.AllPointsReceived += GetQuestQueue;
            _networkSynchronization.ChangeParameter += OnQuestChanges;
        }

        private void GetQuestQueue(int code)
        {
            if (code == 121)
            {
                _questStateList = new QuestState[_networkSynchronization.QuestFirstQueue.Count];
                _questFirstQueue = new Vector3[_networkSynchronization.QuestFirstQueue.Count];
                for (int i = 0; i < _networkSynchronization.QuestFirstQueue.Count; i++)
                {
                    _questFirstQueue[(int) _networkSynchronization.QuestFirstQueue[i].x] =
                        _networkSynchronization.QuestFirstQueue[i];
                    _questStateList[i] = QuestState.None;
                }

                _isGamePlay = true;
                StartQuest(_questFirstQueue.Length);
            }
        }

        private void OnQuestChanges(Vector3 questParameters, int code)
        {
            if (code == 122)
            {
                for (int i = 0; i < _questList.Count; i++)
                {
                    if (_questList[i].QuestNumber.x == questParameters.x)
                    {
                        _questList[i].OnSomebodyChangeQuest(QuestState.Busy);
                    }
                }
            }

            if (code == 123)
            {
                for (int i = 0; i < _questList.Count; i++)
                {
                    if (_questList[i].QuestNumber.x == questParameters.x)
                    {
                        _questList[i].OnSomebodyChangeQuest(QuestState.Done);
                    }
                }
            }
        }

        private void StartQuest(int count)
        {
            if (_isGamePlay)
            {
                if (_startPointsList.Count > 0)
                {
                    if (_questCounter < _questFirstQueue.Length)
                    {
                        var quest = _questConfigs[(int) _questFirstQueue[_questCounter].y];
                        var place = _placesConfigs.Places[(int) _questFirstQueue[_questCounter].z];
                        InitQuest(quest, place, _questFirstQueue[_questCounter]);
                        _questCounter++;
                    }
                    else
                    {
                        _isGamePlay = false;
                        _happyLineController.StartNewQuest -= StartQuest;
                    }
                }
            }
        }

        private void InitQuest(QuestNpcConfig questConfig, Transform buildingPlace, Vector3 number)
        {
            var num = Random.Range(0, _startPointsList.Count);
            var startPosition = _startPointsList[num];

            var npc = new NpcSpawnHandler(questConfig.NpcConfig, startPosition);
            npc.NpcTransform.SetParent(_characterFolder);
            var npcHappiness = new ResourcesKeeper(questConfig.StartHappiness, ResourcesType.Happiness);
            _happyLineController.ChangeCurrentPopulation(questConfig.StartHappiness);
            var quest = new QuestController(npc, _characterID, _nickName, _resourceCheckUnionController,
                questConfig, _messagePanelView, _canvas, npcHappiness, _happyLineController, number);

            _questList.Add(quest);

            var buildingSpawnHandler =
                new BuildingSpawnHandler(questConfig.BuildingConfig, quest, buildingPlace, _buildingFolder,
                    _cameraController, _audioSource, _musicConfig.BuldSound);

            var npcController = new NpcController(questConfig.NpcConfig, npc, _player, _characterID, quest,
                buildingSpawnHandler, startPosition, _startPointsList);

            var npcView = new NpcViewHandler(npc, questConfig.NpcConfig, _canvas, quest, _camera, npcHappiness);

            _controllers.Add(quest);
            _controllers.Add(npcController);
            _controllers.Add(npcView);
            QuestAdd?.Invoke(quest);
        }

        public void FixedExecute(float deltaTime)
        {
            _controllers.FixedExecute(deltaTime);
        }

        public void Execute(float deltaTime)
        {
            _controllers.Execute(deltaTime);
        }

        public void LateExecute(float deltaTime)
        {
            _controllers.LateExecute(deltaTime);
        }

        public void Cleanup()
        {
            _controllers.Cleanup();
            if (_happyLineController != null) _happyLineController.StartNewQuest -= StartQuest;
            _networkSynchronization.AllPointsReceived -= GetQuestQueue;
        }
    }
}
