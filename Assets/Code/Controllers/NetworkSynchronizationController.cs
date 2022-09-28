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


namespace Code.Controllers
{
    public class NetworkSynchronizationController : MonoBehaviourPun, IOnEventCallback
    {
        public Action<int> AllPointsReceived;
        private PlayersList _playersList;
        private LoadingIndicatorView _loadingIndicator;
        public List<Vector3> AllWoodPlaces { get; } = new List<Vector3>();
        public List<Vector3> AllStonePlaces { get; } = new List<Vector3>();
        public List<Vector3> AllFoodPlaces { get; } = new List<Vector3>();

        private int _woodCount;
        private int _foodCount;
        private int _stoneCount;

        public void Init(LoadingIndicatorView loadingIndicatorView, UnionResourcesConfigParser unionResourcesConfigParser)
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
                case 101:
                    //_playersList.AddPlayer(photonEvent.CustomData);
                    break;
                case 111:
                    var vector = photonEvent.CustomData;
                    AllFoodPlaces.Add((Vector3) vector);
                    _loadingIndicator.UpdateFeedbackText(vector.ToString());
                    if (AllFoodPlaces.Count == _foodCount)
                    {
                        AllPointsReceived?.Invoke(photonEvent.Code);

                    }

                    break;
                case 112:
                    AllWoodPlaces.Add((Vector3) photonEvent.CustomData);
                    //_loadingIndicator.UpdateFeedbackText(photonEvent.CustomData.ToString());
                    if (AllWoodPlaces.Count == _woodCount)
                    {
                        AllPointsReceived?.Invoke(photonEvent.Code);
                    }

                    break;
                case 113:
                    AllStonePlaces.Add((Vector3) photonEvent.CustomData);
                    _loadingIndicator.UpdateFeedbackText(photonEvent.CustomData.ToString());
                    if (AllStonePlaces.Count == _stoneCount)
                    {
                        AllPointsReceived?.Invoke(photonEvent.Code);
                    }

                    break;
                default:
                    //_loadingIndicator.UpdateFeedbackText(photonEvent.Code.ToString());
                    //_loadingIndicator.UpdateFeedbackText(photonEvent.CustomData.ToString());
                    break;
            }
        }
    }
}
