using Code.Assistance;
using Code.Hit;
using Code.Network;
using UnityEngine;

namespace Code.Factory
{
    public class CharacterModel
    {
        public GameObject Character { get; }
        public CharacterPhotonView PhotonView { get; }
        public Transform Transform { get; }
        public Rigidbody Rigidbody { get; }
        public HitHandler HitHandler { get; }
        public TriggerHandler FeetCollider { get; }
        public Animator Animator { get; }
        public Renderer Renderer { get; }
        public AudioSource AudioSource { get; }
        public int CharacterID { get; }
        public int ColliderID { get; }

        public CharacterModel(ICharacterFactory playerFactory,
            NetworkSynchronizationController networkSynchronizationController, Transform feetPrefab)
        {
            var character = playerFactory.SpawnCharacter();
            Character = character;
            Transform = character.transform;
            Rigidbody = character.GetOrAddComponent<Rigidbody>();
            Animator = character.GetOrAddComponent<Animator>();
            Renderer = character.GetComponent<Renderer>();
            HitHandler = character.GetOrAddComponent<HitHandler>();
            var collider = character.GetOrAddComponent<Collider>();
            ColliderID = collider.GetInstanceID();
            CharacterID = character.GetInstanceID();
            PhotonView = character.GetOrAddComponent<CharacterPhotonView>();
            PhotonView.Init(networkSynchronizationController, this);
            AudioSource = character.GetOrAddComponent<AudioSource>();
            AudioSource.volume = 0.2f;
            var feet = Object.Instantiate(feetPrefab, Transform);
            FeetCollider = feet.gameObject.GetOrAddComponent<TriggerHandler>();
        }
    }
}
