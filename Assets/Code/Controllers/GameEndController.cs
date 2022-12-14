using System;
using System.Collections.Generic;
using Code.Interfaces;
using Code.Network;
using Code.Quest;
using Code.ResourcesC;


namespace Code.Controllers
{
    internal class GameEndController : ICleanup
    {
        public Action<Dictionary<string, int>, string> EndGame;
        public Action StartGame;

        private readonly QuestSystemController _questSystemController;
        private readonly NetworkSynchronizationController _networkSynchronizationController;
        private readonly List<IQuestState> _questList = new List<IQuestState>();
        private readonly Dictionary<string, int> _scoreTable = new Dictionary<string, int>();
        private readonly ResourceCounterController _grandGoldController;

        private readonly string _nickName;
        private readonly int _winGold = 50;
        private int _myScore = 0;
        private int _buildCounter;

        public GameEndController(QuestSystemController questSystemController,
            NetworkSynchronizationController networkSynchronizationController,
            ResourceCounterController grandGoldController, string nickName)
        {
            _questSystemController = questSystemController;
            _networkSynchronizationController = networkSynchronizationController;
            _grandGoldController = grandGoldController;
            _nickName = nickName;
            _networkSynchronizationController.AllPointsReceived += FirstQuestQueueReceive;
            _networkSynchronizationController.AddScore += AddScore;
            _questSystemController.QuestAdd += OnQuestAdd;
        }

        private void AddScore(string name, int score)
        {
            if (_scoreTable.ContainsKey(name))
            {
                _scoreTable[name] += score;
            }
            else
            {
                _scoreTable.Add(name, score);
            }

            if (name == _nickName)
            {
                _myScore += score;
            }
        }

        private void FirstQuestQueueReceive(int code)
        {
            if (code == 125)
            {
                _buildCounter = _networkSynchronizationController.AllQuestsInfos.Count;
                StartGame?.Invoke();
            }
        }

        private void OnQuestAdd(IQuestState quest)
        {
            _questList.Add(quest);
            quest.OnStateChange += OnQuestDone;
        }

        private void OnQuestDone(QuestState state)
        {
            if (state == QuestState.Done)
            {
                _buildCounter--;
                if (_buildCounter == 0)
                {
                    var winner = String.Empty;
                    var score = 0;

                    foreach (var line in _scoreTable)
                    {
                        if (score < line.Value)
                        {
                            score = line.Value;
                            winner = line.Key;
                        }
                        else
                        {
                            if (score == line.Value)
                            {
                                winner += $", {line.Key}";
                            }
                        }
                    }

                    _grandGoldController.OnGrandResource(_winGold);

                    if (_myScore == score)
                    {
                        _grandGoldController.OnGrandResource(_winGold);
                    }

                    EndGame?.Invoke(_scoreTable, winner);

                    for (int i = 0; i < _questList.Count; i++)
                    {
                        _questList[i].OnStateChange -= OnQuestDone;
                    }

                    _questSystemController.QuestAdd -= OnQuestAdd;
                }
            }
        }

        public void Cleanup()
        {
            if (_buildCounter == 0)
            {
                for (int i = 0; i < _questList.Count; i++)
                {
                    _questList[i].OnStateChange -= OnQuestDone;
                }
            }

            if (_questSystemController != null) _questSystemController.QuestAdd -= OnQuestAdd;
        }
    }
}
