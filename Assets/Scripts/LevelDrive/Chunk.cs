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

	private void Awake()
	{
		dimensions = transform.Find("back").transform.localScale;
		//dimensions = transform.GetChild(1).transform.localScale;
	}

	public void setQuest(TextAsset q)
	{
		if (questPoint != null)
			questPoint.quest = q;
	}
}
