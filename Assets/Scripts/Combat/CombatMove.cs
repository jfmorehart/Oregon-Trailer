using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[System.Serializable]
public enum MoveType {
	None,
	Melee,
	RangedAtk,
	Move,
	Heal,
	Special,
}
[System.Serializable]
public struct CombatMove {
	public Fighter mover;
	public MoveType moveType;
	public Vector2Int targetSquare;
	public float power;

	public CombatMove(Fighter attacker, MoveType move, Vector2Int toSquare, float str) {
		mover = attacker;
		moveType = move;
		targetSquare = toSquare;
		power = str;
    }

	public void Execute()
	{
		if (mover == null) return;

		Fighter target;
		target = CombatGrid.Instance.grid[CombatGrid.Instance.GridCoordinateToIndex(targetSquare)];
		bool validTarget = (target != null) && (target.isPlayerTeam != mover.isPlayerTeam);
		switch (moveType)
		{
			case MoveType.None:
				Debug.Log("idle");
				break;
			case MoveType.Melee:
				if(!validTarget)break;
				target.TakeDamage(power * 2);
				break;
			case MoveType.RangedAtk:
				if (!validTarget) break;
				target.TakeDamage(power);
				break;
			case MoveType.Move:
				if (validTarget) break;//cant move there if theres someone there
				mover.MoveTo(targetSquare);
				break;
			case MoveType.Heal:
				mover.hp.hp += power;
				break;
		}
		mover.MoveProcessed();
		Debug.Log("executed");
	}
}

