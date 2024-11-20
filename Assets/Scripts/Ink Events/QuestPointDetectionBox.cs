using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestPointDetectionBox : MonoBehaviour
{
    //has reference to the player 
    //checks to see if this detection box should be active
   
    QuestPoint parentQuest;
    private void Awake()
    {
        parentQuest = transform.parent.GetComponent<QuestPoint>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!parentQuest.IsDiscovered && collision.CompareTag("Player"))
        {
            parentQuest.getDiscovered();
        }
    }

}
