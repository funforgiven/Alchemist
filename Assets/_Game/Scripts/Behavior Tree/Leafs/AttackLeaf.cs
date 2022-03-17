using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Alchemist.AI
{
    public class AttackLeaf : Node
    {
        private string _playerStatInformation;
        private string _damageInformation;
        public AttackLeaf(NodeController nodeController, string playerStatInformation, string damageInformation) : base(nodeController)
        {
            _playerStatInformation = playerStatInformation;
            _damageInformation =  damageInformation;
        }
    
        public override void Initialize()
        {
            State = NodeState.Running;
            HasInitialized = true;
        }
        
        public override void Tick()
        {
            OnNodeTick();
            
            var playerStats = NodeController.Blackboard.Get<CharacterStats>(_playerStatInformation);
            var damage = NodeController.Blackboard.Get<int>(_damageInformation);
            
            playerStats.ModifyHealthOffset(damage);
            State = NodeState.Success;
        }
    }
}
