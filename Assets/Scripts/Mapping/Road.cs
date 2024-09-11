using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour
{
    [Tooltip("This is the location that this road leads to.")]
    [SerializeField]
    private LocationParent _targetLocation;

    public LocationParent TargetLocation { get { return _targetLocation; } }

    //probably need a reference to the bezier curve path
    //todo watch sebastian lague video on how to do it

    private void Start()
    {
        if (_targetLocation == null)
        {
            Debug.LogError("Road location is not set! \n   Road : " + transform.name);
        }
    }


    public void goToNextLocation()
    {
        //player will go from the start of this road, to the end of the road.
        //todo: check to see if this should be vehicle driven. Should the vehicle set its position instead of the player?
        //this way the random events could be set up easier
    }

}
