using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance;

    public bool isPlayersTurn;

    public List<Fighter> fighters;

    public TMP_Text turnText;

	public GameObject endpanel;
	public TMP_Text endpanelText;

	public static Action<bool> NewTurn;

	public GameObject friendly;
	public GameObject enemy;

	public CharacterBase gatorBase;
	public CharacterBase medicBase;
	public Character medic;
	public Character gatorHead;

	private void Awake()
	{
        Instance = this;
		isPlayersTurn = false;
		gatorHead = new Character(gatorBase);
		medic = new Character(medicBase);
		if (CurrentGame.activeParty == null) {
			CurrentGame.NewParty(medic, medic);
		}
		if (CombatantsStatic.combatants == null)
		{
			CombatantsStatic.combatants = new Character[2];
			CombatantsStatic.combatants[0] = gatorHead;
			CombatantsStatic.combatants[1] = gatorHead;
		}
	}
	private void Start()
	{
		Party party = CurrentGame.activeParty;
		for (int i =0; i < party.members.Count; i++) {
			GameObject go = Instantiate(friendly);
			Fighter f = go.GetComponent<Fighter>();
			Vector2Int pos = CombatGrid.RandomEmptyGridSquare(4, 10);
			f.Setup(pos, party.members[i]);
		}
		for (int i = 0; i < CombatantsStatic.combatants.Length; i++)
		{
			GameObject go = Instantiate(enemy);
			Fighter f = go.GetComponent<Fighter>();
			Vector2Int pos = CombatGrid.RandomEmptyGridSquare(0, 5);
			f.Setup(pos, CombatantsStatic.combatants[i]);
		}
		ProcessTurn();
	}

	public void ProcessTurn() {
		//used for simultaneous moves
        foreach(Fighter h in fighters) { 
            if(isPlayersTurn == h.isPlayerTeam) {
                h.plannedMove.Execute();
			}
	    }
		if (WLCheck()) return;
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
	public bool WLCheck() {
		//checks if the player has won or lost
		bool teamAremains = false;
		bool teamBremains = false;

		foreach (Fighter h in fighters)
		{
			if (h.isPlayerTeam) {
				teamAremains = true;
			}
			else {
				teamBremains = true;
			}
			if(teamAremains && teamBremains) {
				return false;
			}	
		}
		if (teamAremains) {
			Debug.Log("Victory to team A");
			endpanel.SetActive(true);
			endpanelText.text = "VICTORY";
		}
		if (teamBremains) {
			Debug.Log("Victory to team B");
			endpanelText.text = "DEFEAT";
		}
		return true;
	}
    public void ProcessIndividualMove(Fighter f) {

		f.plannedMove.Execute();
		WLCheck();

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
