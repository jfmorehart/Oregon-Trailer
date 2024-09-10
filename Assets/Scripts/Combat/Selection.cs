using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Selection : MonoBehaviour
{
    public ContactFilter2D contactFilter;
    public Fighter selected;
    Vector2Int[] highlightedSquares;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            CombatGrid.Instance.ClearAllSquareHighlights();
            Vector3 testPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Collider2D[] results = Physics2D.OverlapPointAll(testPoint);
            if(results.Length > 0) {
				Deselect();
                highlightedSquares = null;
				for (int i = 0; i < results.Length; i++) {
                    if (results[i].gameObject.TryGetComponent(out Fighter fight)) {
                        selected = fight;
                        fight.Select();
                        highlightedSquares = CombatGrid.Instance.WalkableSquares(fight.gridPosition);
		            }
		        }
            }
            else {
                //missed the raycast, so lets try to get nearest gridsquare
                Vector2Int gpos = CombatGrid.Instance.WorldToGrid(testPoint);

                if (CombatGrid.IsValidSquare(gpos)) {
                    CombatGrid.Instance.ColorBox(gpos.x, gpos.y, Color.red);

                    if(highlightedSquares != null) {
						if (highlightedSquares.Contains(gpos) && selected.isPlayerTeam == CombatManager.Instance.isPlayersTurn)
						{
							Debug.Log("valid command input!");
                            selected.plannedMove = new CombatMove(selected, MoveType.Move, gpos, 0);
                            CombatManager.Instance.ProcessTurn();
						}
					}

		        }
                highlightedSquares = null;
	        }
		}
    }

    void Deselect() {
        if (selected == null) return;
		selected.Deselect();
		selected = null;
	}
}
