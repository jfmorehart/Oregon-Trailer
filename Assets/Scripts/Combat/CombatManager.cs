using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance;

    public bool isPlayersTurn;

    public List<Fighter> fighters;

	private void Awake()
	{
        Instance = this;
	}
	public void ProcessTurn() {
        Debug.Log("processing");
        foreach(Fighter h in fighters) { 
            if(isPlayersTurn == h.isPlayerTeam) {
                h.plannedMove.Execute();
			}
	    }
        isPlayersTurn = !isPlayersTurn;
    }
}
