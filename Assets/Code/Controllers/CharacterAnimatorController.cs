using Code.Configs;
using Code.UserInput;
using Photon.Pun;
using UnityEngine;

namespace Code.Controllers
{
    internal class CharacterAnimatorController : MonoBehaviourPun
    {
        private Animator _animator;
        private InputInitialization _input;
        private PlayerConfig _config;
        private static readonly int Speed = Animator.StringToHash("Speed");
        private float _horizontal;
        private float _vertical;

        
        public void Init(InputInitialization input, Animator animator, PlayerConfig config)
        {
            _input = input;
            _animator = animator;
            _config = config;
            _input.InputHorizontal.AxisOnChange += HorizontalOnAxisOnChange;
            _input.InputVertical.AxisOnChange += VerticalOnAxisOnChange;
        }
        
        private void HorizontalOnAxisOnChange(float value) => _horizontal = value;
        private void VerticalOnAxisOnChange(float value) => _vertical = value;
        
        public void Update()
        {
            if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
            {
                return;
            }

            if (!_animator)
            {
                return;
            }

            AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);

            // if (Input.GetButtonDown("Work"))
            // {
            //     _animator.SetTrigger("Work");
            // }

            _animator.SetFloat(Speed, _horizontal * _horizontal * _config.Speed + _vertical * _vertical * _config.Speed);

        }
        
        public void OnDestroy()
        {
            _input.InputHorizontal.AxisOnChange -= HorizontalOnAxisOnChange;
            _input.InputVertical.AxisOnChange -= VerticalOnAxisOnChange;
        }
    }
}
