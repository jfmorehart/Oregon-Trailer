using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestPoint : MonoBehaviour
{
    //discovery distance - serializable
    [SerializeField]
    [Tooltip("This is essentially how hidden it is, or easy to interact with, it is")]
    float discoveryDistance = 30;
    //interaction distance 
    [SerializeField]
    float interactionDistance = 5;

    private bool isDiscovered = false;

    //notebook quest
    [SerializeField]
    TextAsset quest;

    [SerializeField]
    Transform playerTransform;

    [SerializeField]
    private Sprite markerSprite;

    bool activated = false;

    private void Awake()
    {
        //find the player, or get a reference from some manager at some point
        //playerTransform = transform.Find("Van");
    }

    // Start is called before the first frame update
    void Start()
    {
        if (quest == null)
        {
            Debug.LogError("Quest is not set on questpoint script: " + name);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
        if (!isDiscovered)
        {            
            //check to see if the player is close enough to discover this attraction
            //we should probably add new ways for the player to discover things, like by going through a collider at some particular place
           
        }
        else
        {
            //show on UI

        }
        //if we are close enough to interact and we press E, give quest
        if (checkDistance() && !activated)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                activated = true;
                centralEventHandler.StartEvent(quest, true);

            }

            //make sure it is considered discovered just in case we mess up the trigger box
            isDiscovered = true;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!isDiscovered && collision.CompareTag("Player"))
        {
            isDiscovered = true;
        }
    }

    bool checkDistance()
    {
        if (Vector2.Distance(playerTransform.position, transform.position) < interactionDistance)
            return true;
        return false;
        
    }

}
