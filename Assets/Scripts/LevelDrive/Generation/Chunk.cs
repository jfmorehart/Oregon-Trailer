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

	private void Awake()
	{
		//dimensions = transform.Find("back").transform.localScale;
		Transform roadstart = transform.Find("road_start_point");
		Transform roadend = transform.Find("road_end_point");
		dimensions.x = Mathf.Abs(roadstart.position.x - roadend.position.x);
		dimensions.y = Mathf.Abs(roadstart.position.y - roadend.position.y);
		//dimensions = transform.GetChild(1).transform.localScaae;
	}

	public void setQuest(TextAsset q)
	{
		if (questPoint != null)
			questPoint.quest = q;
	}
}
