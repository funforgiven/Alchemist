using UnityEngine;

namespace Item
{
    public class ScrollController : ItemController
    {
        [Header("Audio")]
        [SerializeField] private AudioClip CastSound;

        public override void OnItemUse(Item item, GameObject owner)
        {
            AudioSource.PlayClipAtPoint(CastSound, transform.position);

            item.ModifyStackAmount(-1);
            OnScrollCast(owner);

            if(item.StackAmount <= 0)
                owner.GetComponent<Equipment>().ClearSocket(EquipmentSocketType.RightHand);
        }

        public virtual void OnScrollCast(GameObject owner)
        {
        }
    }

}
