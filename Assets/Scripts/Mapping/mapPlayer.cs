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

    //strictly set the position with no animation
    public void setPositionStrict(MapNode origin, MapNode dest = null)
    {
        if(dest == null)
            transform.position = origin.VanPosition.position;
        else
            transform.position = Vector2.Lerp(origin.transform.localPosition, dest.transform.localPosition, 0.5f);
    }

    //this is called from the MapManager
    public void setPosition(MapNode origin, MapNode destination = null)
    {
        
        //just go to origin if destination is null
        if (destination == null)
        {
            //transform.position = origin.VanPosition.position;
            Vector2 ovanpos = new Vector2 (origin.transform.localPosition.x, origin.transform.localPosition.y + 40);
            //Debug.Log("dasdhbasdh" + origin.vanPositionDifference);
            //Vector2 ovanpos = new Vector2 (origin.transform.localPosition.x, origin.transform.localPosition.y + 50);
            StartCoroutine(setPositionRoutine(ovanpos));//origin.VanPosition.localPosition));
        }
        else
        {
            //otherwise go to next mid point
            //transform.position = Vector2.Lerp(origin.transform.position, destination.transform.position, 0.5f);
            Vector2 dvanpos = new Vector2(destination.transform.localPosition.x, destination.transform.localPosition.y + 40);

            StartCoroutine(setPositionRoutine(Vector2.Lerp(origin.transform.localPosition, dvanpos, 0.5f)));
        }
    }

    private IEnumerator setPositionRoutine(Vector2 endpos)
    {
        //Debug.Log();
        //this goes from whatever position its at now, to the specified position
        float duration = 2f;
        float t = 0;
        float startxVal = transform.localPosition.x;
        float startyVal = transform.localPosition.y;
        transform.localPosition = new Vector3(startxVal, startyVal);
        while (t < duration)
        {
            float xVal = EasingFunction.EaseInOutCubic( startxVal, endpos.x, t/duration);
            float yVal = EasingFunction.EaseInOutCubic(startyVal, endpos.y, t/duration);
            transform.localPosition = new Vector2(xVal, yVal);
            t += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = endpos;
    }


}
