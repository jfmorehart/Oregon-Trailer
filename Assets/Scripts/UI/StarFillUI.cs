using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarFillUI : MonoBehaviour
{
    public GameObject starFill;
    
    // values needed to larp
    public float startX;
    public float endX;
    public float startTime = 1f;
    public float endTime = 20f;

    public void Start()
    {
        // take the start of x
        startX = starFill.transform.localPosition.x;
        endX = startX - 30; // TAKE THE WIDTH OF THE STAR
        // Notes: positioned the point of the image on the far right for smooth transition
        
        //make the mom object communicate with this script to give it a timer
    }

    public void Update()
    {
        // find the game object which possesses this script
        //InLevelCarSlider ilcs = GameObject.Find("Driving UI MOM").GetComponent<InLevelCarSlider>();
        //Debug.Log(MapManager.instance.PlayerCurrentTime);
        if(MapManager.instance.PlayerCurrentTime <= startTime)
            starFill.SetActive(true);
        
        //Debug.Log("Starfill  " + endTime + 
        //          " | " + (MapManager.instance.PlayerCurrentTime <=endTime) );
        if (MapManager.instance.PlayerCurrentTime >= startTime && MapManager.instance.PlayerCurrentTime < endTime) 
        {
            Debug.Log("More then start time");
            starFill.transform.localPosition = Vector3.Lerp(new Vector3(startX, starFill.transform.localPosition.y), 
                new Vector3(endX, starFill.transform.localPosition.y), 
                MapManager.instance.PlayerCurrentTime/(endTime - startTime));
        }
        if(MapManager.instance.PlayerCurrentTime >= endTime)
            starFill.SetActive(false);
    }

}
