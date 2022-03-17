using Ability.Support;
using UnityEngine;
using Ability;

namespace Alchemist.AI
{
    public class SupportAIController : NodeController
    {
        [Header("Support Settings")]
        [SerializeField] private Transform supportBaseTransform;
        [Header("Audio")]
        [SerializeField] private AudioClip supportExplodeSound;
        [SerializeField] private AudioClip supportKamikazeSound;
        
        private GameObject _player;
        private EnemyStats _self;
        private AudioSource _audioSource;

        private void Awake()
        {
            _player = GameObject.FindWithTag("Player");
            _self = GetComponent<EnemyStats>();
            _audioSource = GetComponent<AudioSource>();

            #region Skills
            
            var supportBasicSettings = new SupportBasicSettings
            {
                HealAmount = 7,
                Cooldown = 2f
            };

            var supportKamikazeSettings = new SupportKamikazeSettings
            {
                Damage = 25,
                DamageRadius = 5f,
                IsOnlySupportLeft = false,
                FollowTime = 3f
            };

            var supportBasic = new SupportBasic(_self, supportBasicSettings);
            var supportKamikaze = new SupportKamikaze(_self, supportKamikazeSettings);
            #endregion

            #region BlackBoard
            
            Blackboard = new Blackboard();
            
            Blackboard.Add("ability_basic", supportBasic);
            Blackboard.Add("ability_kamikaze", supportKamikaze);
            
            Blackboard.Add("enemy_stats", GetComponent<EnemyStats>());
            Blackboard.Add("explosion_sound", supportExplodeSound);
            
            Blackboard.Add("support_movement_speed", 5f);
            Blackboard.Add("support_flee_distance", 5f);
            Blackboard.Add("support_chase_distance", 10f);

            Blackboard.Add("support_flee_destination", transform.position);

            Blackboard.Add("player_object", _player);
            Blackboard.Add("player_position", _player.transform.position);
            Blackboard.Add("player_stat", _player.GetComponent<CharacterStats>());
            #endregion

            #region BasicHeal

            var abilityConditionCheck1 = new AbilityConditionCheckLeaf(this, "ability_basic");
            var abilityCastLeaf1 = new AbilityCastLeaf(this, "ability_basic", "enemy_stats");
            var sequence1 = new Sequence(this, abilityConditionCheck1, abilityCastLeaf1);
            #endregion

            #region Movement
            
            var proximity1 = new ProximityLeaf(this, "support_flee_distance", "player_object");
            var move1 = new MoveLeaf(this, "support_movement_speed", "support_flee_destination");
            var sequence2 = new Sequence(this, proximity1, move1);
            
            var proximity2 = new ProximityLeaf(this, "support_chase_distance", "player_object");
            var inverter1 = new Inverter(this, proximity2);
            var move2 = new MoveLeaf(this, "support_movement_speed", "player_position", 3f);
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
            lookAt.y = supportBaseTransform.position.y;
            supportBaseTransform.LookAt(lookAt);

            var origin = supportBaseTransform.position + new Vector3(0, 1, 0);
            var direction = -supportBaseTransform.up;
            var layerMask = LayerMask.NameToLayer("Ground");
            if (Physics.Raycast(origin, direction, out var hit, Mathf.Infinity, layerMask))
            {
                transform.position = hit.point + new Vector3(0, 3, 0);
            }
        }
        
        private void UpdateBlackboard()
        {
            Blackboard.Update("player_position", Blackboard.Get<GameObject>("player_object").transform.position);

            var transform1 = transform;
            var fleeDestination = (transform1.position - _player.transform.position).normalized + transform1.position;
            Blackboard.Update("support_flee_destination", fleeDestination);
        }
        private void PlayKamikaze()
        {
            _audioSource.PlayOneShot(supportKamikazeSound);
        }
    }
}
