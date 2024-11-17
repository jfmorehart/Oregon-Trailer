using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempLevelUnloader : MonoBehaviour
{
    //this is a temp function to load and unload a level from 
    [SerializeField]
    GameObject level;

    public void unloadLevel()
    {
        level.SetActive(false);
    }
    
    public void loadLevel()
    {
        level.SetActive(true);
    }
}
