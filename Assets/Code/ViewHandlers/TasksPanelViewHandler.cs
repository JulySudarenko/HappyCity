using System.Collections.Generic;
using Code.Interfaces;
using Code.View;
using UnityEngine;

namespace Code.ViewHandlers
{
    internal class TasksPanelViewHandler : IInitialization, ICleanup
    {
        private readonly List<int> _tasksList = new List<int>();
        private readonly Transform _tasksPanelView;
        private readonly RectTransform _rectTransformTasksPanel;
        private readonly Vector2 _openPanelSize = new Vector2(100.0f, 450.0f);
        private readonly Vector2 _openPanelPosition = new Vector2(-50.0f, 0.0f);
        private readonly Vector2 _closePanelSize = new Vector2(40.0f, 450.0f);
        private readonly Vector2 _closePanelPosition = new Vector2(-20.0f, 0.0f);
        
        private LineElementView _tasksLineElement;
        private Vector2 _sizeDeltaOpenClose = new Vector2(60.0f, 0.0f);
        
        public TasksPanelViewHandler(Transform tasksPanelView, LineElementView tasksLineElement)
        {
            _tasksPanelView = tasksPanelView;
            _tasksLineElement = tasksLineElement;
            _rectTransformTasksPanel = _tasksPanelView.GetComponent<RectTransform>();
        }

        public void Initialize()
        {
            _tasksPanelView.gameObject.SetActive(true);
        }

        private void OnPanelSelected()
        {
            _rectTransformTasksPanel.sizeDelta = _openPanelSize;
            _rectTransformTasksPanel.localPosition = _openPanelPosition;
        }

        private void OnPanelDeselected()
        {
            _rectTransformTasksPanel.sizeDelta = _closePanelSize;
            _rectTransformTasksPanel.localPosition = _closePanelPosition;
        }

        private void OnTaskAdd()
        {
        }

        private void OnTaskRemove()
        {
        }

        public void Cleanup()
        {
        }
    }
}
