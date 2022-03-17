using System;
using System.Collections.Generic;
using System.Linq;
using Alchemist.Input;
using UnityEngine;

namespace Item
{
    public class Equipment : MonoBehaviour
    {
        [SerializeField] private List<EquipmentSocket> sockets;

        private Inventory _inventory;
        private InputCapture _inputCapture;

        private void Awake()
        {
            _inventory = GetComponent<Inventory>();
            _inputCapture = GetComponent<InputCapture>();
        }

        private void Update()
        {
            if(_inputCapture.LeftMouseKeyDown)
                UseEquippedItemAtSocket(EquipmentSocketType.RightHand);
        }

        /// <summary>
        /// Removes an item from inventory and adds it to the equipment slot.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="type"></param>
        public void Equip(Item item, EquipmentSocketType type)
        {
            var targetSocket = sockets.First(socket => socket.socketType == type);

            if (targetSocket.item != null)
                Unequip(targetSocket.socketType);

            var position = targetSocket.socket.position;
            var rotation = targetSocket.socket.rotation;
            targetSocket.item = item;

            _inventory.Remove(item);

            var worldItem = ItemManager.Instance.SpawnItem(item, position, rotation, true);
            worldItem.transform.SetParent(targetSocket.socket);
        }

        /// <summary>
        /// Removes an item from equipment slot and adds it back to the inventory.
        /// </summary>
        /// <param name="type"></param>
        public void Unequip(EquipmentSocketType type)
        {
            var targetSocket = sockets.First(socket => socket.socketType == type);
            var itemInSocket = targetSocket.item;

            ClearSocket(type);
            
            _inventory.Add(itemInSocket);
        }

        /// <summary>
        /// Uses the item at given socket.
        /// </summary>
        /// <param name="type"></param>
        private void UseEquippedItemAtSocket(EquipmentSocketType type)
        {
            var targetSocket = sockets.First(socket => socket.socketType == type);
            var itemInSocket = targetSocket.item;
            
            itemInSocket?.InvokeItemUse(gameObject);
        }

        /// <summary>
        /// Clears the given socket
        /// </summary>
        /// <param name="type"></param>
        public void ClearSocket(EquipmentSocketType type)
        {
            var targetSocket = sockets.First(socket => socket.socketType == type);
            
            if (targetSocket.socket.childCount > 0)
                Destroy(targetSocket.socket.GetChild(0).gameObject);
            
            targetSocket.item = null;
        }
    }
}