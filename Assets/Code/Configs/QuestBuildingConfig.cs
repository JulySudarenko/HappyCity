using UnityEngine;

namespace Code.Configs
{
    [CreateAssetMenu(fileName = "QuestBuildingConfig", menuName = "Data/QuestBuildingConfig", order = 0)]
    public sealed class QuestBuildingConfig : QuestConfig
    {
        [SerializeField] private string _npcConfigPath = "NonPlayerCharacterConfig";

        private NonPlayerCharacterConfig _npcConfig;

        public NonPlayerCharacterConfig NpcConfig
        {
            get
            {
                if (_npcConfig == null)
                {
                    _npcConfig = Assistance.Assistant.Load<NonPlayerCharacterConfig>(_npcConfigPath);
                }

                return _npcConfig;
            }
        }
    }
}
