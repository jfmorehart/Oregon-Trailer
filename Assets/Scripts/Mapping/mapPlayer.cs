using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mapPlayer : MonoBehaviour
{
    //tracks where the player should be on the 
    public static mapPlayer instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else if (instance == null)
        {
            instance = this;
        }
    }

    //this is called from the MapManager
    public void setPosition(MapNode origin, MapNode destination = null)
    {
        //just go to origin if destination is null
        if (destination == null)
        {
            transform.position = origin.VanPosition.position;
        }
        else
        {
            //otherwise go to next mid point
            transform.position = Vector2.Lerp(origin.transform.position, destination.transform.position, 0.5f);
        }
    }
}
