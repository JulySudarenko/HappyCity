using System.Collections.Generic;
using Code.Configs;
using Code.Interfaces;
using Code.Quest;
using Code.ResourcesC;
using Code.View;
using Code.ViewHandlers;
using UnityEngine;

namespace Code.Controllers
{
    internal class ViewController : IInitialization, ICleanup
    {
        private readonly ResourcesPanelViewHandler _resourcesPanelViewHandler;
        private readonly List<IQuestState> _questList = new List<IQuestState>();
        private readonly TasksPanelViewHandler _tasksPanelViewHandler;
        private readonly QuestSystemController _questControllers;
        private readonly VolumeViewHandler _volumeViewHandler;


        public ViewController(UnionConfig unionConfig, Transform resourcesPanelView,
            ImageLineElement resourceLineElement, Transform tasksPanelView, LineElementView tasksLineElement,
            ResourcesCheckUnionController resourceUnionController,
            QuestSystemController questControllers, AudioSource audioSource,
            MusicConfig config, AudioSource cameraAudio, ImageLineElement volume)
        {
            _questControllers = questControllers;
            _resourcesPanelViewHandler =
                new ResourcesPanelViewHandler(unionConfig, resourcesPanelView, resourceLineElement,
                    resourceUnionController);

            _tasksPanelViewHandler = new TasksPanelViewHandler(tasksPanelView, tasksLineElement, audioSource, config);
            _volumeViewHandler = new VolumeViewHandler(cameraAudio, audioSource, volume);
        }

        public void Initialize()
        {
            _resourcesPanelViewHandler.Initialize();
            _tasksPanelViewHandler.Initialize();
            _questControllers.QuestAdd += AddNewQuest;
        }

        private void AddNewQuest(IQuestState state)
        {
            _questList.Add(state);
            state.OnQuestStart += _tasksPanelViewHandler.OnTaskAdd;
            state.OnQuestDone += _tasksPanelViewHandler.OnTaskRemove;
        }

        public void Cleanup()
        {
            for (int i = 0; i < _questList.Count; i++)
            {
                _questList[i].OnQuestStart -= _tasksPanelViewHandler.OnTaskAdd;
                _questList[i].OnQuestDone -= _tasksPanelViewHandler.OnTaskRemove;
            }

            _questControllers.QuestAdd -= AddNewQuest;
            _volumeViewHandler.Cleanup();
        }
    }
}
