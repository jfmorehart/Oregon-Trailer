using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[HideInInspector]
public class Fighter : MonoBehaviour, IDamageable
{
	public Character me; //character who this instance represents

	public bool isPlayerTeam;
	public bool hasMoved;

	public Vector2Int gridPosition;
	public Vector2Int targetPosition;
	public CombatMove plannedMove;
	

	public SpriteRenderer selectedBubble;
	public SpriteRenderer charSprite;

	public HealthBar hp;

	public void Setup(Vector2Int gpos, Character myself) {
		me = myself;
		charSprite.sprite = me.baseCharacter.combat_sprite;
		if (isPlayerTeam) {
			charSprite.flipX = me.baseCharacter.isFacingRight;
		}
		else {
			charSprite.flipX = !me.baseCharacter.isFacingRight;
		}

		CombatManager.NewTurn += NewTurn; // remember to unsubscribe before death
		UnHighlight();
		gridPosition = gpos;
		RegisterOnGrid();
	}

	public void RegisterOnGrid(){
		CombatManager.Instance.fighters.Add(this);
		CombatGrid.Instance.grid[CombatGrid.Instance.GridCoordinateToIndex(gridPosition)] = this;
		MoveTo(gridPosition);
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
		if(hp.hp < 0.01f) {
			Kill();
		}
	}
	public void Kill() {
		CombatManager.Instance.fighters.Remove(this);
		CombatGrid.Instance.grid[CombatGrid.Instance.GridCoordinateToIndex(gridPosition)] = null;
		Destroy(gameObject);
    }

	public void NewTurn(bool team) {
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
		if (CombatGrid.Instance.grid[CombatGrid.Instance.GridCoordinateToIndex(square)] != null) {
			//already occupied
			CombatGrid.Instance.grid[CombatGrid.Instance.GridCoordinateToIndex(gridPosition)] = this;
			transform.position = CombatGrid.Instance.GridToWorld(gridPosition);
			charSprite.sortingOrder = 10 + (CombatGrid.gsize.y - gridPosition.y);
			Debug.Log(gridPosition + " blocked to " + square);
			return;
		}

		Debug.Log("moving to " + square);
		CombatGrid.Instance.grid[CombatGrid.Instance.GridCoordinateToIndex(gridPosition)] = null;
		gridPosition = square;
		CombatGrid.Instance.grid[CombatGrid.Instance.GridCoordinateToIndex(gridPosition)] = this;
		transform.position = CombatGrid.Instance.GridToWorld(gridPosition);
		charSprite.sortingOrder = 10 + (CombatGrid.gsize.y - gridPosition.y);
	}
}
