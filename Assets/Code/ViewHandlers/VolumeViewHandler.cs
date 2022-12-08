using Code.Interfaces;
using Code.View;
using UnityEngine;
using UnityEngine.UI;

namespace Code.ViewHandlers
{
    internal class VolumeViewHandler : ICleanup
    {
        private readonly AudioSource _cameraAudio;
        private readonly AudioSource _characterAudio;
        private readonly Button _volumeButton;
        private readonly Image _volumeImage;
        private bool _isVolume;
        
        public VolumeViewHandler(AudioSource cameraAudio, AudioSource characterAudio, ImageLineElement view)
        {
            _cameraAudio = cameraAudio;
            _characterAudio = characterAudio;
            _volumeImage = view.Icon;
            _volumeButton = view.GetComponentInParent<Button>();
            _isVolume = true;
            _volumeButton.onClick.AddListener(ActivateVolume);
        }

        private void ActivateVolume()
        {
            if (_isVolume)
            {
                _isVolume = false;
                _volumeImage.gameObject.SetActive(false);
                _cameraAudio.volume = 0.0f;
                _characterAudio.volume = 0.0f;
            }
            else
            {
                _isVolume = true;
                _volumeImage.gameObject.SetActive(true);
                _cameraAudio.volume = 0.1f;
                _characterAudio.volume = 0.2f;
            }
        }
        
        public void Cleanup()
        {
            _volumeButton.onClick.RemoveListener(ActivateVolume);
        }
    }
}
