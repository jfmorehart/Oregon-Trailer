using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointFollower : Drivable
{
    public int current_waypoint;
	public int waypoint_size = 2;
   

    // Update is called once per frame
    protected override void FixedUpdate()
    {
		base.FixedUpdate();

		if(current_waypoint >= ChunkManager.instance.waypoints.Length) return;



		DriveTowards(ChunkManager.instance.waypoints[current_waypoint]);
		Vector3 delta = ChunkManager.instance.waypoints[current_waypoint] - (Vector2)transform.position;

		_rb.AddForce(acceleration * Time.fixedDeltaTime * transform.right);
		

	    if(delta.magnitude < waypoint_size){
			current_waypoint++;
	    }

	}

}
