using System.Collections;
using Ability;
using UnityEngine;

namespace Alchemist.AI
{
    public class AbilityCastLeaf : Node
    {
        private string _abilityInformation;
        private string _targetInformation;

        public AbilityCastLeaf(NodeController nodeController, string abilityInformation, string targetInformation = "") : base(nodeController)
        {
            _abilityInformation = abilityInformation;
            _targetInformation = targetInformation;
        }

        public override void Initialize()
        {
            State = NodeState.Running;
            HasInitialized = true;
        }

        public override void Tick()
        {
            var ability = NodeController.Blackboard.Get<AbilityBase>(_abilityInformation);
            var target = NodeController.Blackboard.Get<CharacterStats>(_targetInformation);

            if (!ability.HasStarted)
            {
                OnNodeTick();
                NodeController.StartCoroutine(ability.OnAbilityUse(target));
            }

            if (ability.HasFinished)
            {
                ability.HasStarted = false;
                State = ability.Result ? NodeState.Success : NodeState.Failure;
                HasInitialized = false;
            }
        }
    }
}