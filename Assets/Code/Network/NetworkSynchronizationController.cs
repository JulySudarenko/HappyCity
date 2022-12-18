using System;
using System.Collections.Generic;
using Code.Assistance;
using Code.Quest;
using Code.ResourcesSpawn;
using Code.View;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;


namespace Code.Network
{
    public class NetworkSynchronizationController : MonoBehaviourPun, IOnEventCallback
    {
        public Action<int> AllPointsReceived;
        public Action<Vector3, int> ChangeParameter;
        public Action<string, int> AddScore;
        public Action<float> ChangeTimer;
        private LoadingIndicatorView _loadingIndicator;
        public List<Vector3> AllWoodPlaces { get; } = new List<Vector3>();
        public List<Vector3> AllStonePlaces { get; } = new List<Vector3>();
        public List<Vector3> AllFoodPlaces { get; } = new List<Vector3>();
        public List<NetQuestsInfo> AllQuestsInfos { get; } = new List<NetQuestsInfo>();

        private int _woodCount;
        private int _foodCount;
        private int _stoneCount;
        private int _questFirstQueueCount;
        private bool _hasQuestStates;

        public void Init(LoadingIndicatorView loadingIndicatorView,
            UnionResourcesConfigParser unionResourcesConfigParser)
        {
            _loadingIndicator = loadingIndicatorView;
            PhotonPeer.RegisterType(typeof(Vector2Int), 242, SerialiseAssistance.SerializeVector3,
                SerialiseAssistance.DeserializeVector3);
            _woodCount = unionResourcesConfigParser.WoodTotalCount;
            _foodCount = unionResourcesConfigParser.FoodTotalCount;
            _stoneCount = unionResourcesConfigParser.StoneTotalCount;
        }

        public void OnEnable() => PhotonNetwork.AddCallbackTarget(this);
        public void OnDisable() => PhotonNetwork.RemoveCallbackTarget(this);

        public void OnEvent(EventData photonEvent)
        {
            switch (photonEvent.Code)
            {
                case 111:
                    var vector = photonEvent.CustomData;
                    AllFoodPlaces.Add((Vector3) vector);
                    if (AllFoodPlaces.Count == _foodCount)
                    {
                        AllPointsReceived?.Invoke(photonEvent.Code);
                    }

                    break;
                case 112:
                    AllWoodPlaces.Add((Vector3) photonEvent.CustomData);
                    if (AllWoodPlaces.Count == _woodCount)
                    {
                        AllPointsReceived?.Invoke(photonEvent.Code);
                    }

                    break;
                case 113:
                    AllStonePlaces.Add((Vector3) photonEvent.CustomData);
                    if (AllStonePlaces.Count == _stoneCount)
                    {
                        AllPointsReceived?.Invoke(photonEvent.Code);
                    }

                    break;
                case 114:
                    ChangeParameter?.Invoke((Vector3) photonEvent.CustomData, photonEvent.Code);
                    break;
                case 115:
                    ChangeParameter?.Invoke((Vector3) photonEvent.CustomData, photonEvent.Code);
                    break;
                case 120: // количество квестов
                    _questFirstQueueCount = (int) photonEvent.CustomData;
                    break;
                case 122: // квест взят одним из игроков
                    ChangeParameter?.Invoke((Vector3) photonEvent.CustomData, photonEvent.Code);
                    ChangeQuestState((Vector3) photonEvent.CustomData, QuestState.Busy);
                    break;
                case 123: // квест выполнен  
                    ChangeParameter?.Invoke((Vector3) photonEvent.CustomData, photonEvent.Code);
                    ChangeQuestState((Vector3) photonEvent.CustomData, QuestState.Done);
                    break;
                case 124: // квест освобожден
                    ChangeParameter?.Invoke((Vector3) photonEvent.CustomData, photonEvent.Code);
                    ChangeQuestState((Vector3) photonEvent.CustomData, QuestState.Start);
                    break;
                case 125: // принят список квестов со статусами
                    // список всех квестов
                    //     // x - порядковый номер,
                    //     // y - номер квеста в конфиге,
                    //     // z - номер стартовой позиции в конфиге
                    if (!_hasQuestStates && !PhotonNetwork.IsMasterClient)
                    {
                        NetQuestsInfo questData = JsonUtility.FromJson<NetQuestsInfo>((string) photonEvent.CustomData);
                        AllQuestsInfos.Add(new NetQuestsInfo(questData.Quest, questData.State));
                        if (AllQuestsInfos.Count == _questFirstQueueCount)
                        {
                            AllPointsReceived?.Invoke(photonEvent.Code);
                            _hasQuestStates = true;
                        }
                    }

                    if (!_hasQuestStates && PhotonNetwork.IsMasterClient)
                    {
                        AllPointsReceived?.Invoke(photonEvent.Code);
                        _hasQuestStates = true;
                    }

                    break;

                case 131: // передача очков
                    NetScoreTable message = JsonUtility.FromJson<NetScoreTable>((string) photonEvent.CustomData);
                    AddScore?.Invoke(message.Name, message.Score);
                    break;
                case 132: // синхронизация времени
                    ChangeTimer?.Invoke((float) photonEvent.CustomData);
                    break;
                default:
                    //_loadingIndicator.UpdateFeedbackText($"Default {photonEvent.Code.ToString()}");
                    break;
            }
        }

        public void CreateQuestQueue(Vector3[] questQueue)
        {
            for (int i = 0; i < questQueue.Length; i++)
            {
                AllQuestsInfos.Add(new NetQuestsInfo(questQueue[i], QuestState.None));
            }

            _questFirstQueueCount = questQueue.Length;
        }

        public void ChangeQuestState(Vector3 quest, QuestState newState)
        {
            for (int i = 0; i < AllQuestsInfos.Count; i++)
            {
                if (AllQuestsInfos[i].Quest == quest)
                {
                    AllQuestsInfos[i].ChangeState(newState);
                    //AllQuestsInfos[i].State = newState;
                    _loadingIndicator.UpdateFeedbackText($"Change state {AllQuestsInfos[i].Quest} {AllQuestsInfos[i].State}");
                    _loadingIndicator.UpdateFeedbackText(newState.ToString());
                }
            }
        }
    }

    public struct NetScoreTable
    {
        public string Name;
        public int Score;
    }

    public class NetQuestsInfo
    {
        public Vector3 Quest;
        public QuestState State;

        public NetQuestsInfo(Vector3 quest, QuestState state)
        {
            Quest = quest;
            State = state;
        }

        public void ChangeState(QuestState newState)
        {
            State = newState;
            Debug.Log(State);
        }
    }
}
