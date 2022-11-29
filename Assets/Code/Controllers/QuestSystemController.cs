using System;
using System.Collections.Generic;
using System.Linq;
using Code.Buildings;
using Code.Configs;
using Code.Interfaces;
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
        private readonly List<Vector3> _startPointsList = new List<Vector3>();
        private readonly Controllers _controllers = new Controllers();
        private readonly ResourcesCheckUnionController _resourceCheckUnionController;
        private readonly HappyLineController _happyLineController = new HappyLineController();
        private readonly List<Vector3> _buildingPlaces = new List<Vector3>();
        private readonly List<QuestNpcConfig> _questConfigs;
        private readonly LineElementView _messagePanelView;
        private readonly Canvas _canvas;
        private readonly Camera _camera;
        private readonly Transform _player;
        private readonly Transform _characterFolder;
        private readonly Transform _buildingFolder;
        private readonly int _characterID;

        public QuestSystemController(QuestNpcConfig[] questConfigs, int characterID,
            ResourcesCheckUnionController resourceCheckUnionController, LineElementView messagePanelView, Canvas canvas,
            Camera camera, Transform player)
        {
            _characterFolder = new GameObject("Characters").transform;
            _buildingFolder = new GameObject("Buildings").transform;
            _questConfigs = questConfigs.ToList();
            _resourceCheckUnionController = resourceCheckUnionController;
            _messagePanelView = messagePanelView;
            _canvas = canvas;
            _camera = camera;
            _player = player;
            _characterID = characterID;
            _buildingPlaces.Add(new Vector3(0.0f, 0.0f, -85.0f));
            _buildingPlaces.Add(new Vector3(20.0f, 0.0f, -85.0f));
            _buildingPlaces.Add(new Vector3(40.0f, 0.0f, -85.0f));

            _controllers.Add(_happyLineController);
            _happyLineController.StartNewQuest += StartQuest;
        }

        private void StartQuest(int count)
        {
            if (_buildingPlaces.Count > _happyLineController.Population)
            {
                var npc = new NpcSpawnHandler(_questConfigs[0].NpcConfig,
                    GenerateStartPoint(_questConfigs[0].NpcConfig));
                var npcController = new NpcController(_questConfigs[0].NpcConfig, npc, _player, _characterID);
                npc.NpcTransform.SetParent(_characterFolder);
                //var target = new Vector3(-30.0f, 0.0f, -30.0f);
                var target = _buildingPlaces[Random.Range(0, _buildingPlaces.Count)];
                npcController.OnGetTarget(npc.NpcTransform.position, target);
            }

            if (_questConfigs.Count > 0)
            {
                Debug.Log("Has quests");
                var quest = ChooseQuest();
                InitQuest(quest.Item1, quest.Item2);
            }
            else
            {
                Debug.Log("All quests is done");
            }
        }

        private (QuestNpcConfig, Transform) ChooseQuest()
        {
            var i = Random.Range(0, _questConfigs.Count);
            QuestNpcConfig quest = _questConfigs[i];

            var newQuestSearch = (quest, _questConfigs[i].BuildingConfig.Places[0]);

            Transform[] places = _questConfigs[i].BuildingConfig.Places;
            List<Transform> freePlaces = new List<Transform>();

            for (int j = 0; j < places.Length; j++)
            {
                if (!_buildingPlaces.Contains(places[j].position))
                {
                    freePlaces.Add(places[j]);
                }
            }

            if (freePlaces.Count > 0)
            {
                Transform place = freePlaces[Random.Range(0, _questConfigs[i].BuildingConfig.Places.Length)];
                _buildingPlaces.Add(place.position);
                newQuestSearch = (quest, place);
                //Debug.Log(place.position);
            }
            else
            {
                _questConfigs.Remove(_questConfigs[i]);
                newQuestSearch = ChooseQuest();
            }

            return newQuestSearch;
        }

        private void InitQuest(QuestNpcConfig questConfig, Transform buildingPlace)
        {
            var startPosition = GenerateStartPoint(questConfig.NpcConfig);
            
            var npc = new NpcSpawnHandler(questConfig.NpcConfig, startPosition);
            npc.NpcTransform.SetParent(_characterFolder);
            var npcHappiness = new ResourcesKeeper(questConfig.StartHappiness, ResourcesType.Happiness);
            _happyLineController.ChangeCurrentPopulation(questConfig.StartHappiness);
            var quest = new QuestController(npc, _characterID, _resourceCheckUnionController,
                questConfig, _messagePanelView, _canvas, npcHappiness, _happyLineController);
            var buildingSpawnHandler =
                new BuildingSpawnHandler(questConfig.BuildingConfig, quest, buildingPlace, _buildingFolder);

            var npcController = new NpcController(questConfig.NpcConfig, npc, _player, _characterID, quest,
                buildingSpawnHandler);

            var npcView = new NpcViewHandler(npc, questConfig.NpcConfig, _canvas, quest, _camera, npcHappiness);

            _controllers.Add(quest);
            _controllers.Add(npcController);
            _controllers.Add(npcView);
            QuestAdd?.Invoke(quest);
        }

        private Vector3 GenerateStartPoint(NonPlayerCharacterConfig config)
        {
            var startPosition = config.SpawnPoints
                .GetChild(Random.Range(0, config.SpawnPoints.childCount)).position;

            if (!Check(startPosition))
            {
                _startPointsList.Add(startPosition);
            }
            else
            {
                startPosition.Set(startPosition.x + Random.Range(2, 5), startPosition.y,
                    startPosition.z);
                if (!Check(startPosition))
                {
                    _startPointsList.Add(startPosition);
                }
                else
                {
                    startPosition.Set(startPosition.x + Random.Range(2, 5), startPosition.y,
                        startPosition.z + Random.Range(2, 5));
                    _startPointsList.Add(startPosition);
                }
            }

            return startPosition;
        }

        private bool Check(Vector3 vector)
        {
            bool flag = false;
            for (int i = 0; i < _startPointsList.Count; i++)
            {
                if (_startPointsList[i] == vector)
                {
                    flag = true;
                }
            }

            return flag;
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
            _happyLineController.StartNewQuest -= StartQuest;
        }
    }
}
