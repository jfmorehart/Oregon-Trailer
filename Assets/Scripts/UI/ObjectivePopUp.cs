using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObjectivePopUp : MonoBehaviour
{
    //string to display
    //public string quest;
    public GameObject questDisplay;
    public GameObject line;
    public TMP_Text questText;

    //Animations
    public AnimationCurve opacityCurve;
    public AnimationCurve scaleCurve;
    private float time = 0;



    public void Update()
    {
        questDisplay.GetComponent<Image>().color = new Color(0.9882353f, 0.972549f, 0.7098039f, opacityCurve.Evaluate(time));
        line.GetComponent<Image>().color = new Color(0.8862746f, 0.3411765f, 0.1647059f, opacityCurve.Evaluate(time));
        questText.color = new Color(0.9882353f, 0.972549f, 0.7098039f, opacityCurve.Evaluate(time));
        //transform.localScale = Vector3.one*scaleCurve.Evaluate(time);
        //questDisplay.text = text; // change the written text
        time += Time.deltaTime;

        if (time >= 4) // after 4 seconds, destroy object
        {
            Destroy(gameObject);
        }
    }


/*                          Don't Think I'm using This?
    public void ObjectiveAppearDissapear(string text)// appear & dissapear
    {
        questDisplay.color = new Color(1, 1, 1, opacityCurve.Evaluate(time));
        transform.localScale = Vector3.one*scaleCurve.Evaluate(time);
        questDisplay.text = text; // change the written text
        time += Time.deltaTime;
    }
    */
    
}
