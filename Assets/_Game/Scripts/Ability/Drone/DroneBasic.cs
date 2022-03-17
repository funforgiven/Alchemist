using System.Collections;
using UnityEngine;

namespace Ability.Drone
{
    public class DroneBasic : AbilityBase
    {
        public DroneBasicSettings Settings { get; private set; }

        private float _cooldown;
        private Transform _spawnTransform;
        private GameObject _projectilePrefab;
        
        public DroneBasic(CharacterStats self, DroneBasicSettings settings) : base(self)
        {
            Settings = settings;
            _projectilePrefab = NodeController.Blackboard.Get<GameObject>(settings.ProjectileName);
            _spawnTransform = NodeController.Blackboard.Get<Transform>(settings.ProjectileSpawnTransform);
        }

        public override IEnumerator OnAbilityUse(params CharacterStats[] targets)
        {
            HasStarted = true;
            HasFinished = false;

            var projectileObject = Object.Instantiate(_projectilePrefab, _spawnTransform.position, _spawnTransform.rotation);
            var projectile = projectileObject.GetComponent<DroneBasicProjectile>();

            projectile.damage = Settings.Damage;
            projectile.speed = Settings.ProjectileSpeed;
            
            yield return new WaitForSeconds(Settings.CastTime);

            _cooldown = Settings.Cooldown;
            Result = true;
            HasFinished = true;
        }

        public override bool CheckConditions()
        {
            var player = NodeController.Blackboard.Get<GameObject>("player_object");
            var origin = _spawnTransform.position;
            var direction = (player.transform.position - _spawnTransform.position).normalized;

            var hasLineOfSight = false;

            if (Physics.Raycast(origin, direction, out var hit, Mathf.Infinity))
                hasLineOfSight = hit.transform.CompareTag("Player");
            
            return _cooldown <= 0 && !HasStarted && hasLineOfSight;
        }

        public override void OnUpdate()
        {
            if (_cooldown > 0)
                _cooldown -= Time.deltaTime;
        }
    }
    
    public struct DroneBasicSettings
    {
        public float CastTime;
        public float Damage;
        public float Cooldown;
        public float ProjectileSpeed;
        public string ProjectileName;
        public string ProjectileSpawnTransform;
    }
}