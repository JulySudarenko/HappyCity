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
        private readonly LineElementView _tasksLineElement;
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
            _tasksList[id].gameObject.SetActive(false);
            Object.Destroy(_tasksList[id].gameObject);
            _tasksList.Remove(id);
        }
    }
}
