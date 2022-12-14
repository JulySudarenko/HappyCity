using Code.Assistance;
using Code.Configs;
using Code.PlayFabLogin;
using Code.View;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;


namespace Code.PhotonLogin
{
    public class PhotonLogin : MonoBehaviourPunCallbacks
    {
        [SerializeField] private GameRoomView _gameRoomView;
        [SerializeField] private LoadingIndicatorView _loadingIndicator;
        [SerializeField] private MusicConfig _musicConfig;
        private PlayerNameInputManager _nameInputManager;
        private AudioSource _audio;
        private const string GameVersion = "1.0";
        private bool _isConnecting;

        private void Awake()
        {
            if (PhotonNetwork.InRoom)
            {
                PhotonNetwork.LeaveRoom();
            }

            _gameRoomView.PlayButton.onClick.AddListener(Connect);
            PhotonNetwork.AutomaticallySyncScene = true;
            _audio = gameObject.GetOrAddComponent<AudioSource>();
            _audio.clip = _musicConfig.ButtonsSound;
            _loadingIndicator.ShowLoadingStatusInformation(ConnectionState.Default,
                "Enter your name and choose character.");
            _gameRoomView.PlayButton.onClick.AddListener(PlaySound);
            _nameInputManager = new PlayerNameInputManager(_gameRoomView.PlayerName, _loadingIndicator);
        }

        private void PlaySound()
        {
            _audio.Play();
        }

        private void Connect()
        {
            _loadingIndicator.ShowLoadingStatusInformation(ConnectionState.Waiting, "Waiting for connection...");

            _isConnecting = true;
            if (PhotonNetwork.IsConnected)
            {
                _loadingIndicator.UpdateFeedbackText("Joining Room...");
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                _loadingIndicator.UpdateFeedbackText("Connecting...");
                PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = GameVersion;
            }
        }

        public override void OnConnectedToMaster()
        {
            if (_isConnecting)
            {
                _loadingIndicator.UpdateFeedbackText("Connecting...");
                PhotonNetwork.JoinRandomRoom();
            }
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            _loadingIndicator.UpdateFeedbackText("OnJoinRandomFailed: Next -> Create a new Room");
            PhotonNetwork.CreateRoom(null);
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            _loadingIndicator.ShowLoadingStatusInformation(ConnectionState.Fail, $"OnDisconnected {cause}");

            Debug.LogError("Disconnected");
            _isConnecting = false;
        }

        public override void OnJoinedRoom()
        {
            _loadingIndicator.ShowLoadingStatusInformation(ConnectionState.Success,
                $"OnJoinedRoom with {PhotonNetwork.CurrentRoom.PlayerCount} Player(s)");

            PhotonNetwork.LoadLevel(2);
        }

        private void OnDestroy()
        {
            _gameRoomView.PlayButton.onClick.RemoveListener(Connect);
            _gameRoomView.PlayButton.onClick.RemoveListener(PlaySound);
            _nameInputManager.OnDestroy();
        }
    }
}
