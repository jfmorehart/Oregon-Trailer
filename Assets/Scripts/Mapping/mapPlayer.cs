using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class mapPlayer : MonoBehaviour
{
    //tracks where the player should be on the 
    public static mapPlayer instance;
    private Canvas canvas;
    [SerializeField]
    private Transform _o, _d;//

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


    private void Start()
    {
        canvas = MapManager.instance.GetComponent<Canvas>();
    }


    //strictly set the position with no animation
    public void setPositionStrict(MapNode origin, MapNode dest = null)
    {
        if (dest == null)
        {
            Vector3 _origin;
            RectTransformUtility.ScreenPointToWorldPointInRectangle((RectTransform)canvas.transform, Camera.main.WorldToScreenPoint(origin.transform.position), Camera.main, out _origin);
            Debug.Log("MAPPLAYER strict DEST NULL " + transform.position + " " + _origin);

            //transform.position = _origin;

            _o.transform.position = _origin;

            transform.position = _o.transform.position;
            //StartCoroutine(setPositionRoutine(_o.transform.position));

        }
        else
        {
            //transform.position = Vector2.Lerp(origin.transform.position, dest.transform.position, 0.5f);
            Vector2 dvanpos = new Vector2(dest.transform.position.x, dest.transform.position.y + 40);
            Vector3 dvan;
            RectTransformUtility.ScreenPointToWorldPointInRectangle((RectTransform)dest.transform, Camera.main.WorldToScreenPoint(dest.transform.position), Camera.main, out dvan);

            Vector3 _origin;
            RectTransformUtility.ScreenPointToWorldPointInRectangle((RectTransform)origin.transform, Camera.main.WorldToScreenPoint(origin.transform.position), Camera.main, out _origin);


            //transform.position = (Vector2.Lerp(_origin, dvan, 0.5f));
            //Debug.Log("MAPPLAYER strict DEST SET " + transform.position + " " + origin.VanPosition);


            transform.position = Vector2.Lerp(_o.transform.position, _d.transform.position, 0.5f);

        }

    }

    //this is called from the MapManager
    public void setPosition(MapNode origin, MapNode destination = null)
    {
        
        //just go to origin if destination is null
        if (destination == null)
        {
            //transform.position = origin.VanPosition.position;
            
            Vector2 ovanpos = new Vector2 (origin.transform.position.x, origin.transform.position.y + 40);
            //Debug.Log("dasdhbasdh" + origin.vanPositionDifference);
            //Vector2 ovanpos = new Vector2 (origin.transform.localPosition.x, origin.transform.localPosition.y + 50);

            _o.transform.position = origin.transform.position;

            StartCoroutine(setPositionRoutine(_o.transform.position));//origin.VanPosition.localPosition));

            //transform.position = Vector2.Lerp(origin.transform.position, dest.transform.position, 0.5f);

            Vector3 _origin;
            RectTransformUtility.ScreenPointToWorldPointInRectangle((RectTransform)origin.transform, Camera.main.WorldToScreenPoint(origin.transform.position), Camera.main, out _origin);

            Debug.Log("MAPPLAYER dest is null  " + transform.position + " " + origin.VanPosition);




        }
        else
        {
            //otherwise go to next mid point
            //transform.position = Vector2.Lerp(origin.transform.position, destination.transform.position, 0.5f);
            Vector2 dvanpos = new Vector2(destination.transform.position.x, destination.transform.position.y + 40);
            Vector3 dvan;
            RectTransformUtility.ScreenPointToWorldPointInRectangle((RectTransform)canvas.transform, Camera.main.WorldToScreenPoint(destination.transform.position), Camera.main, out dvan);

            Vector3 _origin;
            RectTransformUtility.ScreenPointToWorldPointInRectangle((RectTransform)canvas.transform, Camera.main.WorldToScreenPoint(origin.transform.position), Camera.main, out _origin);

            //create two transforms and set those to be where the van moves
            _o.transform.position = _origin;
            _d.transform.position = dvan;

            Vector2 midwayPos = Vector2.Lerp(_o.transform.position, _d.transform.position, 0.5f);

            StartCoroutine(setPositionRoutine(midwayPos, true));
            //get local position instead of world pos because it will spit the van onto the regular UI When this is used
            
            Debug.Log("MAPPLAYER dest is set " + transform.position + " " + _origin + " " + dvan +" "+ Vector2.Lerp(_origin, dvan, 0.5f));


        }
    }

    private IEnumerator setPositionRoutine(Vector2 endpos, bool goMidway = false)
    {
        //Debug.Log();
        //this goes from whatever position its at now, to the specified position
        float duration = 2f;
        float t = 0;
        float startxVal = transform.position.x;
        float startyVal = transform.position.y;
        transform.localPosition = new Vector3(startxVal, startyVal);
        while (t < duration)
        {
            float xVal = EasingFunction.EaseInOutCubic( _o.transform.position.x, _d.transform.position.x, t/duration);
            float yVal = EasingFunction.EaseInOutCubic(_o.transform.position.y, _d.transform.position.y, t/duration);
            transform.position = new Vector2(xVal, yVal);
            t += Time.deltaTime;
            yield return null;
        }
        
        if (goMidway)
            transform.position = Vector2.Lerp(_o.transform.position, _d.transform.position, 0.5f);
        else
            transform.position = endpos;
    }


    //find the position on screen of destination point(s)
    //set the origin position and destination position
    //make the van move to the position needed
}
