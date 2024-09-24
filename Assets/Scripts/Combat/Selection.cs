using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Selection : MonoBehaviour
{
    public ContactFilter2D contactFilter;
    public Fighter selected;
    Vector2Int[] highlightedSquares;

    MoveType moveTypeSelected;

    public GameObject moveSelectionMenu;
    public GameObject cancelButton;

	// Update is called once per frame
	void Update()
    {
        if(selected == null) {
			cancelButton.SetActive(false);
			moveSelectionMenu.SetActive(false);
		}
        else{

			if (!selected.isPlayerTeam) {
				cancelButton.SetActive(false);
				moveSelectionMenu.SetActive(false);
				return;
			}

			if (moveTypeSelected == MoveType.None)
			{
				moveSelectionMenu.SetActive(true);
				cancelButton.SetActive(false);
			}
			else
			{
				moveSelectionMenu.SetActive(false);
				cancelButton.SetActive(true);
			}
		}

        if (Input.GetMouseButtonDown(0)) {
			Debug.Log("click");
            CombatGrid.Instance.ClearAllSquareHighlights();
            Vector3 testPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);


			//Check for valid order execution

			Vector2Int gpos = CombatGrid.Instance.WorldToGrid(testPoint);

			if (!TryUnitSelect(testPoint)) {
				if (TryOrderComplete(gpos))
				{
					Deselect();
					highlightedSquares = null;
				}
			}

		}
    }
	bool TryOrderComplete(Vector2Int gpos) {
		if (!CombatGrid.IsValidSquare(gpos, false)) return false;

		//returns true if the 'gpos' square is a valid target for an existing order

		if (highlightedSquares != null)
		{
			if (highlightedSquares.Contains(gpos) && selected.isPlayerTeam == CombatManager.Instance.isPlayersTurn)
			{
				selected.plannedMove = new CombatMove(selected, moveTypeSelected, gpos, 1f);
				CombatManager.Instance.ProcessIndividualMove(selected);
				CombatGrid.Instance.ColorBox(gpos.x, gpos.y, Color.red);
				return true;
			}
			else {

				return false;
			}
		}
		else
		{
			Deselect();
			highlightedSquares = null;
			return false;
		}
	}
	bool TryUnitSelect(Vector2 testPoint) {

		//no valid order found on nearest square, try selecting a unit 
		Collider2D[] results = Physics2D.OverlapPointAll(testPoint);
		if (results.Length > 0)
		{
			for (int i = 0; i < results.Length; i++)
			{
				if (results[i].gameObject.TryGetComponent(out Fighter fight))
				{
					//we found a unit
					//are they the target of an order?
					if (TryOrderComplete(fight.gridPosition)) //execute the order
					{
						//yes
						Deselect();
						highlightedSquares = null;
						return true;
					}
					//no, they aren't
					//select them if you can
					if (!fight.isPlayerTeam) continue;
					if (fight.hasMoved) continue;
					selected = fight;
					fight.ToggleGreen(true);
					return true;
				}
			}
		}
		return false;
	}

    void Deselect() {
        if (selected == null) return;
		selected.ToggleGreen(false);
		moveTypeSelected = MoveType.None;
		selected = null;
		cancelButton.SetActive(false);
		moveSelectionMenu.SetActive(true);
	}

    public void SelectMoveType(int input) {
        moveTypeSelected = (MoveType)input;
		switch (moveTypeSelected) {
			case MoveType.None:
				break;
			case MoveType.Melee:
				highlightedSquares = CombatGrid.Instance.RangedAtkSquares(true, selected.gridPosition, true, 1);
				break;
			case MoveType.Move:
				highlightedSquares = CombatGrid.Instance.WalkableSquares(selected.gridPosition);
				break;
			case MoveType.RangedAtk:
				highlightedSquares = CombatGrid.Instance.RangedAtkSquares(true, selected.gridPosition);
				break;
			case MoveType.Heal:
				List<Fighter> fs = CombatGrid.Instance.GetAllFighters(false, true);
				Vector2Int[] poss = new Vector2Int[fs.Count];
				for (int i = 0; i < fs.Count; i++) {
					poss[i] = fs[i].gridPosition;
				}
				highlightedSquares = poss;
				break;

		}
		
	}
    public void CancelCurrentMoveSelection() {
        cancelButton.SetActive(false);
		moveSelectionMenu.SetActive(true);
		moveTypeSelected = MoveType.None;
    }
}
