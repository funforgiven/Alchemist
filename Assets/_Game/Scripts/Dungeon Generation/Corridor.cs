using UnityEngine;

public class Corridor : MonoBehaviour
{
    [SerializeField] public Exit entrance;
    [SerializeField] public Exit exit;
    
    
    [HideInInspector] public bool isColliding = false;
    [HideInInspector] public GameObject room;
    
    private void OnCollisionStay(Collision other)
    {
        isColliding = true;
    }
    
    private void OnCollisionExit(Collision other)
    {
        isColliding = false;
    }
}
