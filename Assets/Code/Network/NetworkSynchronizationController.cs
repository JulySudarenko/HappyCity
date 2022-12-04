using System;
using System.Collections.Generic;
using Code.Assistance;
using Code.Factory;
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
        private PlayersList _playersList;
        private LoadingIndicatorView _loadingIndicator;
        public List<Vector3> AllWoodPlaces { get; } = new List<Vector3>();
        public List<Vector3> AllStonePlaces { get; } = new List<Vector3>();
        public List<Vector3> AllFoodPlaces { get; } = new List<Vector3>();
        public List<Vector3> QuestFirstQueue { get; } = new List<Vector3>();

        private int _woodCount;
        private int _foodCount;
        private int _stoneCount;
        private int _questFirstQueueCount;

        public void Init(LoadingIndicatorView loadingIndicatorView,
            UnionResourcesConfigParser unionResourcesConfigParser)
        {
            _playersList = new PlayersList(loadingIndicatorView);
            _loadingIndicator = loadingIndicatorView;
            PhotonPeer.RegisterType(typeof(Vector2Int), 242, SerialiseAssistance.SerializeVector3,
                SerialiseAssistance.DeserializeVector3);
            _woodCount = unionResourcesConfigParser.WoodTotalCount;
            _foodCount = unionResourcesConfigParser.FoodTotalCount;
            _stoneCount = unionResourcesConfigParser.StoneTotalCount;
        }

        public void AddPlayer(CharacterPhotonView character)
        {
            _playersList.AddPlayer(character);
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
                case 120:
                    _questFirstQueueCount = (int) photonEvent.CustomData;
                    _loadingIndicator.UpdateFeedbackText($"Quest count {photonEvent.CustomData}");
                    break;
                case 121:
                    QuestFirstQueue.Add((Vector3) photonEvent.CustomData);
                    if (QuestFirstQueue.Count == _questFirstQueueCount)
                    {
                        AllPointsReceived?.Invoke(photonEvent.Code);
                    }
                    break;
                case 122:
                    ChangeParameter?.Invoke((Vector3) photonEvent.CustomData, photonEvent.Code);
                    break;
                case 123:
                    ChangeParameter?.Invoke((Vector3) photonEvent.CustomData, photonEvent.Code);
                    break;
                case 131:
                    NetScoreTable message = JsonUtility.FromJson<NetScoreTable>((string) photonEvent.CustomData);
                    AddScore?.Invoke(message.Name, message.Score);
                    break;
                default:
                    //_loadingIndicator.UpdateFeedbackText($"Default {photonEvent.Code.ToString()}");
                    break;
            }
        }
    }

    public struct NetScoreTable
    {
        public string Name;
        public int Score;
    }
    


}
