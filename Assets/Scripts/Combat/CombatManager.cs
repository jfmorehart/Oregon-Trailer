using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CombatStatics;


public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance;

	public int grid_size_x;
	public int grid_size_y;

	public Vector2 gridSpacer;
	public float perspSpacer;

	[HideInInspector]
	public Vector2 gridOriginOffset;

	public GameObject node_test;

	[HideInInspector]
	public SquareHighlight[] highlights;

	public float minPopupDist;

	public PopupWindow popup;
	public bool popUpActive;

	public int selectedNode = -1;
		
	private void Awake()
	{
		if(Instance != null && Instance != this) {
			Destroy(this);
		}
		else {
			Instance = this;
		}

		CreateGrid();
	}
	public void CreateGrid() {
		gridOriginOffset.x = gridSpacer.x * (grid_size_x - 1) * 0.5f;
		gridOriginOffset.y = gridSpacer.y * (grid_size_y - 1) * 0.5f;

		combatGrid = new CombatNode[grid_size_x * grid_size_y];
		highlights = new SquareHighlight[grid_size_x * grid_size_y];
		for (int i = 0; i < combatGrid.Length; i++)
		{
			Vector2Int nodeGridPos = new Vector2Int(
				i % grid_size_x,
				Mathf.FloorToInt(i / grid_size_x));
			combatGrid[i] = new CombatNode(nodeGridPos);

			GameObject g = Instantiate(node_test, GridToWorld(nodeGridPos), Quaternion.identity, transform);
			//g.transform.localScale = gridSpacer * 0.9f;
			g.name = i.ToString();
			highlights[i] = g.GetComponent<SquareHighlight>();
		}
	}

	private void Update()
	{
		//handling the little popup that follows the mouse
		Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		CombatNode nearestNodeToCursor = NodeAtPosition(WorldToGrid(mouseWorldPosition));
		if (DistanceToMouse(nearestNodeToCursor) < minPopupDist) {
			if (!popUpActive) {
				popUpActive = true;
				popup.gameObject.SetActive(true);
				if(nearestNodeToCursor.occupant != null) {
					popup.tmp.text = nearestNodeToCursor.occupant.name;
				}
				else {
					popup.tmp.text = "Empty";
				}

			}
			else {
				if (Input.GetMouseButtonDown(0)) {
					if (selectedNode != -1) {
						highlights[selectedNode].Deselect();
					}
					selectedNode = IndexOfGridNode(nearestNodeToCursor);
					highlights[selectedNode].Select();

					if (nearestNodeToCursor.occupant != null)
					{
						ActionsWindow.Instance.nameText.text = nearestNodeToCursor.occupant.name;
						ActionsWindow.Instance.OptionsMenu.SetActive(true);
					}
					else
					{
						ActionsWindow.Instance.OptionsMenu.SetActive(false);
						ActionsWindow.Instance.nameText.text = "Empty";
					}
				}
			}
		}
		else {
			if (popUpActive)
			{
				popUpActive = false;
				popup.gameObject.SetActive(false);
			}
		}

		if (Input.GetKeyDown(KeyCode.Escape)) {
			if (selectedNode != -1)
			{
				highlights[selectedNode].Deselect();
			}
			selectedNode = -1;
		}
	}

	public void NextTurn() {
		//process all logic 
		for (int i = 0; i < combatGrid.Length; i++) {
			if (combatGrid[i].occupant != null) {
				ExecuteAction(combatGrid[i]);
			}
		}

		//increment turn
		turn++;
    }

	void ExecuteAction(CombatNode node) {
		// called once for each valid combatant at the action phase of each turn

		// This function attempts to execute whatever action each combatant has
		// said they would like to execute

		Combatant comb = node.occupant;
		CombatNode targetNode = NodeAtPosition(comb.desiredAction.targetSquare);
		switch (comb.desiredAction.actionType) {
			case CombatAction.ActionType.idle:
				break;
			case CombatAction.ActionType.move:
				node.occupant = targetNode.occupant;
				targetNode.occupant = comb;
				comb.MoveTo(GridToWorld(targetNode.position));
				break;
			case CombatAction.ActionType.normalAttack:
				targetNode.occupant.Hit(1); //todo implement weapons
				break;
			case CombatAction.ActionType.specialAttack:
				targetNode.occupant.Hit(2); //todo implement specials
				break;
			case CombatAction.ActionType.useItem:
				//todo implement items or whatever
				break;
		}
	}
}
