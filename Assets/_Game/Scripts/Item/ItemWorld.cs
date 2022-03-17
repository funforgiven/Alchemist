using System;
using System.Collections.Generic;
using System.Linq;
using Interfaces;
using UnityEngine;
    
namespace Item
{
    public class ItemWorld : MonoBehaviour, IInteractable
    {
        public Item Item { get; private set; }
        
        [Header("Item Settings")]
        [SerializeField] private ItemData itemData;
        [SerializeField] private int initialStackAmount;

        private List<ItemController> _itemControllers;
        private GameObject _itemIndicator;

        /// <summary>
        /// Enables the world behaviour of the item.
        /// </summary>
        public void EnableWorldBehaviour(Item item, GameObject itemIndicator)
        {
            Item = item;
            _itemIndicator = itemIndicator;
        }
        
        /// <summary>
        /// Enables equipped behaviour of the item.
        /// </summary>
        public void EnableEquipBehaviour(Item item)
        {
            Item = item;
            
            GetComponentInChildren<MeshFilter>().gameObject.layer = LayerMask.NameToLayer("Equipped Item");
            GetComponentInChildren<Collider>().enabled = false;
            GetComponent<Rigidbody>().isKinematic = true;
            
            _itemControllers = GetComponents<ItemController>().ToList();

            foreach (var itemController in _itemControllers)
                Item.OnItemUse += itemController.OnItemUse;
        }

        private void OnDestroy()
        {
            if(_itemIndicator) Destroy(_itemIndicator.gameObject);
            Item.OnItemUse = null;
        }

        public void Interact(GameObject interactor)
        {
            var interactorInventory = interactor.GetComponent<Inventory>();
            
            if (!interactorInventory) throw new Exception("Can't find inventory!");
            
            interactorInventory.Add(Item);
            Destroy(gameObject);
        }
    }
}