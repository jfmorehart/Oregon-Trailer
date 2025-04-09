using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointFollower : Drivable
{
    public int current_waypoint;
   

    // Update is called once per frame
    protected override void FixedUpdate()
    {
		base.FixedUpdate();

		if(current_waypoint >= ChunkManager.instance.waypoints.Length) return;



		DriveTowards(ChunkManager.instance.waypoints[current_waypoint]);
		Vector3 delta = ChunkManager.instance.waypoints[current_waypoint] - (Vector2)transform.position;

		_rb.AddForce(acceleration * Time.fixedDeltaTime * transform.right);
		

	    if(delta.magnitude < 2){
			current_waypoint++;
	    }

	}

}
