using UnityEngine;
using UnityEngine.UI;

namespace Code.View
{
    public sealed class GameRoomView : MonoBehaviour
    {
        [SerializeField] private InputField _playerNameInputField;

        [SerializeField] private Button _playButton;
        public InputField PlayerName => _playerNameInputField;
        public Button PlayButton => _playButton;
    }
}
