using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObjectivePopUp : MonoBehaviour
{
    //string to display
    public string quest;
    public TMP_Text questDisplay;

    //Animations
    public AnimationCurve opacityCurve;
    public AnimationCurve scaleCurve;
    private float time = 0;
    
    public static ObjectivePopUp instance;

    public void Awake()
    {
        instance = this;
    }


    public void Update()
    {
        questDisplay.color = new Color(1, 1, 1, opacityCurve.Evaluate(time));
        transform.localScale = Vector3.one*scaleCurve.Evaluate(time);
        //questDisplay.text = text; // change the written text
        time += Time.deltaTime;

        if (time >= 4) // after 4 seconds, destroy object
        {
            Destroy(gameObject);
        }
    }



    public void ObjectiveAppearDissapear(string text)// appear & dissapear
    {
        questDisplay.color = new Color(1, 1, 1, opacityCurve.Evaluate(time));
        transform.localScale = Vector3.one*scaleCurve.Evaluate(time);
        questDisplay.text = text; // change the written text
        time += Time.deltaTime;
    }
    
}
