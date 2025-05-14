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
    private Image questImage;
    public TMP_Text cityText;
    public GameObject line;
    private Image lineImage;
    public TMP_Text questText;

    //Animations
    public AnimationCurve opacityCurve;
    public AnimationCurve scaleCurve;
    private float time = 0;
    


    private void Awake()
    {
        if (questDisplay == null)
            Debug.Log("Quest Display is null on awake - " + transform.name + " ");
    }
    private void Start()
    {
        if (questDisplay == null)
            Debug.Log("Quest Display is null on start");
        questImage = questDisplay.GetComponent<Image>();
        lineImage = line.GetComponent<Image>();
    }
    public void Update()
    {
        if(questDisplay == null)
            Debug.Log("Quest Display is null " + (questDisplay == null));
        
        //Fade in/out
        
        //background
        questImage.color = new Color(questImage.color.r, questImage.color.g, questImage.color.b, opacityCurve.Evaluate(time));
        
        //city text
        cityText.color = new Color(cityText.color.r, cityText.color.g, cityText.color.b, opacityCurve.Evaluate(time));
        
        //line
        lineImage.color = new Color(lineImage.color.r, lineImage.color.g, lineImage.color.b, opacityCurve.Evaluate(time));
        
        //quest text
        questText.color = new Color(questText.color.r, questText.color.g, questText.color.b, opacityCurve.Evaluate(time));
        
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
