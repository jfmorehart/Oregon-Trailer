using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuntManager : MonoBehaviour
{
    [SerializeField]
    private GameObject huntScene;
    public static HuntManager instance;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }


    public void displayHunt()
    {
        huntScene.SetActive(true);

    }
    public void hideHunt()
    {
        huntScene.SetActive(false);

        //communicate with map manager to display the map
        MapManager.instance.nodeActivityDone();

    }

}
