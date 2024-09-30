using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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

	public CharacterBase punkBase;
	public Character punk;

	public bool playerTeamOnRight;

	private void Awake()
	{
		Instance = this;
		isPlayersTurn = false;
		gatorHead = new Character(gatorBase);
		medic = new Character(medicBase);
		punk = new Character(punkBase);
		if (CurrentGame.activeParty == null)
		{
			CurrentGame.NewParty(gatorHead, medic);
		}
		if (CombatantsStatic.combatants == null)
		{
			CombatantsStatic.combatants = new Character[3];
			CombatantsStatic.combatants[0] = punk;
			CombatantsStatic.combatants[1] = punk;
			CombatantsStatic.combatants[2] = punk;
		}
	}
	private void Start()
	{
		Party party = CurrentGame.activeParty;
		for (int i = 0; i < party.members.Count; i++)
		{
			GameObject go = Instantiate(friendly);
			Fighter f = go.GetComponent<Fighter>();

			Vector2Int pos;
			if (playerTeamOnRight) { 
				pos = CombatGrid.RandomEmptyGridSquare(CombatGrid.gsize.x / 2, CombatGrid.gsize.x);
			}
			else {
				pos = CombatGrid.RandomEmptyGridSquare(0, CombatGrid.gsize.x / 2);
			}
			
			f.Setup(pos, party.members[i]);
		}
		for (int i = 0; i < CombatantsStatic.combatants.Length; i++)
		{
			GameObject go = Instantiate(enemy);
			Fighter f = go.GetComponent<Fighter>();
			Vector2Int pos;
			if (playerTeamOnRight)
			{
				pos = CombatGrid.RandomEmptyGridSquare(0, CombatGrid.gsize.x / 2);
			}
			else {
				pos = CombatGrid.RandomEmptyGridSquare(CombatGrid.gsize.x / 2, CombatGrid.gsize.x);
			}
			
			f.Setup(pos, CombatantsStatic.combatants[i]);
		}
		ProcessTurn();
	}

	public void ProcessTurn()
	{
		//used for simultaneous moves

		//these moves may kill some 
		List<Fighter> team = new List<Fighter>();
		foreach (Fighter h in fighters)
		{
			if (isPlayersTurn == h.isPlayerTeam)
			{
				team.Add(h);
			}
		}
		foreach(Fighter h in team) { 
			h.plannedMove.Execute();
		}

		if (WLCheck()) return;
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
		}
	}
	public bool WLCheck()
	{
		//checks if the player has won or lost
		bool teamAremains = false;
		bool teamBremains = false;

		foreach (Fighter h in fighters)
		{
			if (h.isPlayerTeam)
			{
				teamAremains = true;
			}
			else
			{
				teamBremains = true;
			}
			if (teamAremains && teamBremains)
			{
				return false;
			}
		}
		if (teamAremains)
		{
			Debug.Log("Victory to team A");
			endpanel.SetActive(true);
			endpanelText.text = "VICTORY";
		}
		if (teamBremains)
		{
			Debug.Log("Victory to team B");
			endpanelText.text = "DEFEAT";
			endpanel.SetActive(true);
			if(SaveManager.instance != null)
				SaveManager.instance.playerDied();
		}
		return true;
	}
	public void ProcessIndividualMove(Fighter f)
	{

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

	public void ExitCombat()
	{
		SceneManager.LoadScene(0);
	}
}
