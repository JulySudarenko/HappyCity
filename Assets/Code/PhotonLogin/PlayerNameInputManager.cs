using Code.PlayFabLogin;
using Code.View;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace Code.PhotonLogin
{
    internal sealed class PlayerNameInputManager
    {
        private const string PlayerNamePrefKey = "PlayerName";

        private readonly LoadingIndicatorView _loadingIndicator;
        private readonly InputField _playerNameInput;
        private string _playerName;
        private bool _hasName;

        public PlayerNameInputManager(InputField playerName, LoadingIndicatorView loadingIndicator)
        {
            _playerNameInput = playerName;
            _loadingIndicator = loadingIndicator;
            _playerNameInput.onEndEdit.AddListener(SetPlayerName);
            CheckPreviousInput();
        }

        private void CheckPreviousInput()
        {
            _hasName = PlayerPrefs.HasKey(PlayerNamePrefKey);
            if (_hasName)
            {
                _playerName = PlayerPrefs.GetString(PlayerNamePrefKey);
                _playerNameInput.text = _playerName;
                PhotonNetwork.NickName = _playerName;
            }
            else
            {
                _playerName = string.Empty;
                PhotonNetwork.NickName = _playerName;
            }
        }

        private void SetPlayerName(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                _loadingIndicator.ShowLoadingStatusInformation(ConnectionState.Default, "Player name not entered");
            }
            else
            {
                PhotonNetwork.NickName = value;
                PlayerPrefs.SetString(PlayerNamePrefKey, value);
            }
        }

        public void OnDestroy()
        {
            _playerNameInput.onEndEdit.RemoveListener(SetPlayerName);
        }
    }
}
