using System.Collections;
using UnityEngine;

namespace Ability.Support
{
    public class SupportBasic : AbilityBase
    {
     
        public SupportBasicSettings Settings { get; private set; }
        
        private float _cooldown;

        private readonly EnemyStats _obj;
        
        public SupportBasic(EnemyStats self, SupportBasicSettings settings) : base(self)
        {
            _obj = self;
            Settings = settings;
        }
        
        public override IEnumerator OnAbilityUse(params CharacterStats[] targets)
        {
            HasStarted = true;
            HasFinished = false;
            
            yield return new WaitForFixedUpdate();
            
            var otherEnemies = _obj.room.enemies;
            foreach (var enemy in otherEnemies)
            {
                Debug.Log(enemy.name);
                enemy.GetComponent<EnemyStats>().ModifyHealthOffset(Settings.HealAmount);
            }
            
            _cooldown = Settings.Cooldown;
        
            Result = true;
            HasFinished = true;
            
            //AudioSource.PlayClipAtPoint(NodeController.Blackboard.Get<AudioClip>("heal_sound"), NodeController.transform.position);
            yield return null;
        }

        public override bool CheckConditions()
        {
            return _cooldown <= 0;
        }
        
        public override void OnUpdate()
        {
            if (_cooldown > 0)
                _cooldown -= Time.deltaTime;
        }
        
    }
    
    public struct SupportBasicSettings
    {
        public float HealAmount;
        public float Cooldown;
    }
}

