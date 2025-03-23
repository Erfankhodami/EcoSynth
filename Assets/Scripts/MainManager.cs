using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    public bool playerGotSward = false;
    public int numberOfEcoInk=0;
    void Awake()
    {
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void SwitchScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void RestartLevel()
    {
        numberOfEcoInk = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
