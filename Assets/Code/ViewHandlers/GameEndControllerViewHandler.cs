using System.Collections.Generic;
using Code.Controllers;
using Code.Timer;
using Code.View;
using Photon.Pun;
using UnityEngine;


namespace Code.ViewHandlers
{
    internal class GameEndControllerViewHandler
    {
        private readonly GameEndController _gameEndController;
        private readonly LineElementView _gameEndView;
        private readonly CharacterView _timeSlider;
        private readonly string _winMessage = "YOU WIN!";
        private readonly string _loseMessage = "YOU LOSE...";
        private readonly float _gameSessionTime = 500.0f;
        private readonly float _deltaTime = 5.0f;

        private ITimeRemaining _gametimer;
        private ITimeRemaining _gametimerProcess;
        private float _gameTimeLeft;

        public GameEndControllerViewHandler(GameEndController gameEndController, LineElementView gameEndView,
            CharacterView gameTimeScale)
        {
            _gameEndController = gameEndController;
            _gameEndView = gameEndView;
            _timeSlider = gameTimeScale;
            _gameTimeLeft = _gameSessionTime;

            _gameEndController.EndGame += ShowWinScreen;
            _gameEndController.StartGame += OnStartGame;
            _gameEndView.Button.onClick.AddListener(RestartLevel);
        }

        private void OnStartGame()
        {
            _gametimer = new TimeRemaining(ShowLoseScreen, _gameSessionTime);
            _gametimerProcess = new TimeRemaining(UpdateTimeLine, _deltaTime, true);
            _gametimer.AddTimeRemaining();
            _gametimerProcess.AddTimeRemaining();
            _timeSlider.SetSliderAreaValue(_gameSessionTime, _gameTimeLeft);
        }

        private void UpdateTimeLine()
        {
            _gameTimeLeft -= _deltaTime;
            _timeSlider.SetSliderAreaValue(_gameSessionTime, _gameTimeLeft);
        }

        private void ShowLoseScreen()
        {
            Time.timeScale = 0.0f;
            _gameEndView.TextUp.text = _loseMessage;
            _gameEndView.gameObject.SetActive(true);
            _gameEndController.EndGame -= ShowWinScreen;
            _gametimer.RemoveTimeRemaining();
            _gametimerProcess.RemoveTimeRemaining();
        }

        private void ShowWinScreen(Dictionary<string, int> _scoreTable, string winner)
        {
            _gameEndView.gameObject.SetActive(true);

            _gameEndView.TextUp.text = $"{_winMessage} \nThe most successful builder is {winner}";
            foreach (var line in _scoreTable)
            {
                _gameEndView.TextDown.text += $"{line.Key}     {line.Value}\n";
            }

            _gametimer.RemoveTimeRemaining();
            _gametimerProcess.RemoveTimeRemaining();
        }

        private void RestartLevel()
        {
            _gameEndController.EndGame -= ShowWinScreen;
            _gameEndView.Button.onClick.RemoveListener(RestartLevel);
            PhotonNetwork.LoadLevel(1);
            //SceneManager.LoadScene(1);
        }
    }
}
