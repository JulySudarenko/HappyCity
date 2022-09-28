using System;
using Code.View;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

namespace Code.Controllers
{
    internal class PhotonConnectionController : MonoBehaviourPunCallbacks
    {
        public Action OnNewPlayerConnection;
        private LoadingIndicatorView _loadingIndicator;

        public void Init(LoadingIndicatorView loadingIndicatorView)
        {
            _loadingIndicator = loadingIndicatorView;
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            _loadingIndicator.UpdateFeedbackText($"Player {newPlayer.NickName} says hello");
            OnNewPlayerConnection?.Invoke();
        }

        public override void OnLeftRoom()
        {
            SceneManager.LoadScene("Profile");
        }

        public void LeaveRoom() => PhotonNetwork.LeaveRoom();

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            _loadingIndicator.UpdateFeedbackText($"Player {otherPlayer.NickName} says goodbye");

        }
    }
}
