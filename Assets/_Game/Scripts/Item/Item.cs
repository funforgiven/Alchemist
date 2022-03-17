using System;
using UnityEngine;
using UnityEngine.Events;

namespace Item
{
    public class Item
    {
        public string ID => _id;
        public string Name => _name;
        public string Description => _description;
        public GameObject Prefab => _prefab;
        public bool Stackable => _stackable;
        public int StackAmount => _stackAmount;
        public int StackLimit => _stackLimit;

        private string _id;
        private string _name;
        private string _description;
        private GameObject _prefab;
        private bool _stackable;
        private int _stackAmount;
        private int _stackLimit;

        public Action<Item, GameObject> OnItemUse;

        public void ModifyStackAmount(int amount)
        {
            _stackAmount += amount;
        }

        public Item(ItemData data, int stackAmount)
        {
            _id = data.itemId;
            _name = data.itemName;
            _description = data.itemDescription;
            _prefab = data.itemPrefab;
            _stackable = data.itemStackable;
            _stackAmount = stackAmount;
            _stackLimit = data.itemStackLimit;
        }

        public void InvokeItemUse(GameObject owner)
        {
            OnItemUse.Invoke(this, owner);
        }
    }
}
