using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TempLevelUnloader : MonoBehaviour
{
    //this is a temp function to load and unload a level from 
    [SerializeField]
    GameObject level;

    public void unloadLevel()
    {
		Debug.Log("unloadLevel");
		SceneManager.LoadScene("AaronLevel");
        //level.SetActive(false);
    }
    
    public void loadLevel()
    {
        Debug.Log("loadLevel");
        SceneManager.LoadScene("Level");
        //level.SetActive(true);
    }
}
