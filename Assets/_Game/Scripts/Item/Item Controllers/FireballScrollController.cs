using UnityEngine;

namespace Item
{
    public class FireballScrollController : ScrollController
    {
        [SerializeField] private GameObject fireballPrefab;
        
        public override void OnScrollCast(GameObject owner)
        {
            var spawnPos = owner.GetComponentInChildren<Camera>().transform.position;
            var spawnRot = owner.GetComponentInChildren<Camera>().transform.rotation;
            Instantiate(fireballPrefab, spawnPos, spawnRot);
        }
    }
}