using System.Collections.Generic;
using Code.Configs;
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
        private readonly AudioSource _audioSource;
        private readonly MusicConfig _musicConfig;
        private bool _isOpen;

        public TasksPanelViewHandler(Transform tasksPanelView, LineElementView tasksLineElement,
            AudioSource audioSource, MusicConfig musicConfig)
        {
            _tasksPanelView = tasksPanelView;
            _tasksLineElement = tasksLineElement;
            _audioSource = audioSource;
            _musicConfig = musicConfig;
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
            _audioSource.clip = _musicConfig.QuestListSound;
            _audioSource.Play();
        }

        private void ClosePanel(int id)
        {
            _tasksList[id].TextDown.gameObject.SetActive(false);
            _audioSource.clip = _musicConfig.QuestListSound;
            _audioSource.Play();
        }

        public void OnTaskAdd(string header, string info, int id)
        {
            var element = Object.Instantiate(_tasksLineElement, _tasksPanelView);
            element.TextUp.text = header;
            element.TextDown.text = info;
            _tasksList.Add(id, element);
            element.Button.onClick.AddListener(() => OnButtonClick(id));
            element.gameObject.SetActive(true);
            _audioSource.clip = _musicConfig.QuestStartSound;
            _audioSource.Play();
        }

        public void OnTaskRemove(int id)
        {
            _tasksList[id].Button.onClick.RemoveAllListeners();
            _tasksList[id].gameObject.SetActive(false);
            Object.Destroy(_tasksList[id].gameObject);
            _tasksList.Remove(id);
            _audioSource.clip = _musicConfig.QuestdoneSound;
            _audioSource.Play();
        }
    }
}
