using System;
using System.Collections;
using System.Collections.Generic;
using Ability;
using Ability.Drone;
using Alchemist.AI;
using UnityEngine;

namespace Alchemist.AI
{
    public class DroneAIController : NodeController
    {
        [Header("Drone Settings")]
        [SerializeField] private GameObject droneBasicAttackProjectilePrefab;
        [SerializeField] private Transform droneBasicAttackSpawnTransform;
        [SerializeField] private Transform droneBaseTransform;
        [Header("Audio")]
        [SerializeField] private AudioClip droneLaserShootSound;
        [SerializeField] private AudioClip droneExplodeSound;
        [SerializeField] private AudioClip droneKamikazeSound;

        private GameObject _player;
        private CharacterStats _self;
        private AudioSource _audioSource;
    
        private void Awake()
        {
            _player = GameObject.FindWithTag("Player");
            _self = GetComponent<CharacterStats>();
            _audioSource = GetComponent<AudioSource>();
            animator = GetComponent<Animator>();
            
            Blackboard = new Blackboard();
            Blackboard.Add("projectile_basic", droneBasicAttackProjectilePrefab);
            Blackboard.Add("projectile_basic_spawn_transform", droneBasicAttackSpawnTransform);
            
            #region Skills
            var droneBasicSettings = new DroneBasicSettings
            {
                ProjectileName = "projectile_basic",
                ProjectileSpawnTransform = "projectile_basic_spawn_transform",
                ProjectileSpeed = 10f,
                CastTime = 0.25f,
                Damage = 9f,
                Cooldown = 3f
            };
            
            var droneBasic = new DroneBasic(_self, droneBasicSettings);

            var droneKamikazeSettings = new DroneKamikazeSettings()
            {
                Damage = 25,
                DamageRadius = 5f,
                FollowTime = 3f,
                MaxHealthToUse = 10
            };
            
            var droneKamikaze = new DroneKamikaze(_self, droneKamikazeSettings);
            #endregion

            #region Blackboard

            Blackboard.Add("enemy_stats", GetComponent<EnemyStats>());
            Blackboard.Add("explosion_sound", droneExplodeSound);
            
            Blackboard.Add("ability_basic", droneBasic);
            Blackboard.Add("ability_kamikaze", droneKamikaze);

            Blackboard.Add("drone_movement_speed", 5f);
            Blackboard.Add("drone_flee_distance", 7f);
            Blackboard.Add("drone_chase_distance", 10f);

            Blackboard.Add("drone_flee_destination", transform.position);

            Blackboard.Add("player_object", _player);
            Blackboard.Add("player_position", _player.transform.position);
            Blackboard.Add("player_stat", _player.GetComponent<CharacterStats>());
            #endregion

            #region Basic Attack
            
            var abilityConditionCheck1 = new AbilityConditionCheckLeaf(this, "ability_basic");
            var abilityCastLeaf1 = new AbilityCastLeaf(this, "ability_basic", "player_stat");
            var sequence1 = new Sequence(this, abilityConditionCheck1, abilityCastLeaf1);
            
            #endregion

            #region Movement
            
            var proximity1 = new ProximityLeaf(this, "drone_flee_distance", "player_object");
            var move1 = new MoveLeaf(this, "drone_movement_speed", "drone_flee_destination");
            var sequence2 = new Sequence(this, proximity1, move1);
            
            var proximity2 = new ProximityLeaf(this, "drone_chase_distance", "player_object");
            var inverter1 = new Inverter(this, proximity2);
            var move2 = new MoveLeaf(this, "drone_movement_speed", "player_position", 3f);
            var sequence3 = new Sequence(this, inverter1, move2);
            
            var selector1 = new Selector(this, sequence2, sequence3);
            
            var stop1 = new StopLeaf(this);
            #endregion

            #region Kamikaze
            var abilityConditionCheck2 = new AbilityConditionCheckLeaf(this, "ability_kamikaze");
            var abilityCastLeaf2 = new AbilityCastLeaf(this, "ability_kamikaze", "player_stat");
            var sequence4 = new Sequence(this, abilityConditionCheck2, abilityCastLeaf2);
            #endregion
            
            var rootSelector = new Selector(this, sequence1, sequence4, selector1, stop1);
            var rootRepeater = new Repeater(this, rootSelector, true);
            Root = new Root(this, rootRepeater);

            abilityCastLeaf1.NodeTick += PlayLaserShoot;
            abilityCastLeaf2.NodeTick += PlayKamikaze;
            
            // Subscribe events
            rootRepeater.NodeTick += UpdateBlackboard;
        }
        
        private void Start()
        {
            Root.Tick();
        }

        private void Update()
        {
            Blackboard.Get<AbilityBase>("ability_basic").OnUpdate();

            var lookAt = _player.transform.position;
            lookAt.y = droneBaseTransform.position.y;
            droneBaseTransform.LookAt(lookAt);

            var origin = droneBaseTransform.position + new Vector3(0, 1, 0);
            var direction = -droneBaseTransform.up;
            var layerMask = LayerMask.NameToLayer("Ground");
            if (Physics.Raycast(origin, direction, out var hit, Mathf.Infinity, layerMask))
            {
                transform.position = hit.point + new Vector3(0, 3, 0);
            }
        }

        /// <summary>
        /// Updates blackboard information
        /// </summary>
        private void UpdateBlackboard()
        {
            Blackboard.Update("player_position", Blackboard.Get<GameObject>("player_object").transform.position);

            var transform1 = transform;
            var fleeDestination = (transform1.position - _player.transform.position).normalized + transform1.position;
            Blackboard.Update("drone_flee_destination", fleeDestination);
        }

        private void PlayLaserShoot()
        {
            _audioSource.PlayOneShot(droneLaserShootSound);
        }

        private void PlayKamikaze()
        {
            _audioSource.PlayOneShot(droneKamikazeSound);
        }
    }

}
