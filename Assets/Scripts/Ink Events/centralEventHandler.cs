using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Ink;
using Ink.Runtime;
using UnityEngine.EventSystems;
using UnityEngine.Events;
public class centralEventHandler : MonoBehaviour
{
    //reference to the Event parent
    [SerializeField]
    private GameObject EventParent;
    [SerializeField]
    private TextMeshProUGUI DescriptionText;
    [SerializeField]
    private TextMeshProUGUI pressContinueText;

    public static centralEventHandler instance;
    //reference to each event button

    //we just need to set the button game object
    [SerializeField]
    private List<GameObject> buttonObjects = new List<GameObject>();
    private List<Button> buttonScripts = new List<Button>();
    private List<TextMeshProUGUI> buttonTexts = new List<TextMeshProUGUI>();

    private Story currentStory;
    private bool eventPlaying = true;

    private void Awake()
    {
        //set the options based on the 

        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else if (instance == null)
        {
            instance = this;
        }



        
    }


    private void Start()
    {
        for (int i = 0; i < buttonObjects.Count; i++)
        {
            if (i > buttonObjects.Count)
            {
                Debug.Log("Cannot get button script at " + i);
                continue;
            }
            buttonScripts.Add(buttonObjects[i].GetComponent<Button>());
            buttonTexts.Add(buttonObjects[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>());
        }

        for (int i = 0; i < buttonObjects.Count; i++)
        {
            resetChoice(i, false);
        }

        //clearScreen
        eventPlaying = false;
        EventParent.SetActive(false);

        Debug.Log("Start Sequence Finished");
    }
    private void Update()
    {
        if (!eventPlaying)
        {
            EventParent.SetActive(false);
            return;
        }

        if (currentStory.currentChoices.Count == 0)
        {
            //probably use different input system
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0))
            {
                continueStory();
            }
        }
        
       
    }
    public static void StartEvent(TextAsset inkJSON)
    {
        instance.startEvent(inkJSON);
    }
    public void startEvent(TextAsset inkJSON)
    {
        //takes in the ink's json file.
        //if there is no option present, display just the text

        //setting the currentStory is necessary whenever starting a new dialogue event.
        currentStory = new Story(inkJSON.text);
        eventPlaying = true;
        EventParent.SetActive(true);

        continueStory();
    }

    private void continueStory()
    {
        if (currentStory.canContinue)
        {
            DescriptionText.text = currentStory.Continue();
            pressContinueText.gameObject.SetActive(true);
            displayChoices();
        }
        else
        {
            pressContinueText.gameObject.SetActive(false);
            exitEvent();
            return;
        }

    }
    
    private void displayChoices()
    {

        List<Choice> currentChoices = currentStory.currentChoices;
        Debug.Log("Currentchoices amount : " + currentChoices.Count );
        if (currentChoices.Count > buttonObjects.Count)
        {
            Debug.LogError("There are too many choices and not enough buttons. **Cannot display all choices** \n" + "Num of choices given: " + currentChoices.Count);
        }
        if (currentChoices.Count == 0)
        {
            pressContinueText.gameObject.SetActive(false);
        }

        for (int i = 0; i < buttonObjects.Count; i++)
        {
            //loop through options. If we have an option present then set it active

            // otherwise set it inactive
            //Debug.Log("Button choice");


            if (i >= currentChoices.Count || currentChoices.Count == 0)
            {
                //disable the choice button
                resetChoice(i, false);
            }
            else
            {
                //add function to buttons

                //Debug.Log("I : " + i);
                buttonObjects[i].SetActive(true);
                buttonTexts[i].text = currentChoices[i].text;
            }

        }

        //select first choice in list
        StartCoroutine(SelectFirstChoice());

    }

    public void makeChoice(int choiceIndex)
    {

        //Debug.Log("Making choice on button " + choiceIndex);
        if (currentStory.currentChoices.Count == 0 || currentStory.currentChoices.Count < choiceIndex)
        {
            Debug.Log("Current Story has no choices or the index is too big");
            return;
        }
        currentStory.ChooseChoiceIndex(choiceIndex);
        //currentStory.Continue();
        continueStory();
    }

    private IEnumerator SelectFirstChoice()
    {
        //choices are selected by using arrow keys.
        //Trying to figure out way to make it more responsive to the mouse click instead


        //Event system requires that we clear it first, then wait
        //for at least one frame before we set the new frame
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(buttonObjects[0]);
    }


    private void exitEvent()
    {
        eventPlaying = false;
        EventParent.SetActive(false);
        DescriptionText.text = "";

        //remove text and listeners from each button
        for (int i = 0; i < buttonObjects.Count; i++)
        {
            resetChoice(i);
        }
        Debug.Log("event Exited");
    }

    //all choices when listeners are removed, should have a single listener to 
    private void resetChoice(int i, bool setActive = true)
    {

        //Debug.Log("Attempting to reset choice at " + i + " -- SetActive: " + setActive);

        if (i< buttonObjects.Count)
        {
            buttonScripts[i].onClick.RemoveAllListeners();
            buttonTexts[i].text = "";
            buttonScripts[i].onClick.AddListener(delegate { makeChoice(i); } ); //Using delegates for now: these wont show up in the inspector but it should be fine still. If you think setting it up with UnityEvents would be better lemme know
        }
        if (!setActive) {
            buttonObjects[i].SetActive(false); 
        }
        else {
            buttonObjects[i].SetActive(true); 
        }
    }


    public void testEvent()
    {
        Debug.Log("OPTION CHOSEN");
    }
    public void testEvent2()
    {
        Debug.Log("SECOND OPTION CHOSEN");
    }

}
