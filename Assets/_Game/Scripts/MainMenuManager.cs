using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{

    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject loading;
    
    public void HandleExit()
    {
        Application.Quit();
    }

    public void HandleStart()
    {
        menu.SetActive(false);
        loading.SetActive(true);
        SceneManager.LoadScene(1);
    }
}
