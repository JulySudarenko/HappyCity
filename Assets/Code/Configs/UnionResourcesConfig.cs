using Code.Assistance;
using UnityEngine;

namespace Code.Configs
{
    [CreateAssetMenu(fileName = "UnionResourcesConfig", menuName = "UnionResourcesConfig", order = 0)]
    public sealed class UnionResourcesConfig : ScriptableObject
    {
        [SerializeField] private string _resourcesFolder = "ResConfigs";

        private ResourcesConfig[] _resourcesConfigs;

        public ResourcesConfig[] AllResourcesConfigs
        {
            get
            {
                _resourcesConfigs = Assistant.LoadAll<ResourcesConfig>(_resourcesFolder);
                return _resourcesConfigs;
            }
        }
    }
}
