using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIToggle : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private bool state;
    [SerializeField]
    private GameObject targetObject;

    //animate hover
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        Debug.Log("entered");
        transform.Rotate(0f, 0f, 5f, Space.Self);
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        transform.Rotate(0f, 0f, -5f, Space.Self);
    }
    
    public void Toggle()
    {
        //toggleBool
        state = !state;
        
        //go through sprite list
        targetObject.SetActive(state);
    }

}
