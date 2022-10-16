using Code.View;
using UnityEngine.UI;

namespace Code.ViewHandlers
{
    internal class MessagePanelViewHandler
    {
        private readonly LineElementView _messagePanelView;

        public MessagePanelViewHandler(LineElementView messagePanelView)
        {
            _messagePanelView = messagePanelView;
        }

        public Button AcceptButton => _messagePanelView.Button; 
        
        public void ShowMessage(string message)
        {
            _messagePanelView.TextUp.text = message;
        }

        public void ChangeTextOnButton(string text)
        {
            _messagePanelView.TextDown.text = text;
        }

        public void ActivationPanel(bool flag)
        {
            _messagePanelView.gameObject.SetActive(flag);
        }
    }
}
