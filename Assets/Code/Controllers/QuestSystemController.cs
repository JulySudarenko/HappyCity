using System.Collections.Generic;
using Code.Buildings;
using Code.Character;
using Code.Configs;
using Code.Factory;
using Code.Interfaces;
using Code.NPC;
using Code.Quest;
using Code.ResourcesC;
using Code.View;
using Code.ViewHandlers;
using UnityEngine;

namespace Code.Controllers
{
    internal class QuestSystemController : IInitialization, IFixedExecute, IExecute, ILateExecute, ICleanup
    {
        private readonly List<IQuestState> _questList = new List<IQuestState>();
        private readonly Controllers _controllers = new Controllers();
        private readonly ResourcesCheckUnionController _resourceCheckUnionController;
        private readonly QuestNpcConfig _houseQuest;
        private readonly QuestNpcConfig _farmQuest;
        private readonly int _characterID;


        public QuestSystemController(QuestNpcConfig houseQuest, QuestNpcConfig farmQuest, int characterID,
            ResourcesCheckUnionController resourceCheckUnionController, LineElementView messagePanelView, Canvas canvas, Camera camera)
        {
            _resourceCheckUnionController = resourceCheckUnionController;
            _houseQuest = houseQuest;
            _farmQuest = farmQuest;
            _characterID = characterID;
            InitQuest(_houseQuest, messagePanelView, canvas, camera);
            InitQuest(_farmQuest, messagePanelView, canvas, camera);
        }

        public List<IQuestState> QuestList => _questList;

        private void InitQuest(QuestNpcConfig questConfig, LineElementView messagePanelView, Canvas canvas, Camera camera)
        {
            var npc = new NpcSpawnHandler(questConfig.NpcConfig);
            var quest = new QuestController(npc, _characterID, _resourceCheckUnionController,
                questConfig, messagePanelView, canvas);
            var houseSpawner = new BuildingSpawnHandler(questConfig.BuildingConfig, quest);
            var npcController = new NpcController(questConfig.NpcConfig, npc);
            var npcView = new NpcViewHandler(npc, questConfig.NpcConfig, canvas, quest, camera);
            _questList.Add(quest);
            _controllers.Add(quest);
            _controllers.Add(houseSpawner);
            _controllers.Add(npcController);
            _controllers.Add(npcView);
        }
        
        public void Initialize()
        {
            _controllers.Initialize();
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
        }

    }
}
