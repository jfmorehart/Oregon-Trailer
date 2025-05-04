using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    public static ChunkManager instance;
    public bool test_autoGenerate = false;
    public bool singleChunkMode;

    public int levelSize;
    public Chunk[] level;
    public Chunk[] chunkBag;

    [SerializeField]
    public ChunkBiome[] environment_chunkBags;

    public Chunk[] QuestChunkBag;
    public List<TextAsset> Quests;

    public int enemySpawnsRemaining;

    public GameObject boundaryStack;
    public GameObject endHouse;

    GameObject spawnedEndHouse = null;

    public Vector2[] waypoints;
	public Vector2[] boundary_top;
	public Vector2[] boundary_bottom;

	public GameObject boundary_prefab;
	public GameObject boundary_point;
	public float boundary_width;

	[SerializeField]
    private GameObject VanObj;

	public GameObject riverMaker;

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

    void SortWaypoints() {
		//manual sorting
		Waypoint[] to_add;
		List<Vector2> v2s = new();
		for (int i = 0; i < level.Length; i++)
		{
			to_add = level[i].wayPoints;
			for (int j = 0; j < to_add.Length; j++)
			{
				v2s.Add(to_add[j].transform.position);
			}
		}
		waypoints = v2s.ToArray();

		//boundaries
        v2s.Clear();
		List<Vector2> bottom = new List<Vector2>();

		//close out the back
		//Instantiate(boundary_point, Vector2.right *30, Quaternion.Euler(0, 0, 90), transform);
		v2s.Add(Vector2.right * 80);
		v2s.Add(Vector2.right * 30);
		bottom.Add(Vector2.right * 30);

		for (int i = 0; i < level.Length; i++)
		{
			int topL = 0;
			if (level[i].boundary_top != null) {
				topL = level[i].boundary_top.Length;
			}
			int botL = 0;
			if(level[i].boundary_bottom != null)
			{
				botL = level[i].boundary_bottom.Length;
			}

			for (int j = 0; j < Mathf.Max(topL, botL); j++)
			{
				if(j < topL) {
					v2s.Add(level[i].boundary_top[j].transform.position);
				}
				if (j < botL)
				{
					bottom.Add(level[i].boundary_bottom[j].transform.position);
				}
			}
		}
		//close out the end
		v2s.Add(spawnedEndHouse.transform.position - Vector3.right * 30);
		bottom.Add(spawnedEndHouse.transform.position - Vector3.right * 30);
		Instantiate(boundary_point, spawnedEndHouse.transform.position - Vector3.right * 10, Quaternion.Euler(0, 0, 90), transform);

		if(bottom.Count > 3) {
			boundary_bottom = bottom.ToArray();
			InstantiateBoundary(boundary_bottom);
		}
		if(v2s.Count > 3) {
			boundary_top = v2s.ToArray();
			InstantiateBoundary(boundary_top);
		}
	}

	void InstantiateBoundary(Vector2[] points) {
		GameObject parent = Instantiate(new GameObject(), transform);
		parent.name = "Boundary";

		for (int i = 0; i < points.Length; i++)
		{
			if (points.Length > i + 1)
			{
				Vector3 middle = points[i] + points[i + 1];
				middle *= 0.5f;
				middle.z = 9;
				Vector2 delta = points[i + 1] - points[i];
				GameObject go = Instantiate(boundary_prefab, middle, Quaternion.identity, parent.transform);
				go.transform.localScale = new Vector3(delta.magnitude, boundary_width, 0);
				go.transform.right = delta;
				go.transform.eulerAngles = new Vector3(0, 0, go.transform.eulerAngles.z);
			}
			else
			{
				//this is the last point
			}
		}

		//RiverMeshMaker rmm = Instantiate(riverMaker, transform).GetComponent<RiverMeshMaker>();
		//rmm.Generate(points);
    }

	void Start() {
        if(test_autoGenerate) RandomGenerateLevel();


        //auto sorting

		//GameObject[] waypoint_objects = GameObject.FindGameObjectsWithTag("Waypoint");

		//Debug.Log("tagged waypoints: " + waypoint_objects.Length);
		//List<Waypoint> to_order = new List<Waypoint>();
		//List<int> indices = new List<int>();
		//for (int i = 0; i < waypoint_objects.Length; i++)
		//{
		//	if (waypoint_objects[i].TryGetComponent(out Waypoint way))
		//	{
		//		to_order.Add(way);

  //              Chunk recurse = Recurse(way.transform);
  //              if(recurse == null) {
  //                  indices.Add(way.order);
  //              }
  //              else { 
		//            for(int x = 0; x < level.Length; x++) {
  //                      if (level[x] == recurse) {
  //                          indices.Add(way.order + x * 1000);
		//				}
		//            }
		//        }
		//	}
		//}
		//Waypoint[] ordered = to_order.ToArray();
		//int[] keys = indices.ToArray();
	 //   waypoints = new Vector2[ordered.Length];
		//System.Array.Sort(keys, ordered);
		//for (int i = 0; i < ordered.Length; i++)
		//{
		//	waypoints[i] = ordered[i].gameObject.transform.position;
		//}
	}
    Chunk Recurse(Transform g) {
		if (g.parent == null)
		{
            return null;
		}
		if (g.parent.TryGetComponent(out Chunk chunk))
		{
            return chunk;
		}
        return Recurse(g.parent);
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

            if (PlayerVan.vanTransform != null)
            {
                Destroy(PlayerVan.vanTransform.gameObject);

            }
		}
        level = null;
	}

    public RoadPath path;
    public void GenerateLevel(List<TextAsset> questsToSpawn)
    {
        Quests = questsToSpawn;
        Debug.Log("Generating Level Request");
        if (MapManager.instance.playersCurrentNode.Roads.Length < 1) {

            Debug.LogError("Level Request Denied: No road info set in currentNode!");
            return;
	    }
        path = MapManager.instance.playersNewPath;
        Debug.Log("path:" + path);
        levelSize = path.roadLength;
        singleChunkMode = levelSize == 1;
        //feed chunkbag from BiomeType enum
        chunkBag = environment_chunkBags[(int)path.type].chunks;

        RandomGenerateLevel();
    }

    public void RandomGenerateLevel()
    {
        DestroyLevel();

        if (levelSize == 0)
        {
            levelSize = 5;
        }


        //the van is not rotated in the prefab
        if (Quests == null)
        {
            Debug.Log("Quests is null");
            Quests = new List<TextAsset>();
        }
        //pick out which chunks to spawn quests at
        if (Quests != null && Quests.Count > levelSize)
        {
            Debug.LogError("Too many quests to spawn compared to level size");
            return;
        }

        List<int> chunksToSpawnQuestsAt = new List<int>();
        //Debug.Log("chunk list size" + chunksToSpawnQuestsAt.Count);
        //Debug.Log("Questsize" + Quests.Count);

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

		level = new Chunk[levelSize];
        //Debug.Log(level.GetLength(0));
		GenerationLoop(chunksToSpawnQuestsAt);
        Transform endpoint = level[levelSize - 1].transform.Find("road_end_point");
        if(endpoint == null) {
            Debug.LogError("level contains no endpoint!");
            return;
	    }
	    Vector2 endpos = endpoint.position;
		if (spawnedEndHouse == null)
        {
            spawnedEndHouse = Instantiate(endHouse, endpos, Quaternion.identity, transform); //new instance
        }
        else
        {
            spawnedEndHouse.transform.position = endpos; ;
        }

		Vector2 firstStart = level[0].transform.Find("road_start_point").position;
		GameObject vplayer = Instantiate(VanObj, firstStart, Quaternion.Euler(0, 0, 180));

        if(MapManager.instance != null) {
			PlayerVan.vanTransform.GetComponent<Breakable>().hp = MapManager.instance.VanHealth;
			MapManager.instance.playersCurrentNode.WinCondition.startLevel();
		}

        SortWaypoints();


    }

    void GenerationLoop(List<int> questChunks) {

		float levelLengthSoFar = 0;
		Vector2 lastRoadEnd = Vector2.zero;

		for (int i = 0; i < levelSize; i++)
		{
			Chunk toSpawn;  //prefab
            bool removeEnemies = true;
            //this is slow but we can change chunkstospawnquestat to a hashset or something soon
            if(path.forcedSections != null) {
				if (path.forcedSections.Length > i)
				{
					toSpawn = path.forcedSections[i];
                    removeEnemies = false;
                }
                else {
					toSpawn = chunkBag[Random.Range(0, chunkBag.Length)];
				}
			}
            else if (questChunks.Contains(i))
            {
                toSpawn = QuestChunkBag[Random.Range(0, QuestChunkBag.Length)];
                toSpawn.setQuest(Quests[0]);
                Quests.RemoveAt(0);
            }
            else
            {
				toSpawn = chunkBag[Random.Range(0, chunkBag.Length)];
			}
		


			//Actually Spawn level
			toSpawn = Instantiate(toSpawn, transform); //new instance
			level[i] = toSpawn;

            if(removeEnemies) RemoveEnemies(i);


			//Aligning levels!

			//negative length of level
			levelLengthSoFar -= toSpawn.dimensions.x * 0.5f;
            Transform startPoint = toSpawn.transform.Find("road_start_point");
			Vector2 startpos = startPoint.position;
			float startRoadY = startpos.y;

			//Y displacement to correct for
			float yDiff = lastRoadEnd.y - startRoadY;

			//fix!
			toSpawn.transform.position = new Vector3(lastRoadEnd.x - startPoint.localPosition.x, yDiff, 0);
			levelLengthSoFar -= toSpawn.dimensions.x * 0.5f;

			//Save endpoint for next iteration of loop
			Vector2 endpos = toSpawn.transform.Find("road_end_point").position;
            lastRoadEnd = endpos;

			////boundaries
			//Debug.Log(i + " diff = " + yDiff);
			//GameObject boundary = Instantiate(boundaryStack, transform);
			//boundary.transform.position = new Vector3(levelLengthSoFar + toSpawn.dimensions.x * 0.5f, toSpawn.dimensions.y * 0.5f + yDiff);
			//boundary.transform.localScale = new Vector3(1, yDiff, 1);
			//boundary = Instantiate(boundaryStack, transform);
			//boundary.transform.position = new Vector3(levelLengthSoFar + toSpawn.dimensions.x * 0.5f, -toSpawn.dimensions.y * 0.5f + yDiff);
			//boundary.transform.localScale = new Vector3(1, yDiff, 1);
		}
	}
    void RemoveEnemies(int i) {
        Chunk toSpawn = level[i];
        if (singleChunkMode) return;
		//DO NOT SPAWN ENEMIES IN THE FIRST CHUNK
		if (i != 0 && toSpawn.enemies.Count > 0)
		{
			//Destroy Extras
			int overSpawn = toSpawn.enemies.Count - enemySpawnsRemaining;
			for (int e = 0; e < overSpawn; e++)
			{
				Destroy(toSpawn.enemies[0].gameObject);
				toSpawn.enemies.RemoveAt(0);
			}
			enemySpawnsRemaining -= toSpawn.enemies.Count;
			//the length of the list of toSpawn enemies is the number 'spawned' (not destroyed)
		}
		else
		{
			//Delete all enemies
			int overSpawn = toSpawn.enemies.Count;
			for (int e = 0; e < overSpawn; e++)
			{
				Destroy(toSpawn.enemies[0].gameObject);
				toSpawn.enemies.RemoveAt(0);
			}
			enemySpawnsRemaining -= toSpawn.enemies.Count;

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
