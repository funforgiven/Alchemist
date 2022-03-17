using Item;
using UnityEngine;

public class IceballScrollController : ScrollController
{
    [SerializeField] private GameObject iceballPrefab;
        
    public override void OnScrollCast(GameObject owner)
    {
        var spawnPos = owner.GetComponentInChildren<Camera>().transform.position;
        var spawnRot = owner.GetComponentInChildren<Camera>().transform.rotation;
        Instantiate(iceballPrefab, spawnPos, spawnRot);
    }
}
