using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour, IDamageable
{
    public int hP;
	public bool isPlayerTeam;
	public Vector2Int gridPosition;
	public Vector2Int targetPosition;
	public CombatMove plannedMove;

	public SpriteRenderer selectedBubble;

	private void Start()
	{
		RegisterOnGrid();
		MoveTo(gridPosition);
		Deselect();
	}

	private void Update()
	{
		//todo replace with requiring player input
		if(isPlayerTeam == CombatManager.Instance.isPlayersTurn) { 
			if(plannedMove.moveType == MoveType.None) {
				plannedMove = new CombatMove(this, MoveType.Move, CombatGrid.RandomWalk(gridPosition), 1);
			}
		}
	}

	public void RegisterOnGrid(){
		CombatManager.Instance.fighters.Add(this);
		CombatGrid.Instance.grid[CombatGrid.Instance.GridCoordinateToIndex(gridPosition)] = this;
	}

	public void Select() {
		if (selectedBubble != null) selectedBubble.enabled = true;
	}

	public void Deselect() {
		if (selectedBubble != null) selectedBubble.enabled = false;
	}

	public void TakeDamage(int dmg)
	{

	}

	public void MoveProcessed() {
		plannedMove = new CombatMove(this, MoveType.None, Vector2Int.zero, 0);
    }
	public void MoveTo(Vector2Int square) {
		Debug.Log("moving to " + square);
		CombatGrid.Instance.grid[CombatGrid.Instance.GridCoordinateToIndex(gridPosition)] = null;
		gridPosition = square;
		CombatGrid.Instance.grid[CombatGrid.Instance.GridCoordinateToIndex(gridPosition)] = this;
		transform.position = CombatGrid.Instance.GridToWorld(gridPosition);
	}
}
