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

	public float baseAtk;

	public void Setup(Vector2Int gpos, Character myself) {
		Debug.Log(Vector2Int.Distance(Vector2Int.zero, Vector2Int.one));
		me = myself;
		charSprite.sprite = me.baseCharacter.combat_sprite;
		if (isPlayerTeam) {
			charSprite.flipX = me.baseCharacter.isFacingRight;
		}
		else {
			charSprite.flipX = !me.baseCharacter.isFacingRight;
		}
		if (!CombatManager.Instance.playerTeamOnRight) {
			charSprite.flipX = !charSprite.flipX;
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
				Debug.Log("new turn registered " + gridPosition);
				Vector2Int[] meleeSquares = CombatGrid.Instance.RangedAtkSquares(isPlayerTeam, gridPosition, false, 1.6f);
				if(meleeSquares.Length > 0) {
					plannedMove = new CombatMove(this, MoveType.Melee, meleeSquares[Random.Range(0, meleeSquares.Length)], baseAtk);
				}
				else {
					plannedMove = new CombatMove(this, MoveType.Move, CombatGrid.RandomWalk(gridPosition, CombatManager.Instance.playerTeamOnRight? 1: -1), 1);
				}
			}
		}
    }

	public void MoveProcessed() {
		StartCoroutine(Wiggle(0.25f));
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
		StartCoroutine(LerpTo(0.1f));
		//transform.position = CombatGrid.Instance.GridToWorld(gridPosition);
		charSprite.sortingOrder = 10 + (CombatGrid.gsize.y - gridPosition.y);
	}

	public IEnumerator LerpTo(float lerpTime) {
		Vector3 initPos = transform.position;
		float lerpVal = 0;
		while (lerpVal < lerpTime) {
			lerpVal += Time.deltaTime;
			transform.position = Vector3.Lerp(initPos, CombatGrid.Instance.GridToWorld(gridPosition), lerpVal / lerpTime);
			yield return new WaitForEndOfFrame();
		}
    }
	public IEnumerator Wiggle(float duration)
	{
		float atime = 0;
		float freq = 0.5f;
		float amp = 0.2f;
		float init_amp = amp;
		while (atime < duration)
		{
			atime += Time.deltaTime;
			amp = init_amp * (duration / atime + 0.01f);
			transform.position = amp * new Vector3(Mathf.PerlinNoise1D(Time.time * freq), Mathf.PerlinNoise1D(freq * Time.time + 0.5f), 0);
			yield return new WaitForEndOfFrame();
		}
	}
}
