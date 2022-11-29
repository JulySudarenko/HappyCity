using Code.Assistance;
using Code.Configs;
using Code.Factory;
using Code.Interfaces;
using Code.View;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Code.ViewHandlers
{
    internal class PlayerViewHandler : IExecute, ILateExecute
    {
        private readonly Camera _camera;
        private readonly CharacterView _characterView;
        private readonly Renderer _targetRenderer;
        private readonly Transform _targetTransform;

        private Vector3 _targetPosition;
        private readonly float _characterHeight;


        public PlayerViewHandler(CharacterModel characterModel, PlayerConfig config, Canvas canvas, Camera camera)
        {
            _camera = camera;
            var view = Object.Instantiate(config.PlayerView, canvas.transform);
            //view.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
            _characterView = view.gameObject.GetOrAddComponent<CharacterView>();
            _characterView.Init(canvas);

            _targetTransform = characterModel.Transform;
            _targetRenderer = characterModel.Renderer;
            _characterHeight = config.Height;

            _characterView.SetText(characterModel.PhotonView.photonView.Owner.NickName);
            _characterView.ActivateText(true);
            _characterView.ActivateQuestion(false);
            _characterView.ActivateSlider(false);
        }

        public void Execute(float deltaTime)
        {
            if (_targetTransform == null)
            {
                Object.Destroy(_characterView.gameObject);
                return;
            }
        }

        public void LateExecute(float deltaTime)
        {
            _targetPosition = _targetTransform.position;
            _targetPosition.y += _characterHeight;

            _characterView.SetPosition(_targetPosition, _targetRenderer, _camera);
        }
    }
}
