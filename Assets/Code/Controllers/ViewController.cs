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

        public ViewController(UnionResourcesConfig unionResourcesConfig, Transform resourcesPanelView,
            ImageLineElement resourceLineElement, Transform tasksPanelView, LineElementView tasksLineElement)
        {
            _resourcesPanelViewHandler =
                new ResourcesPanelViewHandler(unionResourcesConfig, resourcesPanelView, resourceLineElement);

            _tasksPanelViewHandler = new TasksPanelViewHandler(tasksPanelView, tasksLineElement);
        }

        public void Initialize()
        {
            _resourcesPanelViewHandler.Initialize();
            _tasksPanelViewHandler.Initialize();
        }

        public void Cleanup()
        {
        }
    }
}
