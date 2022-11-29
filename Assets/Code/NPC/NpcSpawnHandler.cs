using Code.Assistance;
using Code.Configs;
using Code.Hit;
using UnityEngine;

namespace Code.NPC
{
    internal class NpcSpawnHandler
    {
        public Transform NpcTransform { get; }
        public IHit HitHandler { get; }
        public Rigidbody NpcRigidbody { get; }
        public Animator NpcAnimator { get; }
        public Renderer Renderer { get; }
        public int NpcId { get; }

        public bool IsTalking { get; private set; }

        public NpcSpawnHandler(NonPlayerCharacterConfig config, Vector3 startPosition)
        {
            NpcTransform = Object.Instantiate(config.Prefab, startPosition, Quaternion.identity);
            NpcRigidbody = NpcTransform.gameObject.GetOrAddComponent<Rigidbody>();
            NpcAnimator = NpcTransform.gameObject.GetOrAddComponent<Animator>();
            HitHandler = NpcTransform.gameObject.GetOrAddComponent<TriggerHandler>();
            Renderer = NpcTransform.GetComponent<Renderer>();
            NpcId = NpcTransform.gameObject.GetInstanceID();
            IsTalking = false;
        }

        public void OnDialog(bool value)
        {
            IsTalking = value;
        }
    }
}
