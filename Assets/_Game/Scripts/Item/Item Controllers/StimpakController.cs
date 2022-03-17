using UnityEngine;

namespace Item
{
    public class StimpakController : ItemController
    {
        [Header("Stimpak Settings")]
        [SerializeField] private int healAmount = 50;
        [SerializeField] private AudioClip stimpakClip;

        public override void OnItemUse(Item item, GameObject owner)
        {
            AudioSource.PlayClipAtPoint(stimpakClip, transform.position);

            var stats = owner.GetComponent<CharacterStats>();
            stats.ModifyHealthOffset(healAmount);
            item.ModifyStackAmount(-1);
            
            Debug.Log(item.StackAmount);
            
            if(item.StackAmount <= 0)
                owner.GetComponent<Equipment>().ClearSocket(EquipmentSocketType.RightHand);
        }
    }

}
