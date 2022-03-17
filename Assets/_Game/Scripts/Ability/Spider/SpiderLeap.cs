using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Ability.Spider
{
    public class SpiderLeap : AbilityBase
    {
        public SpiderLeapSettings Settings { get; private set; }

        private NavMeshAgent _navMeshAgent;
        private float _cooldown;
        private static readonly int AbilityLeafTrigger = Animator.StringToHash("ability_leap_trigger");
        private static readonly string DamageSphereName = "leap_damage_sphere";

        private DamageSphere _damageSphere;

        public SpiderLeap(CharacterStats self, SpiderLeapSettings settings) : base(self)
        {
            Settings = settings;
            
            _navMeshAgent = Self.GetComponent<NavMeshAgent>();
            _cooldown = 0;
        }

        public override IEnumerator OnAbilityUse(params CharacterStats[] targets)
        {
            HasStarted = true;
            HasFinished = false;

            _damageSphere = NodeController.Blackboard.Get<DamageSphere>(DamageSphereName);
            _damageSphere.damage = Settings.Damage;
            _damageSphere.enabled = true;

            _navMeshAgent.isStopped = false;
            _navMeshAgent.stoppingDistance = 0;
            _navMeshAgent.speed = Settings.LeapSpeed;
            _navMeshAgent.destination = targets[0].transform.position;
            NodeController.animator.SetTrigger(AbilityLeafTrigger);

            yield return new WaitForSeconds(Settings.CastTime);

            _navMeshAgent.speed = NodeController.Blackboard.Get<float>("spider_movement_speed");
            _navMeshAgent.stoppingDistance = NodeController.Blackboard.Get<float>("spider_follow_distance");
            _navMeshAgent.isStopped = true;

            _damageSphere.enabled = false;
            
            _cooldown = Settings.Cooldown;
            Result = true;
            HasFinished = true;
        }

        public override bool CheckConditions()
        {
            var distance = Vector3.Distance(NodeController.Blackboard.Get<Vector3>("player_position"), NodeController.transform.position);
            return _cooldown <= 0 && !HasStarted && distance < Settings.LeapSpeed * Settings.CastTime;
        }

        public override void OnUpdate()
        {
            if (_cooldown > 0)
                _cooldown -= Time.deltaTime;
        }
    }

    public struct SpiderLeapSettings
    {
        public float CastTime;
        public float LeapSpeed;
        public float Damage;
        public float Cooldown;
    }
}