using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    public static ChunkManager instance;
    public bool generateOnStart = false;

    public int levelSize;
    public Chunk[] level;
    public Chunk[] chunkBag;
    public Chunk[] QuestChunkBag;
    public List<TextAsset> Quests;

    public int enemySpawnsRemaining;

    public GameObject boundaryStack;
    public GameObject endHouse;

    GameObject spawnedEndHouse = null;

    [SerializeField]
    private GameObject VanObj;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start() {
        if(generateOnStart) GenerateLevel();
	}



    public void DestroyLevel() {
		if (level != null)
		{
            Debug.Log("LevelDestroyingSelf");
			for (int i = 0; i < level.Length; i++)
			{
                if (level[i] != null)
                {
                    Destroy(level[i].gameObject);
                }
            }

            if (MouseDriving.vanTransform != null)
            {
                Destroy(MouseDriving.vanTransform.gameObject);

            }
		}
        level = null;
	}

    public void GenerateLevel(List<TextAsset> questsToSpawn)
    {
        Quests = questsToSpawn;
        Debug.Log("Generating Level Request");
        GenerateLevel();
    }

    public void GenerateLevel()
    {
        DestroyLevel();

        level = new Chunk[levelSize];
        float levelLengthSoFar = 0;
        float lastRoadY = 0;
        //the van is not rotated in the prefab

        //pick out which chunks to spawn quests at
        if (Quests.Count > levelSize)
        {
            Debug.Log("Too many quests to spawn compared to level size");
            return;
        }

        List<int> chunksToSpawnQuestsAt = new List<int>();
        Debug.Log("chunk list size" + chunksToSpawnQuestsAt.Count);
        Debug.Log("Questsize" + Quests.Count);
        int internalcount = 0;

        //chooses which chunk index each quest should be spawned at
        for (int i = 0; i < Quests.Count; i++)
        {
            int x = Random.Range(1, levelSize);
            Debug.Log("gen num: " + x);

            while (chunksToSpawnQuestsAt.Contains(x))
            {
                x = Random.Range(1,levelSize);

                Debug.Log("repeat number found - new num "+ x);
            }
            chunksToSpawnQuestsAt.Add(x);

        }
        

        Debug.Log("crAC " + Quests.Count + " " + chunksToSpawnQuestsAt.Count);

        Instantiate( VanObj, Vector2.zero, Quaternion.Euler(0,0, 180));
        for (int i= 0; i < levelSize; i++) {

            Chunk toSpawn;  //prefab

            //this is slow but we can change chunkstospawnquestat to a hashset or something soon
            if (chunksToSpawnQuestsAt.Contains(i))
            {
                toSpawn = QuestChunkBag[Random.Range(0, QuestChunkBag.Length)];
                toSpawn.setQuest(Quests[0]);
                Quests.RemoveAt(0);
            }
            else
                toSpawn = chunkBag[Random.Range(0, chunkBag.Length)];


            toSpawn = Instantiate(toSpawn, transform); //new instance
            level[i] = toSpawn;
            
            

            
            //DO NOT SPAWN ENEMIES IN THE FIRST CHUNK
            if (i != 0 && toSpawn.enemies.Count > 0)
            {
                //Enemy spawn system, not very good

                int overSpawn = toSpawn.enemies.Count - enemySpawnsRemaining;
                for (int e = 0; e < overSpawn; e++)
                {
                    Destroy(toSpawn.enemies[0].gameObject);
                    toSpawn.enemies.RemoveAt(0);
                }
                enemySpawnsRemaining -= toSpawn.enemies.Count;

            }
            else
            {
                int overSpawn = toSpawn.enemies.Count;
                for (int e = 0; e < overSpawn; e++)
                {
                    Destroy(toSpawn.enemies[0].gameObject);
                    toSpawn.enemies.RemoveAt(0);
                }
                enemySpawnsRemaining -= toSpawn.enemies.Count;

            }

            //Aligning levels
            levelLengthSoFar -= toSpawn.dimensions.x * 0.5f;
            //Transform road = toSpawn.transform.GetChild(0);
            //Vector2 startpos = road.position + 0.5f * road.localScale.x * road.right;
            Vector2 startpos = toSpawn.transform.Find("road_start_point").position;
            float startRoadY = startpos.y;

            float yDiff = lastRoadY - startRoadY;

			toSpawn.transform.position = new Vector3(levelLengthSoFar, yDiff, 0);
            levelLengthSoFar -= toSpawn.dimensions.x * 0.5f;

            //road = toSpawn.transform.GetChild(0);
            //Vector2 endpos = road.position - 0.5f * road.localScale.x * road.right;
            Vector2 endpos = toSpawn.transform.Find("road_end_point").position;
			lastRoadY = endpos.y;
            Debug.Log(lastRoadY);

            ////boundaries
            //Debug.Log(i + " diff = " + yDiff);
            //GameObject boundary = Instantiate(boundaryStack, transform);
            //boundary.transform.position = new Vector3(levelLengthSoFar + toSpawn.dimensions.x * 0.5f, toSpawn.dimensions.y * 0.5f + yDiff);
            //boundary.transform.localScale = new Vector3(1, yDiff, 1);
            //boundary = Instantiate(boundaryStack, transform);
            //boundary.transform.position = new Vector3(levelLengthSoFar + toSpawn.dimensions.x * 0.5f, -toSpawn.dimensions.y * 0.5f + yDiff);
            //boundary.transform.localScale = new Vector3(1, yDiff, 1);
        }

        if (spawnedEndHouse == null)
        {
            spawnedEndHouse = Instantiate(endHouse, new Vector3(levelLengthSoFar, lastRoadY, 0), Quaternion.identity, transform); //new instance
        }
        else
        {
            spawnedEndHouse.transform.position = new Vector3(levelLengthSoFar, lastRoadY, 0);
        }
    }

	// Update is called once per frame
	void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) {
            //GenerateLevel();
	    }
    }
}
