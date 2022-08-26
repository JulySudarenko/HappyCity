using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Code.PlayFabLogin
{
    public class AdminLogin : MonoBehaviour
    {
        [SerializeField] private Button _adminEnterButton;
        
        private const string AuthKeyName = "IdName";
        private const string AuthKeyPassword = "IdPassword";

        private void Awake()
        {
            _adminEnterButton.onClick.AddListener(Login);
        }

        private void Login()
        {
            PlayFabClientAPI.LoginWithPlayFab(new LoginWithPlayFabRequest
                {
                    Username = PlayerPrefs.GetString(AuthKeyName),
                    Password = PlayerPrefs.GetString(AuthKeyPassword)
                }, result =>
                {
                   
                    SceneManager.LoadScene(1);
                },
                error =>
                {
                    Debug.Log($"Fail: {error.ErrorMessage}");
                });
        }
    }
}
