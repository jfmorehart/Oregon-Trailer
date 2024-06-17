using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopupWindow : MonoBehaviour
{
    public TMP_Text tmp;
    // Update is called once per frame
    void Update()
    {
        if (CombatManager.Instance.popUpActive) { 
            transform.position = Input.mousePosition;
	    }
    }
}
