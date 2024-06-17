using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatNode
{
	public Vector2Int position;
	public Combatant occupant;

	public CombatNode(Vector2Int pos) {
		position = pos;
		occupant = null;
    }
}
