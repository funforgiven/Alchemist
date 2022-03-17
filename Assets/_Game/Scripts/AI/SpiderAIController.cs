using System;
using Ability;
using Ability.Spider;
using UnityEngine;

namespace Alchemist.AI
{
    public class SpiderAIController : NodeController
    {
        [Header("Sounds")] 
        [SerializeField] private AudioClip spiderLeapSound;

        private GameObject _player;
        private CharacterStats _self;
        private AudioSource _audioSource;

        private void Awake()
        {
            _player = GameObject.FindWithTag("Player");
            _self = GetComponent<CharacterStats>();
            _audioSource = GetComponent<AudioSource>();
            animator = GetComponent<Animator>();

            #region Skills
            var spiderBasicSettings = new SpiderBasicSettings()
            {
                CastTime = 0.5f,
                CastDistance = 2f,
                Cooldown = 2f,
                Damage = 5f
            };

            var spiderLeapSettings = new SpiderLeapSettings
            {
                LeapSpeed = 10f,
                CastTime = 1f,
                Damage = 9f,
                Cooldown = 5f
            };
            
            var spiderBasic = new SpiderBasic(_self, spiderBasicSettings);
            var spiderLeap = new SpiderLeap(_self, spiderLeapSettings);
            #endregion
            
            #region Blackboard
            Blackboard = new Blackboard();

            Blackboard.Add("ability_basic", spiderBasic);
            Blackboard.Add("ability_leap", spiderLeap);

            Blackboard.Add("spider_follow_distance", 3f);
            Blackboard.Add("spider_movement_speed", 2f);
            Blackboard.Add("ability_cooldown_basic_attack", 1f);

            Blackboard.Add("player_object", _player);
            Blackboard.Add("player_position", _player.transform.position);
            Blackboard.Add("player_stat", _player.GetComponent<CharacterStats>());

            foreach (var damageSphere in GetComponentsInChildren<DamageSphere>())
                Blackboard.Add(damageSphere.name, damageSphere);
            
            #endregion

            #region Ability Cast - Leap
            var abilityConditionCheckLeaf1 = new AbilityConditionCheckLeaf(this, "ability_leap");
            var abilityCast1 = new AbilityCastLeaf(this, "ability_leap", "player_stat");
            var sequence1 = new Sequence(this, abilityConditionCheckLeaf1, abilityCast1);
            #endregion

            #region Move to player
            var proximity2 = new ProximityLeaf(this, "spider_follow_distance", "player_object");
            var inverter1 = new Inverter(this, proximity2);
            var move1 = new MoveLeaf(this, "spider_movement_speed", "player_position", 2f);
            var sequence2 = new Sequence(this, inverter1, move1);
            #endregion
            
            #region Basic Attack
            var abilityConditionCheckLeaf2 = new AbilityConditionCheckLeaf(this, "ability_basic");
            var abilityCast2 = new AbilityCastLeaf(this, "ability_basic", "player_stat");
            var sequence3 = new Sequence(this, abilityConditionCheckLeaf2, abilityCast2);
            #endregion

            var rootSelector = new Selector(this, sequence1, sequence2, sequence3);
            var rootRepeater = new Repeater(this, rootSelector, true);
            Root = new Root(this, rootRepeater);

            abilityCast1.NodeTick += PlayLeapSound;
            
            // Subscribe events
            rootRepeater.NodeTick += UpdateBlackboard;
        }

        private void Start()
        {
            Root.Tick();
        }

        private void Update()
        {
            Blackboard.Get<AbilityBase>("ability_leap").OnUpdate();
            Blackboard.Get<AbilityBase>("ability_basic").OnUpdate();
        }
        
        /// <summary>
        /// Updates blackboard information
        /// </summary>
        private void UpdateBlackboard()
        {
            Blackboard.Update("player_position", Blackboard.Get<GameObject>("player_object").transform.position);
        }

        private void PlayLeapSound()
        {
            _audioSource.PlayOneShot(spiderLeapSound);
        }
    }
}