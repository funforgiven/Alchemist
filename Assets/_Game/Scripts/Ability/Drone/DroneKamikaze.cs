using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Ability.Drone
{
    public class DroneKamikaze : AbilityBase
    {
        public DroneKamikazeSettings Settings { get; private set; }
        
        public DroneKamikaze(CharacterStats self, DroneKamikazeSettings settings) : base(self)
        {
            Settings = settings;
        }

        public override IEnumerator OnAbilityUse(params CharacterStats[] targets)
        {
            HasStarted = true;
            HasFinished = false;
            
            NodeController.hasPaused = true;

            var agent = NodeController.GetComponent<NavMeshAgent>();
            var player = targets[0];

            var timeLeft = Settings.FollowTime;

            while (timeLeft > 0)
            {
                agent.SetDestination(player.transform.position);

                timeLeft -= Time.deltaTime;
                yield return new WaitForSeconds(Time.deltaTime);
            }

            var nearbyObjects = Physics.OverlapSphere(NodeController.transform.position, Settings.DamageRadius);
            foreach (var nearbyObject in nearbyObjects)
            {
                var isPlayer = nearbyObject.GetComponent<PlayerStats>();
                if(isPlayer)
                    isPlayer.ModifyHealthOffset(-Settings.Damage);
            }

            AudioSource.PlayClipAtPoint(NodeController.Blackboard.Get<AudioClip>("explosion_sound"), NodeController.transform.position);
            NodeController.Blackboard.Get<EnemyStats>("enemy_stats").HandleDeath();
            Object.Destroy(NodeController.gameObject);
            yield return null;
        }

        public override bool CheckConditions()
        {
            return Self.Health <= Settings.MaxHealthToUse;
        }
    }
    
    public struct DroneKamikazeSettings
    {
        public float Damage;
        public float DamageRadius;
        public float MaxHealthToUse;
        public float FollowTime;
    }
}