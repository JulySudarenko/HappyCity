using Code.Assistance;
using Code.Configs;
using Code.Hit;
using Code.Interfaces;
using Code.Timer;
using UnityEngine;

namespace Code.Controllers
{
    internal class NpcController : IInitialization, IExecute, IFixedExecute, ICleanup
    {
        private static readonly int Walk = Animator.StringToHash("Walk");
        private static readonly int Stay = Animator.StringToHash("Stay");
        private readonly NpcSpawnHandler _npc;
        private readonly NonPlayerCharacterConfig _config;
        private ITimeRemaining _timeRemaining;
        private Vector3 _target;
        private Vector3 _newVelocity;
        private float _horizontal;
        private float _vertical;
        private bool _hasTarget;

        public NpcController(NonPlayerCharacterConfig npcConfig, NpcSpawnHandler npc)
        {
            _config = npcConfig;
            _npc = npc;
            // _target = new Vector3(-30.0f, 0.0f, -30.0f);
            // OnGetTarget(NpcTransform.position, _target);
        }

        private void OnGetTarget(Vector3 start, Vector3 target)
        {
            _target = target;
            _npc.NpcTransform.position = start;
            _npc.NpcTransform.gameObject.SetActive(true);
            _timeRemaining = new TimeRemaining(WaitAfterActivation, 5.0f);
            _timeRemaining.AddTimeRemaining();
        }

        private void WaitAfterActivation()
        {
            _hasTarget = true;
            _timeRemaining.RemoveTimeRemaining();
        }

        public void Initialize()
        {
        }

        public void Execute(float deltaTime)
        {
            if (_hasTarget)
            {
                _npc.NpcAnimator.SetTrigger(Walk);
            }
            else
            {
                _npc.NpcAnimator.SetTrigger(Stay);
            }
        }

        public void FixedExecute(float deltaTime)
        {
            if (_hasTarget)
            {
                GoToTarget();
            }
        }

        private void GoToTarget()
        {
            var direction = (_target - _npc.NpcTransform.position).normalized;

            _horizontal = direction.x;
            _vertical = direction.z;

            _npc.NpcTransform.LookAt(_target);

            Vector3 relativePos = _newVelocity;
            var angle = Vector3.Angle(Vector3.forward, relativePos);
            var axis = Vector3.Cross(Vector3.forward, relativePos);
            _npc.NpcTransform.rotation = Quaternion.AngleAxis(angle, axis);

            _newVelocity.Set(_horizontal, 0.0f, _vertical);
            _npc.NpcRigidbody.AddForce(_newVelocity * _config.Speed);

            if (Mathf.Abs(_npc.NpcTransform.position.x - _target.x) < 1.0f &&
                Mathf.Abs(_npc.NpcTransform.position.z - _target.z) < 1.0f)
            {
                _timeRemaining = new TimeRemaining(Deactivate, 5.0f);
                _timeRemaining.AddTimeRemaining();
                _hasTarget = false;
            }
        }

        private void Deactivate()
        {
            _npc.NpcTransform.gameObject.SetActive(false);
            _timeRemaining.RemoveTimeRemaining();
        }

        public void Cleanup()
        {
        }
    }

    internal class NpcSpawnHandler
    {
        public Transform NpcTransform { get; }
        public IHit HitHandler { get; }
        public Rigidbody NpcRigidbody { get; }
        public Animator NpcAnimator { get; }
        public int NPCID { get; }

        public NpcSpawnHandler(NonPlayerCharacterConfig config)
        {
            var startPosition = config.SpawnPoints.GetChild(Random.Range(0, config.SpawnPoints.childCount))
                .position;
            NpcTransform = Object.Instantiate(config.Prefab, startPosition, Quaternion.identity);
            NpcRigidbody = NpcTransform.gameObject.GetOrAddComponent<Rigidbody>();
            NpcAnimator = NpcTransform.gameObject.GetOrAddComponent<Animator>();
            HitHandler = NpcTransform.gameObject.GetOrAddComponent<TriggerHandler>();
            NPCID = NpcTransform.gameObject.GetInstanceID();
        }
    }
}
