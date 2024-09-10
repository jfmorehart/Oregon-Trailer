using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatGrid : MonoBehaviour
{
	public static CombatGrid Instance;

    public GameObject prefab;
    public Vector2 offset, spacer;
    public static Vector2Int gsize = new Vector2Int(6, 3);
	public float tiltSpacer;
    public Fighter[] grid;
	public SpriteRenderer[] boxes;

	private void Awake()
	{
		Instance = this;
		MakeGrid();
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

	public void HighlightWalkableSquares(Vector2Int startPos) { 
		for(int i = 0; i < 9; i++) {
			Vector2Int testPos = startPos + new Vector2Int((i % 3) - 1, Mathf.FloorToInt(i / 3) - 1);
			Debug.Log(i + " " + new Vector2Int((i % 3) - 1, Mathf.FloorToInt(i / 3) - 1));
			if (Vector2Int.Distance(startPos, testPos) < 0.5f) continue;
			if (IsValidSquare(testPos)) {
				ColorBox(testPos.x, testPos.y, Color.green);
			}
		}
    }

	public void ClearAllSquareHighlights() { 
		foreach(SpriteRenderer spr in boxes) {
			spr.color = Color.white;
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

	public static bool IsValidSquare(Vector2Int dest) {
		if (dest.x < 0) return false;
		if (dest.y < 0) return false;
		if (dest.x >= gsize.x) return false;
		if (dest.y >= gsize.y) return false;
		if (Instance.grid[Instance.GridCoordinateToIndex(dest)] != null) return false;
		return true;
	}

	public static Vector2Int RandomGridSquare() {
		return new Vector2Int(Random.Range(0, gsize.x), Random.Range(0, gsize.y));
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
