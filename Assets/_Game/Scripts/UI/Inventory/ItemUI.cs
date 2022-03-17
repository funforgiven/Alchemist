using TMPro;
using UnityEngine;

namespace Item
{
    public class ItemUI : MonoBehaviour
    {
        [Header("Item UI Settings")]
        public TextMeshProUGUI itemText;
        
        private Inventory _inventory;
        private Equipment _equipment;

        private Item _item;

        /// <summary>
        /// Initializes the item UI.
        /// </summary>
        /// <param name="inventory">Inventory</param>
        /// <param name="equipment">Equipment</param>
        /// <param name="item">Initialized item</param>
        public void Initialize(Inventory inventory, Equipment equipment, Item item)
        {
            _inventory = inventory;
            _equipment = equipment;
            _item = item;

            UpdateUI();
        }

        /// <summary>
        /// Updates the UI text.
        /// </summary>
        public void UpdateUI()
        {
            if(_item.Stackable)
                itemText.text = _item.Name + " x " + _item.StackAmount;
            else
                itemText.text = _item.Name;
        }
        
        /// <summary>
        /// Equips the item
        /// </summary>
        public void Equip()
        {
            if (_item != null && _inventory)
            {
                _equipment.Equip(_inventory.Get(_item), EquipmentSocketType.RightHand);
                Destroy(gameObject);
            }
        }
    }
}

