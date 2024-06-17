using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CombatStatics
{
	public static CombatNode[] combatGrid;
	public static int turn;

	//Below are helper functions, just math stuff

	public static Vector2 GridToWorld(Vector2Int gridPos)
	{
		Vector2 world = new Vector2(
			gridPos.x * CombatManager.Instance.gridSpacer.x,
			gridPos.y * CombatManager.Instance.gridSpacer.y);
		return world - CombatManager.Instance.gridOriginOffset;
	}

	public static Vector2Int WorldToGrid(Vector2 world)
	{
		Vector2 interm = world + CombatManager.Instance.gridOriginOffset;
		interm.x /= CombatManager.Instance.gridSpacer.x;
		interm.y /= CombatManager.Instance.gridSpacer.y;
		Vector2Int grid = new Vector2Int(
			Mathf.RoundToInt(interm.x),
			Mathf.RoundToInt(interm.y));
		grid.x = Mathf.Max(0, grid.x);
		grid.x = Mathf.Min(CombatManager.Instance.grid_size_x - 1, grid.x);
		grid.y = Mathf.Max(0, grid.y);
		grid.y = Mathf.Min(CombatManager.Instance.grid_size_y - 1, grid.y);
		return grid;
	}

	public static float DistanceToMouse(CombatNode gridNode)
	{
		Vector2 wpos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		return Vector2.Distance(wpos, GridToWorld(gridNode.position));
	}

	public static CombatNode NodeAtPosition(Vector2Int pos)
	{
		return combatGrid[IndexOfGridNode(pos)];
	}

	public static int IndexOfGridNode(Vector2Int position)
	{
		return position.x + position.y * CombatManager.Instance.grid_size_x;
	}
	public static int IndexOfGridNode(CombatNode gridn)
	{
		return IndexOfGridNode(gridn.position);
	}
}
