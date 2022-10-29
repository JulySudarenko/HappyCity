using Code.Configs;
using Code.Interfaces;
using Code.ResourcesC;
using Code.View;
using Code.ViewHandlers;
using UnityEngine;

namespace Code.Controllers
{
    internal class ViewController : IInitialization, ICleanup
    {
        private readonly ResourcesPanelViewHandler _resourcesPanelViewHandler;
        private readonly TasksPanelViewHandler _tasksPanelViewHandler;
        private readonly QuestSystemController _questControllers;

        public ViewController(UnionResourcesConfig unionResourcesConfig, Transform resourcesPanelView,
            ImageLineElement resourceLineElement, Transform tasksPanelView, LineElementView tasksLineElement,
            ResourceCounterController woodCounter, ResourceCounterController foodCounter,
            ResourceCounterController stoneCounter, ResourceCounterController goldCounter, QuestSystemController questControllers)
        {
            _questControllers = questControllers;
            _resourcesPanelViewHandler =
                new ResourcesPanelViewHandler(unionResourcesConfig, resourcesPanelView, resourceLineElement,
                    woodCounter, foodCounter, stoneCounter, goldCounter);

            _tasksPanelViewHandler = new TasksPanelViewHandler(tasksPanelView, tasksLineElement);
        }

        public void Initialize()
        {
            _resourcesPanelViewHandler.Initialize();
            _tasksPanelViewHandler.Initialize();
            for (int i = 0; i < _questControllers.QuestList.Count; i++)
            {
                _questControllers.QuestList[i].OnQuestStart += _tasksPanelViewHandler.OnTaskAdd;
                _questControllers.QuestList[i].OnQuestDone += _tasksPanelViewHandler.OnTaskRemove;
            }

        }

        public void Cleanup()
        {
            for (int i = 0; i < _questControllers.QuestList.Count; i++)
            {
                _questControllers.QuestList[i].OnQuestStart -= _tasksPanelViewHandler.OnTaskAdd;
                _questControllers.QuestList[i].OnQuestDone -= _tasksPanelViewHandler.OnTaskRemove;
            }
        }
    }
}
