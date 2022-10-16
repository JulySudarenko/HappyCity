using Code.Configs;
using Code.Factory;
using Code.Interfaces;
using Code.UserInput;
using UnityEngine;

namespace Code.Controllers
{
    internal class PlayerController : IInitialization, IExecute, IFixedExecute, ICleanup
    {
        private readonly CharacterMoveController _moveController;
        private readonly CharacterAnimatorController _animatorController;

        public PlayerController(InputInitialization input, PlayerConfig config, CharacterModel characterModel,
            Camera camera)
        {
            _moveController = new CharacterMoveController(characterModel, input, config, camera);
            _animatorController = new CharacterAnimatorController(characterModel, _moveController);
        }

        public void Initialize()
        {
            _moveController.Initialize();
        }

        public void FixedExecute(float deltaTime)
        {
            _moveController.FixedExecute(deltaTime);
        }

        public void Execute(float deltaTime)
        {
            _animatorController.Execute(deltaTime);
        }

        public void Cleanup()
        {
            _moveController.Cleanup();
        }
    }
}
