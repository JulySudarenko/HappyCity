using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Code.View
{
    public sealed class DialogPanelView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _textUp;
        [SerializeField] private Button _acceptButton;
        [SerializeField] private Button _buildButton;
        
        public TMP_Text TextUp => _textUp;
        public Button AcceptButton => _acceptButton;
        public Button BuildButton => _buildButton;
    }
}
