﻿using Code.Assistance;
using Code.Controllers;
using Code.Hit;
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
        public Animator Animator { get; }
        public int CharacterID { get; }
        public int ColliderID { get; }

        public CharacterModel(ICharacterFactory playerFactory,
            NetworkSynchronizationController networkSynchronizationController)
        {
            var character = playerFactory.SpawnCharacter();
            Character = character;
            Transform = character.transform;
            Rigidbody = character.GetOrAddComponent<Rigidbody>();
            Animator = character.GetOrAddComponent<Animator>();
            HitHandler = character.GetOrAddComponent<HitHandler>();
            var collider = character.GetOrAddComponent<Collider>();
            ColliderID = collider.GetInstanceID();
            CharacterID = character.GetInstanceID();
            PhotonView = character.GetOrAddComponent<CharacterPhotonView>();
            PhotonView.Init(networkSynchronizationController, this);

            Debug.Log($"character ID {CharacterID}");
            Debug.Log($"collider ID {ColliderID}");
        }
    }
}
