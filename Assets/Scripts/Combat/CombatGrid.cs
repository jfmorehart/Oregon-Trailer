using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatGrid : MonoBehaviour
{
	public static CombatGrid Instance;

    public GameObject prefab;
    public Vector2 offset, spacer;
    public static Vector2Int gsize = new Vector2Int(9, 5);
	public float tiltSpacer;

	[HideInInspector]
    public Fighter[] grid;
	[HideInInspector]
	public SpriteRenderer[] boxes;

	private void Awake()
	{
		Instance = this;
		MakeGrid();

		Vector2 random = boxes[Random.Range(0, boxes.Length)].transform.position;
		Debug.Log(random + " " + WorldToGrid(random) + " " + GridToWorld(WorldToGrid(random)));
	}

	private void Update()
	{
        if (Input.GetKeyDown(KeyCode.Space)) {
            ClearGrid();
			MakeGrid();
	    }
	}
    void MakeGrid() {
		grid = new Fighter[gsize.x * gsize.y];
		boxes = new SpriteRenderer[gsize.x * gsize.y];
		for (int x = 0; x < gsize.x; x++)
		{
			for (int y = 0; y < gsize.y; y++)
			{
				Vector2 wpos = GridToWorld(x, y);
				boxes[y * gsize.x + x] = Instantiate(prefab, wpos, Quaternion.identity, transform).GetComponent<SpriteRenderer>();
				grid[y * gsize.x + x] = null;
			}
		}
	}
    void ClearGrid() {
        for(int i = 0; i < grid.Length; i++) {
            Destroy(grid[i]);
		}
    }

	public Vector2Int[] RangedAtkSquares(bool shooterteam, Vector2Int startPos, bool highlight = true, int range = 100) {
		List<Vector2Int> poss = GetPositionsOfTeam(!shooterteam);
		List<Vector2Int> validTargets = new List<Vector2Int>();
		foreach(Vector2Int sqr in poss) {

			if(range < 99) { 
				//rangecheck
				if(Vector2Int.Distance(startPos, sqr) > range) {
					continue;
				}
			}
			if (highlight) ColorBox(sqr.x, sqr.y, Color.green);
			validTargets.Add(sqr);
		}
		return poss.ToArray();
	}

	public Vector2Int[] WalkableSquares(Vector2Int startPos, bool highlight = true)
	{
		List<Vector2Int> walkables = new List<Vector2Int>();
		for (int i = 0; i < 9; i++)
		{
			Vector2Int testPos = startPos + new Vector2Int((i % 3) - 1, Mathf.FloorToInt(i / 3) - 1);
			//Debug.Log(i + " " + new Vector2Int((i % 3) - 1, Mathf.FloorToInt(i / 3) - 1));
			if (Vector2Int.Distance(startPos, testPos) < 0.5f) continue;
			if (IsValidSquare(testPos))
			{
				if (highlight) ColorBox(testPos.x, testPos.y, Color.green);
				walkables.Add(testPos);
			}
		}
		return walkables.ToArray();
	}

	public List<Vector2Int> GetPositionsOfTeam(bool ofTeam)
	{
		List<Vector2Int> poss = new List<Vector2Int>();
		foreach (Fighter f in GetAllFighters(false, ofTeam))
		{
			poss.Add(f.gridPosition);
		}
		return poss;
	}


	public List<Fighter> GetAllFighters(bool all = false, bool teamOf = true) {
		List<Fighter> fighters = new List<Fighter>();
		for(int i = 0; i < grid.Length; i++) {
			if (grid[i] == null) continue;
			if (all) {
				fighters.Add(grid[i]);
				continue;
			}
			if (grid[i].isPlayerTeam == teamOf) {
				fighters.Add(grid[i]);
			}
		}
		return fighters;
    }

    public Fighter FighterAtPosition(Vector2Int pos){
		return grid[GridCoordinateToIndex(pos)];
	}


	public Vector2 GridToWorld(Vector2Int gridPos) {
        Vector2 world = new Vector2(gridPos.x - Mathf.RoundToInt(gsize.x * 0.5f), gridPos.y - Mathf.RoundToInt(gsize.y * 0.5f));
		world.x *= spacer.x;
		world.x += gridPos.y * spacer.x * tiltSpacer;
		world.y *= spacer.y;
        world -= offset;
        return world;
    }

	public Vector2Int WorldToGrid(Vector2 worldPos) {
		Vector2 unrounded = worldPos += offset;
		unrounded.y /= spacer.y;
		unrounded.y += Mathf.RoundToInt(gsize.y * 0.5f);
		unrounded.x -= unrounded.y * spacer.x * tiltSpacer;
		unrounded.x /= spacer.x;
		unrounded.x += Mathf.RoundToInt(gsize.x * 0.5f);
		return new Vector2Int(Mathf.RoundToInt(unrounded.x), Mathf.RoundToInt(unrounded.y));
	}


	public void ClearAllSquareHighlights() { 
		foreach(SpriteRenderer spr in boxes) {
			spr.color = Color.clear;
		}
    }

	public void ColorBox(int x, int y, Color c) {
		boxes[y * gsize.x + x].color = c;
	}
	public Vector2 GridToWorld(int x, int y)
	{
		return GridToWorld(new Vector2Int(x, y));
	}

	public int GridCoordinateToIndex(int x, int y) {
		return y * gsize.x + x;
	}
	public int GridCoordinateToIndex(Vector2Int pt)
	{
		return pt.y * gsize.x + pt.x;
	}

	public static bool IsValidSquare(Vector2Int dest, bool invalidIfOccupied = true) {
		if (dest.x < 0) return false;
		if (dest.y < 0) return false;
		if (dest.x >= gsize.x) return false;
		if (dest.y >= gsize.y) return false;
		if (Instance.grid[Instance.GridCoordinateToIndex(dest)] != null && invalidIfOccupied) return false;
		return true;
	}

	public static bool HasValidTarget(Vector2Int dest, bool ofTeam) {
		Fighter f = Instance.grid[Instance.GridCoordinateToIndex(dest)];
		if (f == null) return false;
		if (f.isPlayerTeam != ofTeam) return false;
		return true;
	}

	public static Vector2Int RandomGridSquare() {
		return new Vector2Int(Random.Range(0, gsize.x), Random.Range(0, gsize.y));
    }
	public static Vector2Int RandomEmptyGridSquare()
	{
		Vector2Int pos = Vector2Int.zero;
		int tries = 0;
		do
		{
			tries++;
			if (tries > 500) return Vector2Int.zero;
			pos = new Vector2Int(Random.Range(0, gsize.x), Random.Range(0, gsize.y));
		}
		while (Instance.grid[Instance.GridCoordinateToIndex(pos)] != null);
		return pos;
	}
	public static Vector2Int RandomEmptyGridSquare(int xmin, int xmax)
	{
		Vector2Int pos = Vector2Int.zero;
		int tries = 0;
		do
		{
			tries++;
			if (tries > 500) return Vector2Int.zero;
			pos = new Vector2Int(Random.Range(xmin, Mathf.Min(gsize.x, xmax)), Random.Range(0, gsize.y));
		}
		while (Instance.grid[Instance.GridCoordinateToIndex(pos)] != null);
		return pos;
	}
	public static Vector2Int RandomWalk(Vector2Int origin) {
		bool validDest;
		Vector2Int dest;
		int tries = 0;
		do {
			tries++;
			validDest = true;
			dest = origin + new Vector2Int(Random.Range(-1, 2), Random.Range(-1, 2));
			validDest = IsValidSquare(dest);
			if (Vector2Int.Distance(dest, origin) < 0.1f) validDest = false;
			if (tries > 500)
			{
				Debug.Log(tries + " tries");
				return origin;
			}
		} while (!validDest);
		return dest;
	}
}
