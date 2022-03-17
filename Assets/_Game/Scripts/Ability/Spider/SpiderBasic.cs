using System.Collections;
using UnityEngine;

namespace Ability.Spider
{
    public class SpiderBasic : AbilityBase
    {
        public SpiderBasicSettings Settings { get; private set; }

        private float _cooldown;
        private static readonly string DamageSphereName = "basic_damage_sphere";

        private DamageSphere _damageSphere;

        public SpiderBasic(CharacterStats self, SpiderBasicSettings settings) : base(self)
        {
            Settings = settings;
            _cooldown = 0;
        }

        public override IEnumerator OnAbilityUse(params CharacterStats[] targets)
        {
            HasStarted = true;
            HasFinished = false;

            _damageSphere = NodeController.Blackboard.Get<DamageSphere>(DamageSphereName);
            _damageSphere.damage = Settings.Damage;
            _damageSphere.enabled = true;

            yield return new WaitForSeconds(Settings.CastTime);

            _damageSphere.enabled = false;
            _cooldown = Settings.Cooldown;
            Result = true;
            HasFinished = true;
        }

        public override bool CheckConditions()
        {
            var distance = Vector3.Distance(NodeController.Blackboard.Get<Vector3>("player_position"), NodeController.transform.position);
            return _cooldown <= 0 && !HasStarted && distance < Settings.CastDistance;
        }

        public override void OnUpdate()
        {
            if (_cooldown > 0)
                _cooldown -= Time.deltaTime;
        }
    }

    public struct SpiderBasicSettings
    {
        public float CastTime;
        public float CastDistance;
        public float Damage;
        public float Cooldown;
    }
}