using Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;


public class End : MonoBehaviour, IInteractable
{
    public void Interact(GameObject go)
    {
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene(2);
    }
}
