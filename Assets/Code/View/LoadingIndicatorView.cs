using System;
using System.Collections;
using Code.PlayFabLogin;
using TMPro;
using UnityEngine;

namespace Code.View
{
    public class LoadingIndicatorView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _loadingMessage;
        [SerializeField] private GameObject _loadingPanel;
        [SerializeField] private GameObject _loadingImage;

        private Coroutine _isWaiting;

        public void ShowLoadingStatusInformation(ConnectionState state, string feedbackText)
        {
            switch (state)
            {
                case ConnectionState.Default:
                    _loadingMessage.text = feedbackText;
                    _loadingMessage.color = Color.white;
                    break;
                case ConnectionState.Success:
                    _loadingMessage.text = feedbackText;
                    _loadingMessage.color = Color.green;
                    StopIndicator();
                    break;
                case ConnectionState.Fail:
                    _loadingMessage.text = feedbackText;
                    _loadingMessage.color = Color.red;
                    StopIndicator();
                    break;
                case ConnectionState.Waiting:
                    _loadingMessage.text = feedbackText;
                    _loadingMessage.color = Color.yellow;
                    _loadingPanel.SetActive(true);
                    _isWaiting = StartCoroutine(ShowWaitingIndicator());
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        public void UpdateFeedbackText(string feedbackText)
        {
            _loadingMessage.text = $"{_loadingMessage.text}\n{feedbackText}";
        }

        private void StopIndicator()
        {
            StopCoroutine(_isWaiting);
            _loadingPanel.SetActive(false);
        }

        private IEnumerator ShowWaitingIndicator()
        {
            while (true)
            {
                _loadingImage.transform.Rotate(Vector3.forward, 0.5f);
                yield return new WaitForSeconds(0.01f);
            }
        }

        private void OnDestroy()
        {
            if (_isWaiting != null)
                StopIndicator();
        }
    }
}
