using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameManager : MonoBehaviour
{
    public void HandleMenu()
    {
        SceneManager.LoadScene(0);
    }
}
