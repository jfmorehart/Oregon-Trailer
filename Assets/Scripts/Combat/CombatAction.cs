using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CombatAction
{
	public enum ActionType
	{
		idle,
		move,
		normalAttack,
		specialAttack,
		heal,
		useItem
	}
	public ActionType actionType;
	public Vector2Int targetSquare;

}
