using Code.View;
using UnityEngine;
using UnityEngine.UI;


namespace Code.ViewHandlers
{
    internal class DialogPanelViewHandler
    {
        private readonly DialogPanelView _dialogPanelView;

        public DialogPanelViewHandler(DialogPanelView dialogPanelView, Canvas canvas)
        {
            _dialogPanelView = Object.Instantiate(dialogPanelView, canvas.transform);
        }

        public Button AcceptButton => _dialogPanelView.AcceptButton;
        public Button BuildButton => _dialogPanelView.BuildButton;

        public void ShowMessage(string message)
        {
            _dialogPanelView.TextUp.text = message;
        }

        public void ActivationPanel(bool flag)
        {
            _dialogPanelView.gameObject.SetActive(flag);
        }
    }
}
