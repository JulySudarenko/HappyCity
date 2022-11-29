using System.Collections.Generic;
using Code.Configs;
using Code.Interfaces;
using Code.Quest;
using Code.ResourcesC;
using Code.View;
using Code.ViewHandlers;
using UnityEngine;

namespace Code.Controllers
{
    internal class ViewController : IInitialization, ICleanup
    {
        private readonly ResourcesPanelViewHandler _resourcesPanelViewHandler;
        private readonly List<IQuestState> _questList = new List<IQuestState>();
        private readonly TasksPanelViewHandler _tasksPanelViewHandler;
        private readonly QuestSystemController _questControllers;

        public ViewController(UnionConfig unionConfig, Transform resourcesPanelView,
            ImageLineElement resourceLineElement, Transform tasksPanelView, LineElementView tasksLineElement,
            ResourceCounterController woodCounter, ResourceCounterController foodCounter,
            ResourceCounterController stoneCounter, ResourceCounterController goldCounter, QuestSystemController questControllers)
        {
            _questControllers = questControllers;
            _resourcesPanelViewHandler =
                new ResourcesPanelViewHandler(unionConfig, resourcesPanelView, resourceLineElement,
                    woodCounter, foodCounter, stoneCounter, goldCounter);

            _tasksPanelViewHandler = new TasksPanelViewHandler(tasksPanelView, tasksLineElement);
        }

        public void Initialize()
        {
            _resourcesPanelViewHandler.Initialize();
            _tasksPanelViewHandler.Initialize();
            _questControllers.QuestAdd += AddNewQuest;
            // for (int i = 0; i < _questControllers.QuestList.Count; i++)
            // {
            //     _questControllers.QuestList[i].OnQuestStart += _tasksPanelViewHandler.OnTaskAdd;
            //     _questControllers.QuestList[i].OnQuestDone += _tasksPanelViewHandler.OnTaskRemove;
            // }
        }

        public void AddNewQuest(IQuestState state)
        {
            _questList.Add(state);
            state.OnQuestStart += _tasksPanelViewHandler.OnTaskAdd;
            state.OnQuestDone += _tasksPanelViewHandler.OnTaskRemove;
        }

        public void Cleanup()
        {
            for (int i = 0; i < _questList.Count; i++)
            {
                _questList[i].OnQuestStart -= _tasksPanelViewHandler.OnTaskAdd;
                _questList[i].OnQuestDone -= _tasksPanelViewHandler.OnTaskRemove;
            }
            _questControllers.QuestAdd -= AddNewQuest;
        }
    }
}
