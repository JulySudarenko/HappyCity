using System;
using Code.Configs;
using Code.Interfaces;
using Code.ResourcesC;
using Code.ResourcesSpawn;
using Code.View;
using UnityEngine;
using Object = UnityEngine.Object;


namespace Code.ViewHandlers
{
    internal class ResourcesPanelViewHandler : IInitialization
    {
        private readonly UnionConfig _unionConfig;
        private readonly Transform _resourcesPanelView;
        private readonly ImageLineElement _resourceLineElement;
        private ResourceCounterViewHandler _woodHandler;
        private ResourceCounterViewHandler _foodHandler;
        private ResourceCounterViewHandler _stoneHandler;
        private ResourceCounterViewHandler _goldHandler;
        private readonly ResourceCounterController _woodCounter;
        private readonly ResourceCounterController _foodCounter;
        private readonly ResourceCounterController _stoneCounter;
        private readonly ResourceCounterController _goldCounter;

        public ResourcesPanelViewHandler(UnionConfig unionConfig, Transform resourcesPanelView,
            ImageLineElement resourceLineElement,ResourceCounterController woodCounter, ResourceCounterController foodCounter,
            ResourceCounterController stoneCounter, ResourceCounterController goldCounter)
        {
            _unionConfig = unionConfig;
            _resourcesPanelView = resourcesPanelView;
            _resourceLineElement = resourceLineElement;
            _woodCounter = woodCounter;
            _foodCounter = foodCounter;
            _stoneCounter = stoneCounter;
            _goldCounter = goldCounter;
        }

        public void Initialize()
        {
            _resourcesPanelView.gameObject.SetActive(true);
            for (int i = 0; i < _unionConfig.AllResourcesConfigs.Length; i++)
            {
                var config = _unionConfig.AllResourcesConfigs[i];
                switch (config.Type)
                {
                    case ResourcesType.Wood:
                        _woodHandler = CreateNewlineElement(config);
                        _woodCounter.ChangeCount += _woodHandler.ChangeCount;
                        break;
                    case ResourcesType.Stone:
                        _stoneHandler = CreateNewlineElement(config);
                        _stoneCounter.ChangeCount += _stoneHandler.ChangeCount;
                        break;
                    case ResourcesType.Food:
                        _foodHandler = CreateNewlineElement(config);
                        _foodCounter.ChangeCount += _foodHandler.ChangeCount;
                        break;
                    case ResourcesType.Gold:
                        _goldHandler = CreateNewlineElement(config);
                        _goldCounter.ChangeCount += _goldHandler.ChangeCount;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private ResourceCounterViewHandler CreateNewlineElement(ResourcesConfig config)
        {
            ImageLineElement resLineElement = Object.Instantiate(_resourceLineElement, _resourcesPanelView);
            ResourceCounterViewHandler resourceCounter = new ResourceCounterViewHandler(resLineElement, config);
            return resourceCounter;
        }
    }
}
