using Code.Configs;
using Code.Controllers;
using Code.Interfaces;
using Code.UserInput;
using UnityEngine;


namespace Code.Factory
{
    internal class CharacterInitialization : IInitialization, IExecute, IFixedExecute, ILateExecute, ICleanup
    {
        public CharacterModel CharacterModel { get; }
        private readonly CharacterMoveController _moveController;
        private readonly CharacterAnimatorController _animatorController;
        private readonly CameraController _cameraController;

        public CharacterInitialization(ICharacterFactory playerFactory, InputInitialization input, PlayerConfig config,
            CameraConfig cameraConfig, NetworkSynchronizationController networkSynchronizationController)
        {
            CharacterModel = new CharacterModel(playerFactory, networkSynchronizationController);
            var camera = Camera.main;
            _moveController = new CharacterMoveController(CharacterModel, input, config, camera);
            _animatorController = new CharacterAnimatorController(CharacterModel, _moveController);
            _cameraController = new CameraController(camera.transform, cameraConfig, CharacterModel.Transform);
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

        public void LateExecute(float deltaTime)
        {
            _cameraController.LateExecute(deltaTime);
        }

        public void Cleanup()
        {
            _moveController.Cleanup();
        }
    }
}
