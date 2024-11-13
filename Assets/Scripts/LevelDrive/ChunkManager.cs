using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    public int levelSize;
    public Chunk[] level;
    public Chunk[] chunkBag;
    public int enemySpawnsRemaining;

    public GameObject boundaryStack;
    public GameObject endHouse;

    void Start() {
        GenerateLevel();
    }

    void GenerateLevel()
    {
        if(level != null) { 
	        for(int i = 0; i < level.Length; i++) {
                Destroy(level[i]);
	        }
	    }
        level = new Chunk[levelSize];
        float levelLengthSoFar = 0;
        float lastRoadY = 0;
        
        for(int i= 0; i < levelSize; i++) {
            Chunk toSpawn = chunkBag[Random.Range(0, chunkBag.Length)]; //prefab
            toSpawn = Instantiate(toSpawn, transform); //new instance

            //Enemy spawn system, not very good
            int overSpawn = toSpawn.enemies.Count - enemySpawnsRemaining;
			for (int e = 0; e < overSpawn ; e++)
			{
				Destroy(toSpawn.enemies[0].gameObject);
				toSpawn.enemies.RemoveAt(0);
			}
			enemySpawnsRemaining -= toSpawn.enemies.Count;

            //Aligning levels
			levelLengthSoFar -= toSpawn.dimensions.x * 0.5f;
			Transform road = toSpawn.transform.GetChild(0);
            Vector2 startpos = road.position + 0.5f * road.localScale.x * road.right;
            float startRoadY = startpos.y;

            float yDiff = lastRoadY - startRoadY;

			toSpawn.transform.position = new Vector3(levelLengthSoFar, yDiff, 0);
            levelLengthSoFar -= toSpawn.dimensions.x * 0.5f;

            road = toSpawn.transform.GetChild(0);
            Vector2 endpos = road.position - 0.5f * road.localScale.x * road.right;
            lastRoadY = endpos.y;

			////boundaries
			//Debug.Log(i + " diff = " + yDiff);
			//GameObject boundary = Instantiate(boundaryStack, transform);
			//boundary.transform.position = new Vector3(levelLengthSoFar + toSpawn.dimensions.x * 0.5f, toSpawn.dimensions.y * 0.5f + yDiff);
			//boundary.transform.localScale = new Vector3(1, yDiff, 1);
			//boundary = Instantiate(boundaryStack, transform);
			//boundary.transform.position = new Vector3(levelLengthSoFar + toSpawn.dimensions.x * 0.5f, -toSpawn.dimensions.y * 0.5f + yDiff);
			//boundary.transform.localScale = new Vector3(1, yDiff, 1);
		}
	    Instantiate(endHouse, new Vector3(levelLengthSoFar, lastRoadY, 0), Quaternion.identity, transform); //new instance
	}

	// Update is called once per frame
	void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) {
            GenerateLevel();
	    }
    }
}
