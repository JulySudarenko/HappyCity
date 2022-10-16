using Code.Configs;
using Code.Interfaces;
using Code.View;
using Code.ViewHandlers;
using UnityEngine;

namespace Code.Controllers
{
    internal class ViewController : IInitialization, ICleanup
    {
        private readonly ResourcesPanelViewHandler _resourcesPanelViewHandler;
        private readonly TasksPanelViewHandler _tasksPanelViewHandler;
        private readonly QuestController _questController;

        public ViewController(UnionResourcesConfig unionResourcesConfig, Transform resourcesPanelView,
            ImageLineElement resourceLineElement, Transform tasksPanelView, LineElementView tasksLineElement,
            ResourceCounterController woodCounter, ResourceCounterController foodCounter,
            ResourceCounterController stoneCounter, QuestController questController)
        {
            _questController = questController;
            _resourcesPanelViewHandler =
                new ResourcesPanelViewHandler(unionResourcesConfig, resourcesPanelView, resourceLineElement,
                    woodCounter, foodCounter, stoneCounter);

            _tasksPanelViewHandler = new TasksPanelViewHandler(tasksPanelView, tasksLineElement);
        }

        public void Initialize()
        {
            _resourcesPanelViewHandler.Initialize();
            _tasksPanelViewHandler.Initialize();
            _questController.QuestStart += _tasksPanelViewHandler.OnTaskAdd;
            _questController.QuestDone += _tasksPanelViewHandler.OnTaskRemove;
        }

        public void Cleanup()
        {
            _questController.QuestStart -= _tasksPanelViewHandler.OnTaskAdd;
            _questController.QuestDone -= _tasksPanelViewHandler.OnTaskRemove;
        }
    }
}
