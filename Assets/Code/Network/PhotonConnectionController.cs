using System;
using Code.View;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;


namespace Code.Network
{
    internal class PhotonConnectionController : MonoBehaviourPunCallbacks
    {
        public Action NewPlayerConnection;
        public Action NewMasterConnection;
        public Action LeaveGame;
        private LoadingIndicatorView _loadingIndicator;
        private Button _exitButton;

        public PhotonConnectionController(Action leaveGame)
        {
            LeaveGame = leaveGame;
        }

        public void Init(LoadingIndicatorView loadingIndicatorView, Button exitButton)
        {
            _loadingIndicator = loadingIndicatorView;
            _exitButton = exitButton;
            _exitButton.onClick.AddListener(LeaveRoom);
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            _loadingIndicator.UpdateFeedbackText($"Player {newPlayer.NickName} says hello");
            NewPlayerConnection?.Invoke();
        }

        public override void OnLeftRoom()
        {
            PhotonNetwork.LoadLevel(1);
        }

        public void LeaveRoom()
        {
            LeaveGame?.Invoke();
            PhotonNetwork.LeaveRoom();
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            _loadingIndicator.UpdateFeedbackText($"Player {otherPlayer.NickName} says goodbye");
        }

        public override void OnMasterClientSwitched(Player newMasterClient)
        {
            if (newMasterClient.NickName == PhotonNetwork.LocalPlayer.NickName)
            {
                NewMasterConnection?.Invoke();
                _loadingIndicator.UpdateFeedbackText($"Player {newMasterClient.NickName} is master now");
            }
        }
    }
}
