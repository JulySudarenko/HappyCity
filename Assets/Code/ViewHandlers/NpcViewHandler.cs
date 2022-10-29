using System;
using Code.Assistance;
using Code.Configs;
using Code.Interfaces;
using Code.NPC;
using Code.Quest;
using Code.View;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Code.ViewHandlers
{
    internal class NpcViewHandler : IInitialization, IExecute, ILateExecute, ICleanup
    {
        private readonly IQuestState _questState;
        private readonly Camera _camera;
        private readonly CharacterView _characterView;
        private readonly Renderer _targetRenderer;
        private readonly Transform _targetTransform;

        private Vector3 _targetPosition;
        private readonly float _characterHeight;
        private bool _isDialog;


        public NpcViewHandler(NpcSpawnHandler npc, NonPlayerCharacterConfig config, Canvas canvas, IQuestState state, Camera camera)
        {
            _questState = state;
            _camera = camera;
            var view = Object.Instantiate(config.NpcView, canvas.transform);
            _characterView = view.gameObject.GetOrAddComponent<CharacterView>();
            _characterView.Init(canvas);

            _targetTransform = npc.NpcTransform;
            _targetRenderer = npc.Renderer;
            _characterHeight = config.Height;

            _characterView.ActivateText(false);
            _characterView.ActivateQuestion(false);
            _characterView.ActivateSlider(false);
            OnChangeQuestState(QuestState.Start);
        }

        public void Initialize()
        {
            _questState.OnDialog += StartFinishDialog;
            _questState.OnStateChange += OnChangeQuestState;
        }

        public void Execute(float deltaTime)
        {
            if (_targetTransform == null)
            {
                Object.Destroy(_characterView.gameObject);
                return;
            }

            if (_isDialog)
            {
                _characterView.SetSliderAreaValue(50);
            }
        }

        public void LateExecute(float deltaTime)
        {
            _targetPosition = _targetTransform.position;
            _targetPosition.y += _characterHeight;

            _characterView.SetPosition(_targetPosition, _targetRenderer, _camera);
        }


        private void StartFinishDialog(bool value)
        {
            _isDialog = value;
            _characterView.ActivateSlider(value);
        }

        private void OnChangeQuestState(QuestState state)
        {
            switch (state)
            {
                case QuestState.None:
                    _characterView.ActivateQuestion(false);
                    break;
                case QuestState.Start:
                    _characterView.ActivateQuestion(true);
                    break;
                case QuestState.Wait:
                    _characterView.ActivateQuestion(true);
                    break;
                case QuestState.Check:
                    _characterView.ActivateQuestion(true);
                    break;
                case QuestState.Done:
                    _characterView.ActivateQuestion(false);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        public void Cleanup()
        {
            _questState.OnDialog -= StartFinishDialog;
            _questState.OnStateChange -= OnChangeQuestState;
        }
    }
}
