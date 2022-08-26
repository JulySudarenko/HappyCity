using Code.Assistance;
using Code.Configs;
using Code.Controllers;
using Code.UserInput;
using UnityEngine;


namespace Code.Factory
{
    internal class CharacterInitialization
    {
        public Transform Transform { get; }
        public Rigidbody Rigidbody { get; }
        public HitHandler HitHandler { get; }
        
        public CharacterInitialization(ICharacterFactory playerFactory, InputInitialization input, PlayerConfig config)
        {
            var character = playerFactory.SpawnCharacter();
            Transform = character.transform;
            Rigidbody = character.GetOrAddComponent<Rigidbody>();
            Rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
            HitHandler = character.GetOrAddComponent<HitHandler>();

            InitMoveController(character, input, config);
            InitAnimatorController(character, input, config);
        }

        private void InitMoveController(GameObject character, InputInitialization input, PlayerConfig config)
        {
            var characterMoveController = character.GetOrAddComponent<CharacterMoveController>();
            characterMoveController.Init(input, config, Rigidbody);
        }

        private void InitAnimatorController(GameObject character, InputInitialization input, PlayerConfig config)
        {
            var animator = character.GetOrAddComponent<Animator>();
            var animatorController = character.GetOrAddComponent<CharacterAnimatorController>();
            animatorController.Init(input, animator, config);
        }
    }
}
