using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TutorialPopup : MonoBehaviour
{
    public KeyCode key;
    public float timer = 0;
    private float duration = 2;
    public bool finished => (timer > 2);
    SpriteRenderer sr;
    public bool allowTut = false;
    public bool shootTutorial = false;
    private float aliveTimer = 0;
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
                Debug.Log("showing tutorial");
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
        if(!finished && aliveTimer > 25 && timer > 0.75f)
        {
            timer = duration + 5;
        }
        if (!finished && timer > 0.75f)
            aliveTimer += Time.deltaTime;
        if (!finished && Input.GetKey(key))
        {
            timer += Time.deltaTime;
            //alpha of this lerps to whatever % we are at, from 100-0%
            sr.color = (timer < 0.5f) ? new Color(1, 1, 1, 1) : Color.Lerp(new Color(1, 1, 1, 1), new Color(1, 1, 1, 0), (timer -0.5f / duration - 0.5f));
            //sr.color = Color.Lerp(new Color(1, 1, 1, 1), new Color(1, 1, 1, 0), (timer / duration));
        }
        if (finished)
        {
            Destroy(gameObject);
        }
        
    }
}
