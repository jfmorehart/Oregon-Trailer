using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class EventManager : MonoBehaviour
{
    GameObject eventBox;
    private TextMeshProUGUI titleText, descriptionText;
    private Transform responseButtonContainer;
    [SerializeField]
    private GameObject eventBoxPrefab;
    [SerializeField]
    private GameObject responseButtonPrefab;

    public EventObject currentRootEvent;
    //bool eventOnScreen = false;

    public static EventManager instance;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else if (instance == null)
        {
            instance = this;
        }
        //ensure that we have a reference to the event box prefab
        if (eventBoxPrefab == null)
            Debug.LogError("Missing EventBox");


    }

    void Start()
    {
        //define everything relating to the eventbox
        //create an event box that will be our reference to our dialogue box at all times.
        //This is never destroyed, only hidden
        if (eventBox == null)
        {
            eventBox = Instantiate(eventBoxPrefab, GameObject.Find("Canvas").transform);
            titleText = eventBox.transform.GetChild(0).Find("Event Title Text").GetComponent<TextMeshProUGUI>();
            descriptionText = eventBox.transform.GetChild(0).Find("Event Description Text").GetComponent<TextMeshProUGUI>();
            responseButtonContainer = eventBox.transform.GetChild(0).Find("Option Button Container");
        }

        hidebox();

    }
    
    public EventObject objToTest;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            startEvent(objToTest);
        }
    }

    public void hidebox()
    {
        eventBox.SetActive(false);
    }
    public void showBox()
    {
        eventBox.SetActive(true);
    }

    //take in the original sc object, so we have a reference to the object
    public void startEvent(EventObject eventobj)
    {
        currentRootEvent = eventobj;
        showBox();
        titleText.text = eventobj.Title;
        descriptionText.text = eventobj.RootNode.eventText;

        //remove any buttons there were previously
        //create as many buttons as there are responses
        foreach (Transform child in responseButtonContainer)
        {
            Destroy(child.gameObject);
        }
        foreach (EventResponse response in eventobj.RootNode.eventResponses)
        {
            //the container is supposed to have a horizontal layout group or something similar in it
            GameObject buttonObj = Instantiate(responseButtonPrefab, responseButtonContainer);
            buttonObj.GetComponentInChildren<TextMeshProUGUI>().text = response.responseText;

            //add the response function to the button
            buttonObj.GetComponent<Button>().onClick.AddListener(() => selectResponse(response, eventobj.Title));

        }

    }
    public void startEvent(string title, EventNode node)
    {
        showBox();
        titleText.text = title;
        descriptionText.text = node.eventText;

        //remove any buttons there were previously
        //create as many buttons as there are responses
        foreach (Transform child in responseButtonContainer)
        {
            Destroy(child.gameObject);
        }
        foreach (EventResponse response in node.eventResponses)
        {
            //the container is supposed to have a horizontal layout group or something similar in it
            GameObject buttonObj = Instantiate(responseButtonPrefab, responseButtonContainer);
            buttonObj.GetComponentInChildren<TextMeshProUGUI>().text = response.responseText;

            //add the response function to the button
            buttonObj.GetComponent<Button>().onClick.AddListener(() => selectResponse(response,title));
        }
    }

    //Player's response is now selected, and the corresponding node is chosen. 
    public void selectResponse(EventResponse response, string title)
    {
        //Allow the player to recieve items based on their choice - might be discarded
        applyItemAction(response.nextNode);

        if (response.nextNode.eventText != string.Empty)
        {
            startEvent(title, response.nextNode);
        }
        else
        {
            //dialogue over
            //we may need to always have the player end dialogue or choose an option for them to end it, with this structure in place
            currentRootEvent = null;
            hidebox();
        }
    }

    public void applyItemAction(EventNode node)
    {
        //We look at the ID of the current node, then we go to the event of the same ID in the root node
        //Node ActionID -> RootNode ActionList
        if (node.actionID.Count > 0)
        {
            for (int i = 0; i < node.actionID.Count; i++)
            {
                //using the number at action ID in the current index, determine what to do
                Debug.Log(node.actionID[i] + " " + currentRootEvent.actions[node.actionID[i]].item + " " + currentRootEvent.actions[node.actionID[i]].itemAmount);
                switch (currentRootEvent.actions[node.actionID[i]].behavior)
                {
                    case TEMPitemAction.behaviors.add:
                        Debug.Log("Item Added");
                        break;
                    case TEMPitemAction.behaviors.remove:
                        Debug.Log("Item Removed");
                        break;
                    case TEMPitemAction.behaviors.addIndescriminately:
                        Debug.Log("Item Added Indescriminately");
                        break;
                    case TEMPitemAction.behaviors.removeAll:
                        Debug.Log("Item Remove All ");
                        break;
                    default:
                        break;
                }
            }
        }

        
    }



    //so if a specific event needs to be scripted, add it in here and reference it in the event node script.
    //its easy enough to create a function that just says "add object to player" and takes in the object type
    //but my problem is just on the way of referencing the function easily from the node, whether it should be
    
    //basically need a way to execute tempitemaction
    

}
