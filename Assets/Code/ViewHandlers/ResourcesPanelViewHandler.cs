using System;
using Code.Configs;
using Code.Factory;
using Code.Interfaces;
using Code.View;
using UnityEngine;
using Object = UnityEngine.Object;


namespace Code.Controllers
{
    internal class ResourcesPanelViewHandler : IInitialization
    {
        private readonly UnionResourcesConfig _unionResourcesConfig;
        private readonly Transform _resourcesPanelView;
        private readonly ImageLineElement _resourceLineElement;

        public ResourcesPanelViewHandler(UnionResourcesConfig unionResourcesConfig, Transform resourcesPanelView,
            ImageLineElement resourceLineElement)
        {
            _unionResourcesConfig = unionResourcesConfig;
            _resourcesPanelView = resourcesPanelView;
            _resourceLineElement = resourceLineElement;
        }

        public void Initialize()
        {
            _resourcesPanelView.gameObject.SetActive(true);
            for (int i = 0; i < _unionResourcesConfig.AllResourcesConfigs.Length; i++)
            {
                var config = _unionResourcesConfig.AllResourcesConfigs[i];
                switch (config.Type)
                {
                    case ResourcesType.Wood:
                        CreateNewlineElement(config);
                        break;
                    case ResourcesType.Stone:
                        CreateNewlineElement(config);
                        break;
                    case ResourcesType.Food:
                        CreateNewlineElement(config);
                        break;
                    case ResourcesType.Gold:
                        CreateNewlineElement(config);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private ImageLineElement CreateNewlineElement(ResourcesConfig config)
        {
            ImageLineElement resLineElement = Object.Instantiate(_resourceLineElement, _resourcesPanelView);
            resLineElement.gameObject.SetActive(true);
            resLineElement.Icon.sprite = config.Icon;
            resLineElement.Description.text = config.StartValue.ToString();
            return resLineElement;
        }
    }
    
    
    internal class ResourceCounterViewHandler
    {
    }
}
