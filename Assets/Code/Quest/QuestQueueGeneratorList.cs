using System.Collections.Generic;
using Code.Configs;
using UnityEngine;

namespace Code.Quest
{
    internal class QuestQueueGeneratorList
    {
        private readonly List<int> _places = new List<int>();
        private readonly List<int> _quests = new List<int>();
        private readonly List<Vector3> _questQueueList = new List<Vector3>();

        public Vector3[] QuestQueueList => _questQueueList.ToArray();

        public QuestQueueGeneratorList(UnionConfig unionConfig)
        {
            CreatePlacesNumbersList(unionConfig.AllBuildingPlacesConfigs[0].Places.Length);
            CreateQuestNumbersList(unionConfig.AllQuestNpcConfigs);
            CreateQueueList(_quests.Count);
        }

        private void CreatePlacesNumbersList(int count)
        {
            for (int i = 0; i < count; i++)
            {
                _places.Add(i);
            }
        }

        private void CreateQuestNumbersList(QuestNpcConfig[] questsConfigs)
        {
            for (int i = 0; i < questsConfigs.Length; i++)
            {
                for (int j = 0; j < questsConfigs[i].BuildingConfig.NumberOfBuildings; j++)
                {
                    _quests.Add(i);
                }
            }
        }

        private void CreateQueueList(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var randomQuest = Random.Range(0, _quests.Count);
                var randomPoint = Random.Range(0, _places.Count);
                _questQueueList.Add(new Vector3(i, _quests[randomQuest], _places[randomPoint]));
                _places.Remove(_places[randomPoint]);
                _quests.Remove(_quests[randomQuest]);
            }
        }
    }
    
    

    internal class HappyPointsJsonLine
    {
        public int PlayerID;
        public int Points;
    }
}
