using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vanMapMovement : MonoBehaviour
{
    //has locationPoint that it is trying to go to
    [SerializeField]
    LocationPoint origin;
    //has locationpoint that it is going from
    [SerializeField]
    LocationPoint destination;
    //move in a straight line from each point. Check distance and see what percent we move. Based on the map movement 
    public static vanMapMovement instance;

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

    private void Update()
    {
        vanRotation();

        vanMovement();
    }

    private void vanMovement()
    {
        if (GameManager.VanRunning)
        {
            //move to next point
            transform.position = Vector2.MoveTowards(transform.position, destination.transform.position, GameManager.VanSpeed * Time.deltaTime);
        }
        //get percent of the way between the origin and destination using the distance from destination.
        //starting distance (dist: origin, destination) current distance from ending/beginning
        //currentDist/startingDist

        
    }

    public void init(LocationPoint o)
    {
        origin = o;
        transform.position = origin.transform.position;
    }
    private void vanRotation()
    {
        //calculate rotation the van should be at depending on angle they are moving
    }

    public void setOrigin(LocationPoint origin)
    {
        this.origin = origin;
    }

    public void setDestination(LocationPoint destination)
    {
        this.destination = destination;
    }
}
