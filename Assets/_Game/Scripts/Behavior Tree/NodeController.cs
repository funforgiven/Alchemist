using System;
using System.Collections;
using UnityEngine;

namespace Alchemist.AI
{
    /// <summary>
    /// Simple interface that connects a node tree to a GameObject.
    /// </summary>
    public class NodeController : MonoBehaviour
    {
        /// <summary>
        /// Root of the Behaviour Tree
        /// </summary>
        protected Root Root;
        
        /// <summary>
        /// How many update calls per second
        /// </summary>
        [Range(1f, 120f)]
        public float ticksPerSecond = 1f;

        /// <summary>
        /// Whether should we pause the ticks for this node controller.
        /// </summary>
        public bool hasPaused = true;

        /// <summary>
        /// The animator of this node controller
        /// </summary>
        public Animator animator;

        /// <summary>
        /// Blackboards are used to store information for Behaviour Tree.
        /// </summary>
        public Blackboard Blackboard { get; protected set; }
    }
}

