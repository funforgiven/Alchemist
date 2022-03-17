using System;
using System.Collections.Generic;
using UnityEngine;

namespace Item
{
    public class Inventory : MonoBehaviour
    {
        [Header("Inventory Settings")]
        [SerializeField] private List<Item> inventory;
        [SerializeField] private InventoryUI inventoryUI;

        private void Awake()
        {
            inventory = new List<Item>();
        }

        /// <summary>
        /// Adds given item to the inventory
        /// </summary>
        /// <param name="item">Item to add</param>
        public void Add(Item item)
        {
            var itemInInventory = Get(item);
            
            if (!item.Stackable || itemInInventory is null)
            {
                inventory.Add(item);
                if(inventoryUI) inventoryUI.CreateUI(item);
            }
            else
            {
                itemInInventory.ModifyStackAmount(item.StackAmount);
                inventoryUI.UpdateUI();
            }
            
        }

        /// <summary>
        /// Remove the item from inventory
        /// </summary>
        /// <param name="item">Item to remove</param>
        /// <param name="amount">Amount to remove (default removes all)</param>
        public Item Remove(Item item, int amount = 0)
        {
            var itemInInventory = Get(item);

            if (itemInInventory != null)
            {
                if(amount == 0) inventory.Remove(item);
                else inventory[GetIndex(item)].ModifyStackAmount(-amount);
            }
            
            return itemInInventory;
        }

        /// <summary>
        /// Remove the item at given index
        /// </summary>
        /// <param name="index">Index to remove</param>
        /// <returns>Removed item</returns>
        public Item Remove(int index)
        {
            if(inventory.Count < index - 1) throw new IndexOutOfRangeException();
            
            var itemToRemove = inventory[index];
            inventory.RemoveAt(index);

            return itemToRemove;
        }

        /// <summary>
        /// Drop the item from the inventory
        /// </summary>
        /// <param name="item">Item to drop</param>
        public void Drop(Item item)
        {
            // TODO: Complete function
        }

        /// <summary>
        /// Drop the item from the inventory at given index
        /// </summary>
        /// <param name="index">Index to drop</param>
        public void Drop(int index)
        {
            var removedItem = Remove(index);

            if (removedItem != null)
                Instantiate(removedItem.Prefab, transform.position, Quaternion.identity);
        }

        /// <summary>
        /// Finds an item from inventory and returns it
        /// </summary>
        /// <param name="itemName">Item name to search</param>
        /// <returns>Found item</returns>
        public Item Get(string itemName)
        {
            return inventory.Find(item => item.Name == itemName);
        }
        
        /// <summary>
        /// Returns the index of the item.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int GetIndex(Item item)
        {
            return inventory.IndexOf(item);
        }

        /// <summary>
        /// Finds an item from inventory and returns it
        /// </summary>
        /// <param name="searchItem">Item we are looking for</param>
        /// <returns>Found item</returns>
        public Item Get(Item searchItem)
        {
            return inventory.Find(item => item.ID == searchItem.ID);
        }
    }
}
