using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selection : MonoBehaviour
{
    public ContactFilter2D contactFilter;
    public Fighter selected;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            CombatGrid.Instance.ClearAllSquareHighlights();
            Deselect();
            Vector3 testPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Collider2D[] results = Physics2D.OverlapPointAll(testPoint);
            if(results.Length > 0) { 
	            for(int i = 0; i < results.Length; i++) {
                    if (results[i].gameObject.TryGetComponent(out Fighter fight)) {
                        selected = fight;
                        fight.Select();
                        CombatGrid.Instance.HighlightWalkableSquares(fight.gridPosition);
		            }
		        }
	        }
		}
    }

    void Deselect() {
        if (selected == null) return;
		selected.Deselect();
		selected = null;
	}
}
