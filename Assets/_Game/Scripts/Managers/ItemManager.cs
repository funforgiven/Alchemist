using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Item
{
    public class ItemManager : MonoBehaviour
    {
        // Singleton
        public static ItemManager Instance { get; private set; }

        [SerializeField] private List<ItemData> itemData;
        [SerializeField] private GameObject itemIndicatorPrefab;

        private void Awake()
        {
            // Setup singleton
            if (Instance != null && Instance != this)
                Destroy(gameObject);
            else
                Instance = this;
        }

        public GameObject SpawnItem(string itemName, Vector3 spawnPos, bool equipped = false, int initialStack = 1)
        {
            var matchingItem = itemData.FirstOrDefault(data => data.itemName == itemName);
            if (matchingItem == null) throw new Exception($"Could not find item named {itemName}!");

            var item = new Item(matchingItem, initialStack);
            return SpawnItem(item, spawnPos, equipped, initialStack);
        }
        
        public GameObject SpawnItem(string itemName, Vector3 spawnPos, Quaternion rotation, bool equipped = false, int initialStack = 1)
        {
            var matchingItem = itemData.FirstOrDefault(data => data.itemName == itemName);
            if (matchingItem == null) throw new Exception($"Could not find item named {itemName}!");

            var item = new Item(matchingItem, initialStack);
            return SpawnItem(item, spawnPos, rotation, equipped, initialStack);
        }
        public GameObject SpawnItem(Item item, Vector3 spawnPos, bool equipped = false, int initialStack = 1)
        {
            var worldItem = Instantiate(item.Prefab, spawnPos, Quaternion.identity);
            
            if(equipped)
                worldItem.GetComponent<ItemWorld>().EnableEquipBehaviour(item);
            else
            {
                var itemIndicator = Instantiate(itemIndicatorPrefab);
                itemIndicator.GetComponent<ItemIndicator>().targetTransform = worldItem.transform;
                worldItem.GetComponent<ItemWorld>().EnableWorldBehaviour(item, itemIndicator);
            }

            return worldItem;
        }
        
        public GameObject SpawnItem(Item item, Vector3 spawnPos, Quaternion rotation, bool equipped = false, int initialStack = 1)
        {
            var worldItem = Instantiate(item.Prefab, spawnPos, rotation);
            
            if(equipped)
                worldItem.GetComponent<ItemWorld>().EnableEquipBehaviour(item);
            else
            {
                var itemIndicator = Instantiate(itemIndicatorPrefab);
                itemIndicator.GetComponent<ItemIndicator>().targetTransform = worldItem.transform;
                worldItem.GetComponent<ItemWorld>().EnableWorldBehaviour(item, itemIndicator);
            }

            return worldItem;
        }

        public GameObject SpawnRandomItemAt(Vector3 spawnPos, Quaternion rotation)
        {
            var randomItemData = itemData[Random.Range(0, itemData.Count)];

            var initialStack = 1;
            if (randomItemData.itemStackable)
                initialStack = Random.Range(1, 10);
            
            return SpawnItem(randomItemData.itemName, spawnPos, rotation, false, initialStack);
        }

        [ContextMenu("Spawn Random Item")]
        public void SpawnWoodenSword()
        {
            SpawnRandomItemAt(new Vector3(0, 1, 0), Quaternion.identity);
        }
    }

}
