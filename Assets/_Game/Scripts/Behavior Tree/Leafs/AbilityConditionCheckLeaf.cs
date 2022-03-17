using Ability;
using UnityEngine;

namespace Alchemist.AI
{
    public class AbilityConditionCheckLeaf : Node
    {
        private string _abilityInformation;

        public AbilityConditionCheckLeaf(NodeController nodeController, string abilityInformation) : base(nodeController)
        {
            _abilityInformation = abilityInformation;
        }

        public override void Initialize()
        {
            State = NodeState.Running;
            HasInitialized = true;
        }

        public override void Tick()
        {
            OnNodeTick();

            var ability = NodeController.Blackboard.Get<AbilityBase>(_abilityInformation);
            State = ability.CheckConditions() ? NodeState.Success : NodeState.Failure;
        }
    }
}