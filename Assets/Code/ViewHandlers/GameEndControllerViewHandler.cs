using System.Collections.Generic;
using Code.Configs;
using Code.Controllers;
using Code.Network;
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
        private readonly CameraController _cameraController;
        private readonly AudioSource _audioSource;
        private readonly MusicConfig _musicConfig;
        private readonly PhotonConnectionController _photonConnectionController;
        private readonly string _winMessage = "YOU WIN!";
        private readonly string _loseMessage = "YOU LOSE...";
        private readonly float _gameSessionTime = 500.0f;
        private readonly float _deltaTime = 5.0f;

        private ITimeRemaining _gameTimer;
        private ITimeRemaining _gameTimerProcess;
        private float _gameTimeLeft;

        public GameEndControllerViewHandler(GameEndController gameEndController, LineElementView gameEndView,
            CharacterView gameTimeScale, CameraController cameraController, MusicConfig musicConfig,
            AudioSource audioSource, PhotonConnectionController photonConnectionController)
        {
            _gameEndController = gameEndController;
            _gameEndView = gameEndView;
            _timeSlider = gameTimeScale;
            _cameraController = cameraController;
            _musicConfig = musicConfig;
            _audioSource = audioSource;
            _photonConnectionController = photonConnectionController;
            _gameTimeLeft = _gameSessionTime;

            _gameEndController.EndGame += ShowWinScreen;
            _gameEndController.StartGame += OnStartGame;
            _gameEndView.Button.onClick.AddListener(RestartLevel);
        }

        private void OnStartGame()
        {
            _gameTimer = new TimeRemaining(ShowLoseScreen, _gameSessionTime);
            _gameTimerProcess = new TimeRemaining(UpdateTimeLine, _deltaTime, true);
            _gameTimer.AddTimeRemaining();
            _gameTimerProcess.AddTimeRemaining();
            _timeSlider.SetSliderAreaValue(_gameSessionTime, _gameTimeLeft);
        }

        private void UpdateTimeLine()
        {
            _gameTimeLeft -= _deltaTime;
            _timeSlider.SetSliderAreaValue(_gameSessionTime, _gameTimeLeft);
        }

        private void ShowLoseScreen()
        {
            _cameraController.ChangeTarget(5000.0f);
            _cameraController.AudioSource.clip = _musicConfig.LoseGameSound;
            _cameraController.AudioSource.volume = 0.25f;
            _cameraController.AudioSource.Play();
            _gameEndView.TextUp.text = _loseMessage;
            _gameEndView.gameObject.SetActive(true);
            _gameEndController.EndGame -= ShowWinScreen;
            _gameTimer.RemoveTimeRemaining();
            _gameTimerProcess.RemoveTimeRemaining();
            Time.timeScale = 0.0f;
        }

        private void ShowWinScreen(Dictionary<string, int> scoreTable, string winner)
        {
            _cameraController.ChangeTarget(5000.0f);
            _cameraController.AudioSource.clip = _musicConfig.WinGameSound;
            _cameraController.AudioSource.Play();
            _audioSource.clip = _musicConfig.GETRewardSound;
            _cameraController.AudioSource.Play();
            _gameEndView.gameObject.SetActive(true);

            _gameEndView.TextUp.text = $"{_winMessage} \nThe most successful builder is {winner}";
            foreach (var line in scoreTable)
            {
                _gameEndView.TextDown.text += $"{line.Key}     {line.Value}\n";
            }

            _gameTimer.RemoveTimeRemaining();
            _gameTimerProcess.RemoveTimeRemaining();
            Time.timeScale = 0.0f;
        }

        private void RestartLevel()
        {
            _audioSource.clip = _musicConfig.ButtonsSound;
            _audioSource.Play();
            _gameEndController.EndGame -= ShowWinScreen;
            _gameEndView.Button.onClick.RemoveListener(RestartLevel);
            _photonConnectionController.LeaveRoom();
        }
    }
}
