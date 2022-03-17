using System;

namespace Alchemist.AI
{
    /// <summary>
    /// A sequence will visit each child in order, starting with the first,
    /// and when that succeeds will call the second, and so on down the list
    /// of children. If any child fails it will immediately return failure to
    /// the parent. If the last child in the sequence succeeds, then the sequence
    /// will return success to its parent.
    /// </summary>
    public class Sequence : Composite
    {
        /// <summary>
        /// Base constructor for Sequence
        /// </summary>
        /// <param name="nodeController"></param>
        /// <param name="children"></param>
        public Sequence(NodeController nodeController, params Node[] children) : base(nodeController, children)
        {
        }

        public override void Initialize()
        {
            CurrentChildIndex = 0;
            State = NodeState.Running;
            HasInitialized = true;
        }
        
        public override void Tick()
        {
            OnNodeTick();
            
            var currentChild = Children[CurrentChildIndex];
            
            if(!currentChild.HasInitialized)
                currentChild.Initialize();
            
            currentChild.Tick();
            var childState = currentChild.State;

            switch (childState)
            {
                case NodeState.Running:
                    State = NodeState.Running;
                    break;
                case NodeState.Success:
                    if (CurrentChildIndex != Children.Count - 1)
                        CurrentChildIndex++;
                    else
                    {
                        State = NodeState.Success;
                        HasInitialized = false;
                    }
                    break;
                case NodeState.Failure:
                    State = NodeState.Failure;
                    HasInitialized = false;
                    break;
                default:
                    throw new Exception("There is no such NodeState!");
            }
        }
    }
}

