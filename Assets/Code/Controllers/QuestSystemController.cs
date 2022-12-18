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
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.Controllers
{
    internal class QuestSystemController : IFixedExecute, IExecute, ILateExecute, ICleanup
    {
        public Action<IQuestState> QuestAdd;

        private readonly List<QuestNpcConfig> _questConfigs;
        private readonly List<QuestController> _questList = new List<QuestController>();
        private readonly List<Vector3> _startPointsList = new List<Vector3>();
        private readonly List<Vector3> _questsInWork = new List<Vector3>();

        private readonly Controllers _controllers = new Controllers();
        private readonly ResourcesCheckUnionController _resourceCheckUnionController;
        private readonly NetworkSynchronizationController _networkSynchronization;
        private readonly PhotonConnectionController _photonConnectionController;
        private readonly HappyLineController _happyLineController = new HappyLineController();
        private readonly BuildingPlacesConfig _placesConfigs;

        private readonly DialogPanelView _dialogPanelView;
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
        private NetQuestsInfo[] _allQuestFirstQueue;
        private int _questCounter = 0;
        //private bool _hasQuestQueue;


        public QuestSystemController(UnionConfig configs, int characterID, string nickName,
            ResourcesCheckUnionController resourceCheckUnionController, Canvas canvas,
            Camera camera, Transform player, NetworkSynchronizationController networkSynchronization,
            CameraController cameraController, AudioSource audioSource, MusicConfig musicConfig,
            DialogPanelView dialogPanelView, PhotonConnectionController photonConnectionController)
        {
            _characterFolder = new GameObject("Characters").transform;
            _buildingFolder = new GameObject("Buildings").transform;
            _questConfigs = configs.AllQuestNpcConfigs.ToList();
            _placesConfigs = configs.AllBuildingPlacesConfigs[0];
            _dialogPanelView = dialogPanelView;
            _photonConnectionController = photonConnectionController;
            foreach (Transform childPoint in configs.AllQuestNpcConfigs[0].NpcConfig.SpawnPoints)
            {
                _startPointsList.Add(childPoint.position);
            }

            _resourceCheckUnionController = resourceCheckUnionController;
            _canvas = canvas;
            _camera = camera;
            _player = player;
            _networkSynchronization = networkSynchronization;
            _cameraController = cameraController;
            _audioSource = audioSource;
            _musicConfig = musicConfig;
            _characterID = characterID;
            _nickName = nickName;

            _controllers.Add(_happyLineController);
            _happyLineController.StartNewQuest += StartQuest;
            _networkSynchronization.AllPointsReceived += GetQuestQueue;
            _networkSynchronization.ChangeParameter += OnQuestChanges;
            _photonConnectionController.LeaveGame += FreeQuests;
        }

        private void GetQuestQueue(int code)
        {
            // if (code == 121)
            // {
            //     //_questStateList = new QuestState[_networkSynchronization.QuestFirstQueue.Count];
            //     _questFirstQueue = new Vector3[_networkSynchronization.QuestFirstQueue.Count];
            //     for (int i = 0; i < _networkSynchronization.QuestFirstQueue.Count; i++)
            //     {
            //         _questFirstQueue[(int) _networkSynchronization.QuestFirstQueue[i].x] =
            //             _networkSynchronization.QuestFirstQueue[i];
            //         //_questStateList[i] = QuestState.None;
            //     }
            //
            //     _hasQuestQueue = true;
            //     StartQuest(_questFirstQueue.Length);
            // }

            if (code == 125)
            {
                SynchronizeAllQuests();
                StartQuest(_questCounter);
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

            if (code == 124)
            {
                for (int i = 0; i < _questList.Count; i++)
                {
                    if (_questList[i].QuestNumber.x == questParameters.x)
                    {
                        _questList[i].OnSomebodyChangeQuest(QuestState.Start);
                    }
                }
            }
        }

        private void StartQuest(int count)
        {
            // if (_hasQuestQueue)
            // {
            if (_startPointsList.Count > 0)
            {
                if (_questCounter < _allQuestFirstQueue.Length)
                {
                    var quest = _questConfigs[(int) _allQuestFirstQueue[_questCounter].Quest.y];
                    var place = _placesConfigs.Places[(int) _allQuestFirstQueue[_questCounter].Quest.z];
                    InitQuest(quest, place, _allQuestFirstQueue[_questCounter].Quest);
                    _questCounter++;
                }
                else
                {
                    //_hasQuestQueue = false;
                    _happyLineController.StartNewQuest -= StartQuest;
                }

                //}
            }
        }

        // private void SendBusyQuest()
        // {
        //     for (int i = 0; i < _questsInWork.Count; i++)
        //     {
        //         PhotonNetwork.RaiseEvent(127, _questsInWork[i],
        //             new RaiseEventOptions() {Receivers = ReceiverGroup.Others},
        //             new SendOptions() {Reliability = true});
        //     }
        // }

        private void FreeQuests()
        {
            for (int i = 0; i < _questsInWork.Count; i++)
            {
                PhotonNetwork.RaiseEvent(124, _questsInWork[i],
                    new RaiseEventOptions() {Receivers = ReceiverGroup.Others},
                    new SendOptions() {Reliability = true});
            }
        }

        private int FinedMaxNumber(NetQuestsInfo[] allQuests)
        {
            var maxCount = 0;
            for (int i = 0; i < allQuests.Length; i++)
            {
                if (allQuests[i].State != QuestState.None)
                {
                    if (allQuests[i].Quest.x > maxCount)
                    {
                        maxCount = (int) allQuests[i].Quest.x;
                    }
                }
            }

            return maxCount;
        }

        private void ExecutePrevious(NetQuestsInfo[] allQuests)
        {
            Debug.Log("LOOK PREVIOUS");
            
            for (int i = 0; i < allQuests.Length; i++)
            {
                if (allQuests[i].State == QuestState.Done)
                {
                    var quest = _questConfigs[(int) allQuests[i].Quest.y];
                    var place = _placesConfigs.Places[(int) allQuests[i].Quest.z];
                    Debug.Log($"Instantiate {allQuests[i].Quest.x}, {place.position}");
                    var buildingSpawnHandler =
                        new BuildingSpawnHandler(quest.BuildingConfig, place, _buildingFolder);
                    _happyLineController.ChangeCurrentPopulation(quest.StartHappiness);
                    _happyLineController.ChangeHappiness(quest.StartHappiness, quest.BonusHappiness);
                }

                if (i < _questCounter)
                {
                    if (allQuests[i].State != QuestState.None && allQuests[i].State != QuestState.Done)
                    {
                        var quest = _questConfigs[(int) allQuests[i].Quest.y];
                        var place = _placesConfigs.Places[(int) allQuests[i].Quest.z];
                        InitQuest(quest, place, allQuests[i].Quest);
                        Debug.Log($"Quest init {allQuests[i].Quest.x}, {place}");
                    }

                    if (allQuests[i].State == QuestState.Busy)
                    {
                        OnQuestChanges(allQuests[i].Quest, 122);
                    }
                }
            }
        }

        private void SynchronizeAllQuests()
        {
            _allQuestFirstQueue = _networkSynchronization.AllQuestsInfos.ToArray();
            for (int i = 0; i < _networkSynchronization.AllQuestsInfos.Count; i++)
            {
                Debug.Log($"Res {_networkSynchronization.AllQuestsInfos[i].Quest} {_networkSynchronization.AllQuestsInfos[i].State}");
            }

            
            _questCounter = FinedMaxNumber(_allQuestFirstQueue);
            Debug.Log($"Counter {_questCounter}");
            if (_questCounter > 0)
            {
                ExecutePrevious(_allQuestFirstQueue);
                _questCounter = _questCounter++;
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
                questConfig, _canvas, npcHappiness, _happyLineController, number, _dialogPanelView, _questsInWork, _networkSynchronization);

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
            if (_networkSynchronization != null)
            {
                _networkSynchronization.AllPointsReceived -= GetQuestQueue;
            }

            if (_photonConnectionController != null)
            {
                _photonConnectionController.LeaveGame -= FreeQuests;
            }
        }
    }
}
