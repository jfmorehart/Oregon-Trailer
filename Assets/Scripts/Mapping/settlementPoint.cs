using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class settlementPoint : LocationPoint
{

    public override void becomeOrigin()
    {

        
        vanMapMovement.instance.setDestination(roadConnection.Destination);
        vanMapMovement.instance.setOrigin(roadConnection);
        
    }



    public override void init()
    {
        //Debug.Log("Initialized");
        roadConnection.origin = this;
        roadConnection.endingRoadCheck();
        if (roadConnection.endingRoadCheck())
        {
            roadConnection.Destination = this;
        }
    }
}
