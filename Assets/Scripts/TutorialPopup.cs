using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPopup : MonoBehaviour
{

    public KeyCode key;
    public float timer = 0;
    public bool finished => (timer > 1);
    public GameObject tutorialObject;


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
