using UnityEngine;
using UnityEngine.UI;

namespace Code.View
{
    public class PlayerNamePanelView : MonoBehaviour
    {
        [SerializeField] private GameObject _panel;
        [SerializeField] private InputField _nameInput;
        [SerializeField] private Button _acceptNameButton;
        [SerializeField] private Button _cancelButton;

        public InputField NameInput => _nameInput;
        public Button AcceptNameButton => _acceptNameButton;

        public Button CancelButton => _cancelButton;

        public void OpenClosePanel(bool value)
        {
            _panel.SetActive(value);
        }
    }
}
