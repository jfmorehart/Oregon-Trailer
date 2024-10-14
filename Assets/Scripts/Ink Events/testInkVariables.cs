using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using Ink.UnityIntegration;

public class testInkVariables : MonoBehaviour
{
    public static testInkVariables instance;
    [SerializeField]
    public SpriteRenderer sr;
    string variableCheck = "puppy_kicked";
    [SerializeField]
    private Color defaultColor = Color.white;
    [SerializeField]
    private Color puppyKickedcolor = Color.red;
    [SerializeField]
    private Color puppyAvoidedcolor= Color.blue;


    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    public void UpdateChoice()
    {
        string playerchoice = ((Ink.Runtime.StringValue) centralEventHandler.instance.getVariableState(variableCheck)).value;
        Debug.Log("Updating choice - " + playerchoice + " ----" + 
            ((Ink.Runtime.StringValue)centralEventHandler.instance.getVariableState(variableCheck)).value);
        switch (playerchoice)
        {
            case "":
                Debug.Log("Ink value is empty");
                break;
            case "kicked":
                sr.color = puppyKickedcolor;
                Debug.Log("Color changed to " + puppyKickedcolor.r);
                break;
            case "avoided":
                sr.color = puppyAvoidedcolor;
                Debug.Log("Color changed" + puppyAvoidedcolor.r);
                break;
            default:
                Debug.Log("Player choice not handled by switch statement");
                break;
        }
    }
}
