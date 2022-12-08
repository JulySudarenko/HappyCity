using Code.Assistance;
using Code.Configs;
using UnityEngine;
using UnityEngine.UI;

namespace Code.View
{
    public class AuthorizationMenuView : MonoBehaviour
    {
        [Header("Registration menu")] [SerializeField]
        private GameObject _registrationPanel;

        [SerializeField] private InputField _userNameInputField;
        [SerializeField] private InputField _userMailInputField;
        [SerializeField] private InputField _userPasswordInputField;
        [SerializeField] private Button _registrationButton;
        [SerializeField] private Button _deleteAccountButton;
        [SerializeField] private Button _singInButton;

        [Header("Authorization menu")] [SerializeField]
        private GameObject _authorizationPanel;

        [SerializeField] private InputField _userNameAuthorization;
        [SerializeField] private InputField _userPasswordAuthorization;
        [SerializeField] private Button _authorizationButton;
        [SerializeField] private Button _newAccountButton;

        [Header("Config")] [SerializeField] private MusicConfig _musicConfig;
        private AudioSource _audioSource;

        private const string AuthKeyName = "IdName";
        private const string AuthKeyPassword = "IdPassword";
        
        private string _userName;
        private string _userMail;
        private string _userPassword;

        public string UserName => _userName;
        public string UserMail => _userMail;
        public string UserPassword => _userPassword;

        public Button DeleteAccountButton => _deleteAccountButton;
        public Button AuthorizationButton => _authorizationButton;
        public Button RegistrationButton => _registrationButton;

        private void UpdateUserName(string userName) => _userName = userName;
        private void UpdateUserEmail(string userMail) => _userMail = userMail;
        private void UpdateUserPassword(string userPassword) => _userPassword = userPassword;

        public void SubscribeButtonsOnSound()
        {
            _audioSource = gameObject.GetOrAddComponent<AudioSource>();
            _audioSource.clip = _musicConfig.ButtonsSound;

            _registrationButton.onClick.AddListener(PlaySound);
            _deleteAccountButton.onClick.AddListener(PlaySound);
            _singInButton.onClick.AddListener(PlaySound);
            _authorizationButton.onClick.AddListener(PlaySound);
            _newAccountButton.onClick.AddListener(PlaySound);

            if(PlayerPrefs.HasKey(AuthKeyName))
            {
                _userNameAuthorization.text = PlayerPrefs.GetString(AuthKeyName);
                _userName = PlayerPrefs.GetString(AuthKeyName);

                if (PlayerPrefs.HasKey(AuthKeyPassword))
                    _userPasswordAuthorization.text = PlayerPrefs.GetString(AuthKeyPassword);
                _userPassword = PlayerPrefs.GetString(AuthKeyPassword);
            }
        }

        private void PlaySound()
        {
            _audioSource.Play();
        }

        public void DisposeSound()
        {
            _registrationButton.onClick.RemoveListener(PlaySound);
            _deleteAccountButton.onClick.AddListener(PlaySound);
            _singInButton.onClick.AddListener(PlaySound);
            _authorizationButton.onClick.AddListener(PlaySound);
            _newAccountButton.onClick.AddListener(PlaySound);
        }


        public void ChosePanel(bool value)
        {
            if (value)
            {
                _authorizationPanel.SetActive(true);
                _userNameAuthorization.onEndEdit.AddListener(UpdateUserName);
                _userPasswordAuthorization.onEndEdit.AddListener(UpdateUserPassword);
                _newAccountButton.onClick.AddListener(() => ChosePanel(false));
            }
            else
            {
                _registrationPanel.SetActive(true);
                _userNameInputField.onEndEdit.AddListener(UpdateUserName);
                _userMailInputField.onEndEdit.AddListener(UpdateUserEmail);
                _userPasswordInputField.onEndEdit.AddListener(UpdateUserPassword);
                _singInButton.onClick.AddListener(() => ChosePanel(true));
            }

            Dispose(value);
        }

        public void Dispose(bool value)
        {
            if (value)
            {
                _registrationPanel.SetActive(false);
                _userNameInputField.onEndEdit.RemoveAllListeners();
                _userMailInputField.onEndEdit.RemoveAllListeners();
                _userPasswordInputField.onEndEdit.RemoveAllListeners();
                _singInButton.onClick.RemoveListener(() => ChosePanel(true));
            }
            else
            {
                _authorizationPanel.SetActive(false);
                _userNameAuthorization.onEndEdit.RemoveAllListeners();
                _userPasswordAuthorization.onEndEdit.RemoveAllListeners();
                _newAccountButton.onClick.RemoveListener(() => ChosePanel(false));
            }
        }
    }
}
