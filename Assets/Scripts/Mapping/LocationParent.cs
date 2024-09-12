using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationParent : MonoBehaviour
{
    //This holds all the generic information for each location.
    //All locations derive from this. They will hold the actual event for the town. 

    //The All of the roads Out of this locations
    //effectively have a list of roads that head west, with those holding the ref to the town they connect to.
    //use their town information to figure out the options. 
    [SerializeField]
    [Tooltip("Any roads that the player should be able to travel from should be placed here")]
    private List<Road> WestBoundRoads = new List<Road>();
    
    //should probably have a background to display when in the town so it doesnt look like a black background?

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //probably do something cool here, like create a town popup. Similar to dialogue system
    


}
