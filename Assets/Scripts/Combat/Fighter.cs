using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour, IDamageable
{
    public int hP;
	public bool isPlayerTeam;
	public bool hasMoved;

	public Vector2Int gridPosition;
	public Vector2Int targetPosition;
	public CombatMove plannedMove;
	

	public SpriteRenderer selectedBubble;
	public SpriteRenderer charSprite;

	public HealthBar hp;

	private void Awake()
	{
		CombatManager.NewTurn += NewTurn; //unsubscribe before death
		UnHighlight();
	}
	private void Start()
	{
		RegisterOnGrid();
		MoveTo(gridPosition);
	}

	private void Update()
	{
		//todo replace with requiring player input
		//if(isPlayerTeam == CombatManager.Instance.isPlayersTurn) { 
		//	if(plannedMove.moveType == MoveType.None) {
		//		plannedMove = new CombatMove(this, MoveType.Move, CombatGrid.RandomWalk(gridPosition), 1);
		//	}
		//}
	}

	public void RegisterOnGrid(){
		CombatManager.Instance.fighters.Add(this);
		CombatGrid.Instance.grid[CombatGrid.Instance.GridCoordinateToIndex(gridPosition)] = this;
	}

	public void ToggleGreen(bool green) {
		if (selectedBubble != null) selectedBubble.color = green ? Color.green : Color.white;
	}
	public void Highlight() {
		if (selectedBubble != null) selectedBubble.enabled = true;
	}

	public void UnHighlight() {
		if (selectedBubble != null) selectedBubble.enabled = false;
	}

	public void TakeDamage(float dmg)
	{
		Debug.Log(gameObject.name + " took " + dmg + " damage");
		hp.hp -= dmg;
	}

	public void NewTurn(bool team) {
		Debug.Log("newturn  " + team);
		if(team == isPlayerTeam) {
			hasMoved = false;
			Highlight();
			if (!isPlayerTeam) {
				plannedMove = new CombatMove(this, MoveType.Move, CombatGrid.RandomWalk(gridPosition), 1);
			}
		}
    }

	public void MoveProcessed() {
		plannedMove = new CombatMove(this, MoveType.None, Vector2Int.zero, 0);
		hasMoved = true;
		UnHighlight();
    }
	public void MoveTo(Vector2Int square) {
		Debug.Log("moving to " + square);
		CombatGrid.Instance.grid[CombatGrid.Instance.GridCoordinateToIndex(gridPosition)] = null;
		gridPosition = square;
		CombatGrid.Instance.grid[CombatGrid.Instance.GridCoordinateToIndex(gridPosition)] = this;
		transform.position = CombatGrid.Instance.GridToWorld(gridPosition);
		charSprite.sortingOrder = 10 + (CombatGrid.gsize.y - gridPosition.y);
	}
}
