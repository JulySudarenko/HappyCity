using System.Collections.Generic;
using Code.Interfaces;
using Code.View;
using UnityEngine;

namespace Code.ViewHandlers
{
    internal class TasksPanelViewHandler : IInitialization
    {
        private readonly Dictionary<int, LineElementView> _tasksList = new Dictionary<int, LineElementView>();
        private readonly Transform _tasksPanelView;
        // private readonly Vector2 _openPanelSize = new Vector2(100.0f, 0.0f);
        // private readonly Vector2 _openPanelPosition = new Vector2(-50.0f, -225.0f);
        // private readonly Vector2 _closePanelSize = new Vector2(40.0f, 450.0f);
        // private readonly Vector2 _closePanelPosition = new Vector2(-20.0f, -225.0f);

        private readonly LineElementView _tasksLineElement;
        // private Vector2 _sizeDeltaOpenClose = new Vector2(60.0f, 0.0f);
        private bool _isOpen;

        public TasksPanelViewHandler(Transform tasksPanelView, LineElementView tasksLineElement)
        {
            _tasksPanelView = tasksPanelView;
            _tasksLineElement = tasksLineElement;
            _isOpen = true;
        }

        public void Initialize()
        {
            _tasksPanelView.gameObject.SetActive(true);
        }

        private void OnButtonClick(int id)
        {
            if (_isOpen)
            {
                ClosePanel(id);
                _isOpen = false;
            }
            else
            {
                OpenPanel(id);
                _isOpen = true;
            }
        }

        private void OpenPanel(int id)
        {
            _tasksList[id].TextDown.gameObject.SetActive(true);
        }

        private void ClosePanel(int id)
        {
            _tasksList[id].TextDown.gameObject.SetActive(false);
        }

        public void OnTaskAdd(string header, string info, int id)
        {
            var element = Object.Instantiate(_tasksLineElement, _tasksPanelView);
            element.TextUp.text = header;
            element.TextDown.text = info;
            _tasksList.Add(id, element);
            element.Button.onClick.AddListener(() => OnButtonClick(id));
            element.gameObject.SetActive(true);
        }

        public void OnTaskRemove(int id)
        {
            _tasksList[id].Button.onClick.RemoveAllListeners();
            //_tasksList[id].gameObject.SetActive(false);
            Object.Destroy(_tasksList[id]);
            _tasksList.Remove(id);
        }
    }
}
