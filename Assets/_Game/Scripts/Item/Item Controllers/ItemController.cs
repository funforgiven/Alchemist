using UnityEngine;

namespace Item
{
    public class ItemController : MonoBehaviour
    {
        /// <summary>
        /// Gets called whenever an item is used.
        /// </summary>
        public virtual void OnItemUse(Item item, GameObject owner) { }
    }
}