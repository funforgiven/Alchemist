using System;
using System.Collections;
using UnityEngine;

namespace Alchemist.AI
{
    /// <summary>
    /// Repeats child node n times.
    /// </summary>
    public class Repeater : Decorator
    {
        private int _repeatAmount;
        private bool _isInfinite;
        private int _currentRepeat;
        
        /// <summary>
        /// Base constructor for Repeater.
        /// </summary>
        /// <param name="nodeController">Owner node controller</param>
        /// <param name="child">Child of this Repeater node</param>
        /// <param name="isInfinite">Whether is this repeater runs indefinitely or not</param>
        /// <param name="repeatAmount">Times to repeat</param>
        public Repeater(NodeController nodeController, Node child, bool isInfinite, int repeatAmount = 0) : base(nodeController, child)
        {
            _isInfinite = isInfinite;
            _repeatAmount = repeatAmount;
            _currentRepeat = 0;
        }

        public override void Initialize()
        {
            _currentRepeat = 0;
            State = NodeState.Running;
            HasInitialized = true;
        }

        public override void Tick()
        {
            NodeController.StartCoroutine(Repeat());
        }

        public IEnumerator Repeat()
        {
            while (NodeController.hasPaused)
                yield return null;

            if (!_isInfinite && _currentRepeat >= _repeatAmount)
            {
                HasInitialized = false;
                yield return null;
            }
            
            OnNodeTick();
            
            if(!Child.HasInitialized)
                Child.Initialize();

            Child.Tick();
            var childState = Child.State;
            switch (childState)
            {
                case NodeState.Running:
                    State = NodeState.Running;
                    break;
                case NodeState.Success:
                    if (!_isInfinite && _currentRepeat >= _repeatAmount)
                    {
                        State = NodeState.Success;
                        HasInitialized = false;
                    }
                    break;
                case NodeState.Failure:
                    if (!_isInfinite && _currentRepeat >= _repeatAmount)
                    {
                        State = NodeState.Failure;
                        HasInitialized = false;
                    }
                    break;
                default:
                    throw new Exception("There is no such NodeState!");
            }

            _currentRepeat++;
            
            yield return new WaitForSeconds(1 / NodeController.ticksPerSecond);
            NodeController.StartCoroutine(Repeat());
        }
    }
}

