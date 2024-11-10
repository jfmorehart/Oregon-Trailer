using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class questMarker : MonoBehaviour
{
    [SerializeField]
    Transform target;
    [SerializeField]
    Image shownImage;


    public void initialize(Transform targetTransform, Sprite img)
    {

    }



    void Update()
    {
        //check to see if we are within the bounds of the screen
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(target.position);
        bool onScreen =  screenPosition.x > 0 && screenPosition.x < Screen.width && screenPosition.y > 0 && screenPosition.y < Screen.height;
        //this works^, need to figure out how to properly display location on canvas now

        Debug.Log("Target onscreen:"+onScreen);

        //clamp position    
        Vector2 pos = screenPosition;
        pos.x = Mathf.Clamp(pos.x, 0, Screen.width);
        pos.y = Mathf.Clamp(pos.y, 0, Screen.height);

    }


}

