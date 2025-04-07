using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointFollower : Drivable
{
    public int current_waypoint;
   

    // Update is called once per frame
    void Update()
    {
		if(current_waypoint >= ChunkManager.instance.waypoints.Length) return;

		_rb.AddForce(acceleration * Time.fixedDeltaTime * transform.right);
		DriveTowards(ChunkManager.instance.waypoints[current_waypoint]);
	    Vector2 delta = ChunkManager.instance.waypoints[current_waypoint] - (Vector2)transform.position;
	    if(delta.magnitude < 1){
			current_waypoint++;
	    }

	}
}
