using Code.Configs;
using Code.Factory;
using Code.Interfaces;
using Code.UserInput;
using UnityEngine;

namespace Code.Character
{
    public class CharacterMoveController : IInitialization, IFixedExecute, ICleanup
    {
        public bool IsWalk { get; private set; }
        private readonly CharacterModel _character;
        private readonly Camera _camera;
        private readonly InputInitialization _input;
        private readonly PlayerConfig _config;
        private Vector3 _newVelocity;
        private float _horizontal;
        private float _vertical;

        public CharacterMoveController(CharacterModel characterModel, InputInitialization input, PlayerConfig config,
            Camera camera)
        {
            _input = input;
            _config = config;
            _character = characterModel;
            _camera = camera;
        }

        public void Initialize()
        {
            _input.InputMouseLeft.OnButtonHold += OnMouseButtonHold;
        }

        private void OnMouseButtonHold(bool flag) => IsWalk = flag;

        public void FixedExecute(float deltaTime)
        {
            if (_character.PhotonView.CheckIsMine())
            {
                ProcessInputs();
            }
        }

        private void ProcessInputs()
        {
            if (IsWalk)
            {
                if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out var hit))
                {
                    var direction = (hit.point - _character.Transform.position).normalized;

                    _horizontal = direction.x;
                    _vertical = direction.z;

                    Vector3 relativePos = _newVelocity;
                    var angle = Vector3.Angle(Vector3.forward, relativePos);
                    var axis = Vector3.Cross(Vector3.forward, relativePos);
                    _character.Transform.rotation = Quaternion.AngleAxis(angle, axis);

                    _newVelocity.Set(_horizontal, 0.0f, _vertical);
                    _character.Rigidbody.AddForce(_newVelocity * _config.Speed);
                }
            }
        }

        public void Cleanup()
        {
            _input.InputMouseLeft.OnButtonHold -= OnMouseButtonHold;
        }
    }
}
