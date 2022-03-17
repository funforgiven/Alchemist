using System;
using UnityEngine;

namespace Item
{
    public class DebugController : ItemController
    {
        [SerializeField] private string message;
        
        public override void OnItemUse(Item item, GameObject owner)
        {
            Debug.Log(message);
        }
    }
}