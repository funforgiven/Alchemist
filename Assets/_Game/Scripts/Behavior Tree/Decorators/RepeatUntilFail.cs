using System;

namespace Alchemist.AI
{
    /// <summary>
    /// Repeats the child node until it fails.
    /// </summary>
    public class RepeatUntilFail : Decorator
    {
        /// <summary>
        /// Base constructor for RepeatUntilFail
        /// </summary>
        /// <param name="nodeController">Owner node controller</param>
        /// <param name="child">Child of this RepeatUntilFail node</param>
        public RepeatUntilFail(NodeController nodeController, Node child) : base(nodeController, child)
        {
        }

        public override void Initialize()
        {
            State = NodeState.Running;
            HasInitialized = true;
        }

        public override void Tick()
        {
            OnNodeTick();
            
            if(!Child.HasInitialized)
                Child.Initialize();

            Child.Tick();
            var childState = Child.State;
            switch (childState)
            {
                case NodeState.Running:
                case NodeState.Success:
                    State = NodeState.Running;
                    break;
                case NodeState.Failure:
                    State = NodeState.Success;
                    HasInitialized = false;
                    break;
                default:
                    throw new Exception("There is no such NodeState!");
            }
        }
    }
}

