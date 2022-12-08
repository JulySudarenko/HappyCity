using System;
using Code.View;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;


namespace Code.Network
{
    internal class PhotonConnectionController : MonoBehaviourPunCallbacks
    {
        public Action NewPlayerConnection;
        public Action NewMasterConnection;
        private LoadingIndicatorView _loadingIndicator;

        public void Init(LoadingIndicatorView loadingIndicatorView)
        {
            _loadingIndicator = loadingIndicatorView;
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            _loadingIndicator.UpdateFeedbackText($"Player {newPlayer.NickName} says hello");
            NewPlayerConnection?.Invoke();
        }

        public override void OnLeftRoom()
        {
            Debug.Log("Load scene");
            PhotonNetwork.LoadLevel(1);
        }

        public void LeaveRoom()
        {
            // if (PhotonNetwork.InRoom)
            // {
            //     PhotonNetwork.LeaveRoom();
            //     Debug.Log("in room..........");
            // }
            // else
            // {
            //     Debug.Log("NOT in room..........");
            //     PhotonNetwork.LoadLevel(1);
            // }
            //
            // Debug.Log("Leave room finish");
            PhotonNetwork.LoadLevel(1);
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            _loadingIndicator.UpdateFeedbackText($"Player {otherPlayer.NickName} says goodbye");
        }

        public override void OnMasterClientSwitched(Player newMasterClient)
        {
            if (newMasterClient.NickName == PhotonNetwork.LocalPlayer.NickName)
            {
                Debug.Log(newMasterClient.NickName);
                NewMasterConnection?.Invoke();
            }
        }
    }
}
