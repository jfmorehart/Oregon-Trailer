using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TutorialPopup : MonoBehaviour
{
    public KeyCode key;
    public float timer = 0;
    public bool finished => (timer > 1);
    SpriteRenderer sr;
    public bool allowTut = false;
    public bool shootTutorial = false;

    public void Awake()
    {
        //check to see if tutorial should be active
        //if it should then activate itself 
        sr = GetComponent<SpriteRenderer>();
        sr.enabled = false;
        if(PopupManager.instance.IsTutorializing)
        {
            //allow this to go on
            PopupManager.instance.addTutorialPopup(this);
            if (!shootTutorial)
            {
                showTutorial();
            }
        }
        
    }
    public void showTutorial()
    {
        sr.enabled = true;
        allowTut = true;
    }
    private void Update()
    {
        if (!finished && Input.GetKey(key))
        {
            timer += Time.deltaTime;
        }
        if (finished)
        {
            Destroy(gameObject);
        }
    }
}
