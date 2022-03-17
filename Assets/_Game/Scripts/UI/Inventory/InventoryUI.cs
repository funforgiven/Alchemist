using System.Collections.Generic;
using UnityEngine;

namespace Item
{
    public class InventoryUI : MonoBehaviour
    {
        [Header("Inventory UI Settings")]
        [SerializeField] private Transform itemListView;
        [SerializeField] private GameObject itemUIPrefab;
        
        [Header("References")]
        [SerializeField] private Inventory inventory;
        [SerializeField] private Equipment equipment;

        private List<ItemUI> _itemUis = new List<ItemUI>();

        /// <summary>
        /// Creates an UI element for given item.
        /// </summary>
        public void CreateUI(Item item)
        {
            var element = Instantiate(itemUIPrefab, itemListView);
            
            var itemUI = element.GetComponent<ItemUI>();
            itemUI.Initialize(inventory, equipment, item);
            _itemUis.Add(itemUI);

            element.transform.localScale = Vector3.one;
        }

        /// <summary>
        /// Updates the given UI
        /// </summary>
        public void UpdateUI()
        {
            foreach (var ui in _itemUis)
            {
                ui.UpdateUI();
            }
        }
    }
}