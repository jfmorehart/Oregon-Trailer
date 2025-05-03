using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPopup : MonoBehaviour
{
    //checks for a certain input
    public List<interactivePopup> firstPopups = new List<interactivePopup>();
    public List<interactivePopup> secondPopups = new List<interactivePopup>();

    [SerializeField]
    GameObject fwdTutorial, downTutorial, leftTutorial, rightTutorial;

    bool finishedFirstSection = false;
    private void Start()
    {
        
    }

    public void activateTutorial()
    {

    }

    private void Update()
    {
        if (!finishedFirstSection)
        {
            for (int i = 0; i < firstPopups.Count; i++)
            {
                if (!firstPopups[i].finished)
                {
                    if (Input.GetKey(firstPopups[i].key))
                    {
                        firstPopups[i].timer += Time.deltaTime;
                    }
                }
                else if (firstPopups[i].finished && firstPopups[i].tutorialObject)
                {
                    //disable the thing
                    finishedFirstSection = true;

                }
            }
        }
        else
        {

        }
        
    }
}

[Serializable]
public class interactivePopup
{
    public KeyCode key;
    public float timer = 0;
    public bool finished => (timer > 1);
    public GameObject tutorialObject;
}
