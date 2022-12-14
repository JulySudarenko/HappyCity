using System;
using Code.Quest;
using UnityEngine;
using UnityEngine.UI;


namespace Code.View
{
    public class CharacterView : MonoBehaviour
    {
        [SerializeField] private Vector3 _screenOffset = new Vector3(0f, 30f, 0f);
        [SerializeField] private Text _text;
        [SerializeField] private Slider _slider;
        [SerializeField] private Image _image;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Color _colorStart;
        [SerializeField] private Color _colorBusy;
        [SerializeField] private Color _colorInWork;

        public void Init(Canvas canvas)
        {
            transform.SetParent(canvas.transform, false);
        }

        public void SetText(string name)
        {
            if (_text != null)
            {
                _text.text = name;
            }
        }

        public void SetPosition(Vector3 vector, Renderer targetRenderer, Camera camera)
        {
            if (targetRenderer != null)
            {
                _canvasGroup.alpha = targetRenderer.isVisible ? 1f : 0f;
            }

            transform.position = camera.WorldToScreenPoint(vector) + _screenOffset;
        }

        public void SetSliderAreaValue(float value)
        {
            if (_slider != null)
            {
                _slider.value = value / 100;
            }
        }

        public void SetSliderAreaValue(float maxValue, float value)
        {
            if (_slider != null)
            {
                _slider.value = value / maxValue;
            }
        }

        public void ActivateQuestion(bool value)
        {
            _image.gameObject.SetActive(value);
        }

        public void ChangeQuestionColor(QuestState state)
        {
            switch (state)
            {
                case QuestState.None:
                    _image.color = _colorStart;
                    break;
                case QuestState.Start:
                    _image.color = _colorStart;
                    break;
                case QuestState.Wait:
                    _image.color = _colorInWork;
                    break;
                case QuestState.Check:
                    _image.color = _colorInWork;
                    break;
                case QuestState.Busy:
                    _image.color = _colorBusy;
                    break;
                case QuestState.Done:
                    _image.color = _colorBusy;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        public void ActivateText(bool value)
        {
            _text.gameObject.SetActive(value);
        }

        public void ActivateSlider(bool value)
        {
            _slider.gameObject.SetActive(value);
        }
    }
}
