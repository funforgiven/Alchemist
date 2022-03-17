using UnityEngine;

namespace Item
{
    [System.Serializable]
    public class EquipmentSocket
    {
        public EquipmentSocketType socketType;
        public Transform socket;
        public Item item;
    }
    
    [System.Serializable]
    public enum EquipmentSocketType
    {
        Helmet,
        Chest,
        LeftHand,
        RightHand,
        Leggings,
        Boots
    }
}