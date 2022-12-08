using Code.Assistance;
using Code.View;
using UnityEngine;

namespace Code.PlayFabLogin
{
    public class LoginManager : MonoBehaviour
    {
        [SerializeField] private AuthorizationMenuView _authorizationMenu;
        [SerializeField] private LoadingIndicatorView _loadingIndicator;
        
        private Code.PlayFabLogin.PlayFabLogin _playFabLogin;
        private void Awake()
        {
            _playFabLogin = new Code.PlayFabLogin.PlayFabLogin(_authorizationMenu, _loadingIndicator);
        }
    }
}
