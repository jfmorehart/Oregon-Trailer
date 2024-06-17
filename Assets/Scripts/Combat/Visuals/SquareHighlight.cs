using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareHighlight : MonoBehaviour
{
  
	public void Select() {
        GetComponent<SpriteRenderer>().color = Color.green;
    }

	public void Deselect()
	{
		GetComponent<SpriteRenderer>().color = Color.gray;
	}

}
