using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance;

    public bool isPlayersTurn;

    public List<Fighter> fighters;

    public TMP_Text turnText;

	public static Action<bool> NewTurn;

	private void Awake()
	{
        Instance = this;
		isPlayersTurn = false;
	}
	private void Start()
	{
		ProcessTurn();
	}

	public void ProcessTurn() {
		//used for simultaneous moves
        foreach(Fighter h in fighters) { 
            if(isPlayersTurn == h.isPlayerTeam) {
                h.plannedMove.Execute();
			}
	    }
        isPlayersTurn = !isPlayersTurn;
		NewTurn?.Invoke(isPlayersTurn); //tell everyone

		if (isPlayersTurn) {
            turnText.text = "your turn!";
            turnText.color = Color.green;
        }
        else {
			turnText.text = "enemy turn!";
			turnText.color = Color.red;
		}
    }

    public void ProcessIndividualMove(Fighter f) {

		f.plannedMove.Execute();

		foreach (Fighter h in fighters)
		{
			if (isPlayersTurn == h.isPlayerTeam)
			{
				if (!h.hasMoved) return;
			}
		}
		//all units have moved
		isPlayersTurn = !isPlayersTurn;
		NewTurn?.Invoke(isPlayersTurn); //tell everyone
		if (isPlayersTurn)
		{
			turnText.text = "your turn!";
			turnText.color = Color.green;
		}
		else
		{
			turnText.text = "enemy turn!";
			turnText.color = Color.red;
			ProcessTurn();
		}
	}
}
