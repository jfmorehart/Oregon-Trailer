using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turnPoint : LocationPoint
{
    //signal the turn point sign to come into view

    [SerializeField]
    public Road secondRoadConnection;

    private void Awake()
    {
        //Debug.Log("TURNPOINT INIT");

        roadConnection.origin = this;
        secondRoadConnection.origin = this;
        if (roadConnection.endingRoadCheck())
        {
            roadConnection.Destination = this;
        }
        if (secondRoadConnection.endingRoadCheck())
        {
            secondRoadConnection.Destination = this;
        }
    }
    //when the player reaches this point, pick a direction for it to go

    //player pulls up and becomes attached to this. 
    //create sign object that moves into screen.
    //sign communicates with this 

    
    public void chooseDestination()
    {
        //give the van its destination.
        //if van has destination then allow it to run

        //have a subtle slowdown to the player's movement / parallax. 
        //spawn in thing here


    }
    public override void becomeOrigin()
    {
        vanMapMovement.instance.transform.position = transform.position;
        Debug.Log("BecomeOrigin");
        //vanMapMovement.instance.setDestination(roadConnection.Destination);
        vanMapMovement.instance.setOrigin(roadConnection);
        Debug.Log("BECOME ORIGIN");
        GameManager.stopVan();
        GameManager.instance.createSign(roadConnection, secondRoadConnection);
    }
    public override void init()
    {
        roadConnection.origin = this;
    }

}
