using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
	public float enemySpawnMultiplier = 1f;
	public int enemiesSpawned;
	public int enemyMaxThisChunk;
	public Vector2 dimensions;
	public List<EnemyVan> enemies = new List<EnemyVan>();
	public QuestPoint questPoint;
	public Waypoint[] wayPoints;

	public Transform[] boundary_top;
	public Transform[] boundary_bottom;

	private void Awake()
	{
		//dimensions = transform.Find("back").transform.localScale;
		Transform roadstart = transform.Find("road_start_point");
		Transform roadend = transform.Find("road_end_point");
		dimensions.x = Mathf.Abs(roadstart.position.x - roadend.position.x);
		dimensions.y = Mathf.Abs(roadstart.position.y - roadend.position.y);
		//dimensions = transform.GetChild(1).transform.localScaae;

		//(float, float) updown = (float.MinValue, float.MaxValue);
		//if (boundary_top != null) { 
		//	if(boundary_top.Length == 0) {
		//		//add procedural points
		//		updown = VerticalExtents();
		//		boundary_top = new Transform[2];
		//		Vector3 point = new Vector3(transform.position.x + dimensions.x * 0.5f, updown.Item1, 0);
		//		boundary_top[0] = Instantiate(ChunkManager.instance.boundary_point, point, Quaternion.identity).transform;
		//		point.x = transform.position.x - dimensions.x * 0.5f;
		//		boundary_top[1] = Instantiate(ChunkManager.instance.boundary_point, point, Quaternion.identity).transform;
		//	}
		//}
		//if (boundary_bottom != null)
		//{
		//	if (boundary_bottom.Length == 0)
		//	{
		//		//add procedural points
		//		if(updown.Item1 == float.MinValue) {
		//			updown = VerticalExtents();
		//		}
		//		boundary_bottom = new Transform[2];
		//		Vector3 point = new Vector3(transform.position.x + dimensions.x * 0.5f, updown.Item2, 0);
		//		boundary_bottom[0] = Instantiate(ChunkManager.instance.boundary_point, point, Quaternion.identity).transform;
		//		point.x = transform.position.x - dimensions.x * 0.5f;
		//		boundary_bottom[1] = Instantiate(ChunkManager.instance.boundary_point, point, Quaternion.identity).transform;
		//	}
		//}
	}

	public void setQuest(TextAsset q)
	{
		if (questPoint != null)
			questPoint.quest = q;
	}

	
	public (float, float) VerticalExtents() {
		float top = 0, bottom = 0;
		for(int i = 0; i < transform.childCount; i++) {
			top = Mathf.Max(transform.GetChild(i).transform.position.y, top);
			bottom = Mathf.Min(transform.GetChild(i).transform.position.y, bottom);
		}
		return (top, bottom);
    }
}
