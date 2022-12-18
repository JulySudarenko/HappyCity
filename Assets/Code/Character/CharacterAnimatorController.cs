using Code.Controllers;
using Code.Factory;
using Code.Interfaces;
using UnityEngine;

namespace Code.Character
{
    internal class CharacterAnimatorController : IExecute
    {
        private readonly CharacterModel _character;
        private readonly CharacterMoveController _moveController;
        private static readonly int Walk = Animator.StringToHash("Walk");
        private static readonly int Stay = Animator.StringToHash("Stay");


        public CharacterAnimatorController(CharacterModel character, CharacterMoveController moveController)
        {
            _character = character;
            _moveController = moveController;
        }

        public void Execute(float deltaTime)
        {
            if (_character.PhotonView != null)
            {
                if (_character.PhotonView.photonView.IsMine)
                {
                    AnimatorStateInfo stateInfo = _character.Animator.GetCurrentAnimatorStateInfo(0);

                    if (_moveController.IsWalk)
                    {
                        _character.Animator.SetTrigger(Walk);
                    }
                    else
                    {
                        _character.Animator.SetTrigger(Stay);
                    }
                }
            }
        }
    }
}
