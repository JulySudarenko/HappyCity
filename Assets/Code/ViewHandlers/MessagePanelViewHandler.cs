using Code.View;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;


namespace Code.ViewHandlers
{
    internal class MessagePanelViewHandler
    {
        private readonly LineElementView _messagePanelView;

        public MessagePanelViewHandler(LineElementView messagePanelView, Canvas canvas)
        {
            _messagePanelView = Object.Instantiate(messagePanelView, canvas.transform);
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
