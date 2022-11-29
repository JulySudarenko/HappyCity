using Code.Assistance;
using UnityEngine;

namespace Code.Configs
{
    [CreateAssetMenu(fileName = "UnionResourcesConfig", menuName = "UnionResourcesConfig", order = 0)]
    public sealed class UnionConfig : ScriptableObject
    {
        [SerializeField] private string _resourcesFolder = "ResConfigs";
        [SerializeField] private string _questFolder = "QuestConfigs";
        
        private ResourcesConfig[] _resourcesConfigs;
        private QuestNpcConfig[] _questNpcConfigs;

        public ResourcesConfig[] AllResourcesConfigs
        {
            get
            {
                _resourcesConfigs = Assistant.LoadAll<ResourcesConfig>(_resourcesFolder);
                return _resourcesConfigs;
            }
        }
        
        public QuestNpcConfig[] AllQuestNpcConfigs
        {
            get
            {
                _questNpcConfigs = Assistant.LoadAll<QuestNpcConfig>(_questFolder);
                return _questNpcConfigs;
            }
        }
    }
}
