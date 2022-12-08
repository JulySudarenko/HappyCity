using System;
using Code.Assistance;
using Code.Configs;
using Code.Interfaces;
using Code.NPC;
using Code.Quest;
using Code.ResourcesC;
using Code.View;
using UnityEngine;
using Object = UnityEngine.Object;


namespace Code.ViewHandlers
{
    internal class NpcViewHandler : IExecute, ILateExecute, ICleanup
    {
        private readonly IQuestState _questState;
        private readonly IKeeper _happiness;
        private readonly Camera _camera;
        private readonly CharacterView _characterView;
        private readonly Renderer _targetRenderer;
        private readonly Transform _targetTransform;

        private Vector3 _targetPosition;
        private readonly float _characterHeight;


        public NpcViewHandler(NpcSpawnHandler npc, NonPlayerCharacterConfig config, Canvas canvas, IQuestState state,
            Camera camera, IKeeper happiness)
        {
            _questState = state;
            _camera = camera;
            _happiness = happiness;
            var view = Object.Instantiate(config.NpcView, canvas.transform);
            _characterView = view.gameObject.GetOrAddComponent<CharacterView>();
            _characterView.Init(canvas);
            _characterView.SetSliderAreaValue(happiness.ResourceCount());

            _targetTransform = npc.NpcTransform;
            _targetRenderer = npc.Renderer;
            _characterHeight = config.Height;

            _characterView.ActivateText(false);
            _characterView.ActivateQuestion(false);
            _characterView.ActivateSlider(false);
            OnChangeQuestState(QuestState.Start);
            _questState.OnDialog += StartFinishDialog;
            _questState.OnStateChange += OnChangeQuestState;
        }

        public void Execute(float deltaTime)
        {
            if (_targetTransform == null)
            {
                Object.Destroy(_characterView.gameObject);
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
                case QuestState.Busy:
                    _characterView.ChangeQuestionColor();
                    break;
                case QuestState.Done:
                    _characterView.ActivateQuestion(false);
                    _characterView.SetSliderAreaValue(_happiness.ResourceCount());
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
