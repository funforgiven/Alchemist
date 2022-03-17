using System;
using UnityEngine;

namespace Item
{
    [CreateAssetMenu(fileName = "New Item Data", menuName = "Item/Create Item Data", order = 0)]
    public class ItemData : ScriptableObject
    {
        [Header("General")]
        public string itemId;
        public string itemName;
        public string itemDescription;
        public GameObject itemPrefab;

        [Header("Stack Settings")]
        public bool itemStackable;
        public int itemStackLimit;

        private void OnEnable()
        {
            itemId = Guid.NewGuid().ToString();
        }
    }
}