using System;
using Code.Configs;
using Code.View;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.PlayFabLogin
{
    public class PlayFabLogin : IDisposable
    {
        private readonly AuthorizationMenuView _authorizationMenu;
        private readonly LoadingIndicatorView _loadingIndicatorView;

        private const string AuthKey = "IdKey";
        private string _guid;

        private const string AuthKeyName = "IdName";
        private const string AuthKeyPassword = "IdPassword";
        private string _name;
        private string _password;

        private bool _isIdExist;

        public PlayFabLogin(AuthorizationMenuView authorizationMenu, LoadingIndicatorView loadingIndicatorView)
        {
            _authorizationMenu = authorizationMenu;
            _loadingIndicatorView = loadingIndicatorView;
            ActivateMenu();
            _authorizationMenu.SubscribeButtonsOnSound();
            _authorizationMenu.AuthorizationButton.onClick.AddListener(SignIn);
            _authorizationMenu.RegistrationButton.onClick.AddListener(CreateAccount);
        }

        private void ActivateMenu()
        {
            _isIdExist = PlayerPrefs.HasKey(AuthKey);

            if (_isIdExist)
            {
                _authorizationMenu.ChosePanel(true);
                _loadingIndicatorView.ShowLoadingStatusInformation(ConnectionState.Default, "Please, sing up");
            }
            else
            {
                _authorizationMenu.ChosePanel(false);
                _loadingIndicatorView.ShowLoadingStatusInformation(ConnectionState.Default, "Please, registry");
            }
        }

        private void CreateAccount()
        {
            _loadingIndicatorView.ShowLoadingStatusInformation(ConnectionState.Waiting, "Waiting for connection...");
            PlayFabClientAPI.RegisterPlayFabUser(new RegisterPlayFabUserRequest
                {
                    Username = _authorizationMenu.UserName,
                    Email = _authorizationMenu.UserMail,
                    Password = _authorizationMenu.UserPassword,
                    RequireBothUsernameAndEmail = true
                }, result =>
                {
                    _loadingIndicatorView.ShowLoadingStatusInformation(ConnectionState.Success,
                        $"Success: {_authorizationMenu.UserName} is connected");
                    //_guid = PlayerPrefs.GetString(AuthKey, Guid.NewGuid().ToString());
                    PlayerPrefs.SetString(AuthKey, _guid);
                    _isIdExist = PlayerPrefs.HasKey(AuthKey);
                    SceneManager.LoadScene(1);
                },
                error =>
                {
                    _loadingIndicatorView.ShowLoadingStatusInformation(ConnectionState.Fail,
                        $"Fail: {error.ErrorMessage}");
                });
        }

        private void SignIn()
        {
            _loadingIndicatorView.ShowLoadingStatusInformation(ConnectionState.Waiting, "Waiting for connection...");
            PlayFabClientAPI.LoginWithPlayFab(new LoginWithPlayFabRequest
                {
                    Username = _authorizationMenu.UserName,
                    Password = _authorizationMenu.UserPassword
                }, result =>
                {
                    _loadingIndicatorView.ShowLoadingStatusInformation(ConnectionState.Success,
                        $"Success: {_authorizationMenu.UserName}");
                    _guid = PlayerPrefs.GetString(AuthKey, Guid.NewGuid().ToString());

                    _name = _authorizationMenu.UserName;
                    _password = _authorizationMenu.UserPassword;
                    PlayerPrefs.SetString(AuthKeyName, _name);
                    PlayerPrefs.SetString(AuthKeyPassword, _password);

                    SceneManager.LoadScene(1);
                },
                error =>
                {
                    _loadingIndicatorView.ShowLoadingStatusInformation(ConnectionState.Fail,
                        $"Fail: {error.ErrorMessage}");
                });
        }

        // private void DeleteAccount()
        // {
        //     PlayerPrefs.DeleteKey(AuthKey);
        //     _loadingIndicatorView.ShowLoadingStatusInformation(ConnectionState.Success,
        //         "Account was deleted. Please, registry.");
        // }

        // private void SetUserData() {
        //     PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest() {
        //             Data = new Dictionary<string, string>() 
        //             {
        //                 {"Health", 3.ToString()}
        //             }
        //         },
        //         result => Debug.Log("Successfully updated user data"),
        //         error => {
        //             Debug.Log("Got error setting");
        //             Debug.Log(error.GenerateErrorReport());
        //         });
        // }


        public void Dispose()
        {
            _authorizationMenu.Dispose(_isIdExist);
            _authorizationMenu.DisposeSound();
            //_authorizationMenu.DeleteAccountButton.onClick.RemoveListener(DeleteAccount);
            _authorizationMenu.AuthorizationButton.onClick.RemoveListener(SignIn);
            _authorizationMenu.RegistrationButton.onClick.RemoveListener(CreateAccount);
        }
    }
}
