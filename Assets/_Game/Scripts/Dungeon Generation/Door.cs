using System.Collections;
using Interfaces;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    private bool isOpen = false;
    private bool isTriggered = false;
    public bool isLocked = false;
    
    [HideInInspector] public bool isEntrance = false;
    [HideInInspector] public GameObject corridor = null;

    [SerializeField] private AudioClip doorOpenClip;
    [HideInInspector] public Door entranceDoor = null;
    [SerializeField] private AudioSource audioSource;

    public void Interact(GameObject gameObject)
    {
        if(!isLocked)
        {
            //Close corridor entrance and spawn mobs
            if(isEntrance)
            {
                if(!isTriggered)
                {
                    entranceDoor = corridor.GetComponent<Corridor>().entrance.door.GetComponent<Door>(); 
                    entranceDoor.isLocked = true;
                }
                
                StartCoroutine(entranceDoor.CloseDoor());
                var room = corridor.GetComponent<Corridor>().room.GetComponent<Room>();
                room.SpawnEnemies(corridor.GetComponent<Corridor>().exit.door.GetComponent<Door>());
            }
            
            StartCoroutine(OpenDoor());
        }
    }

    private IEnumerator OpenDoor()
    {
        if(isOpen) yield return null;
        
        audioSource.PlayOneShot(doorOpenClip);
        
        var time = 0f;
        var startPosition = transform.position;
        var targetPosition = new Vector3(startPosition.x, 2.5f, startPosition.z);
        var duration = doorOpenClip.length;

        while (time < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        
        transform.position = targetPosition;
        
        isOpen = true;
    }
    
    private IEnumerator CloseDoor()
    {
        if(!isOpen) yield return null;
        
        audioSource.PlayOneShot(doorOpenClip);

        var time = 0f;
        var startPosition = transform.position;
        var targetPosition = new Vector3(startPosition.x, 0, startPosition.z);
        var duration = doorOpenClip.length;

        while (time < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        
        transform.position = targetPosition;
        
        isOpen = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (isOpen && !isTriggered)
        {
            //Close corridor exit and activate mobs
            if (isEntrance && other.CompareTag("Player"))
            {
                isLocked = true;
                entranceDoor.isLocked = false;
                StartCoroutine(CloseDoor());
                isTriggered = true;
            }
        }
    }
}
