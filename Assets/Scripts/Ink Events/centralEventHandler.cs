using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Ink;
using Ink.Runtime;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using Ink.UnityIntegration;
using DG.Tweening;
public class centralEventHandler : MonoBehaviour
{
    //reference to the Event parent
    [SerializeField]
    private GameObject EventParent;
    [SerializeField]
    private TextMeshProUGUI DescriptionText;
    [SerializeField]
    private TextMeshProUGUI pressContinueText;
    [SerializeField]
    private GameObject notebookParent;
    [SerializeField]
    private TextMeshProUGUI notebookDescriptionText;
    [SerializeField]
    private List<GameObject> notebookButtonObjects= new List<GameObject>();
    [SerializeField]
    private List<Button> notebookButtonScripts = new List<Button>();
    private List<TextMeshProUGUI> notebookButtonTexts = new List<TextMeshProUGUI>();
    public static centralEventHandler instance;
    //reference to each event button

    //we just need to set the button game object
    [SerializeField]
    private List<GameObject> buttonObjects = new List<GameObject>();
    private List<Button> buttonScripts = new List<Button>();
    private List<TextMeshProUGUI> buttonTexts = new List<TextMeshProUGUI>();

    private Story currentStory;
    private bool eventPlaying = true;
    public static bool EventPlaying => instance.eventPlaying;

    [SerializeField]
    private InkFile globalsInkFile;
    private DialogueVariables dialogueVariables;
    [SerializeField]
    SpriteRenderer leftSprite, midLeftSprite, midRightSprite, rightSprite;
    [SerializeField]
    TextMeshProUGUI displayName;
    [SerializeField]
    private float typingSpeed = 0.04f;

    private Coroutine displayLineCoroutine;
    private bool canContinueToNextLine = false;

    private bool playingNotebookEvent = false;
    [SerializeField]
    private Image notebookEventSprite;

    private const string SPEAKER_TAG = "speaker";
    private const string SPRITE_TAG = "spr";
    private const string EMOTION_TAG = "emotion";
    private const string SPRITEPOSITION_TAG = "pos";//temporary, until we get the character movement system online
    private const string LAYOUT_TAG = "layout";

    [SerializeField]
    Sprite gatorheadSprite;
    [SerializeField]
    Sprite playerSprite;
    [SerializeField]
    Sprite pigRace1Sprite, pigRace2Sprite, pigRace3Sprite;
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

        EventParent.transform.localPosition= new Vector2(0, -158.67f);//the clutter of the scene is driving me insane

        dialogueVariables = new DialogueVariables(globalsInkFile.filePath);
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
        for (int i = 0; i < notebookButtonObjects.Count; i++)
        {
            if (i > buttonObjects.Count)
            {
                Debug.Log("Cannot get notebook button script at " + i);
                continue;
            }
            notebookButtonScripts.Add(notebookButtonObjects[i].GetComponent<Button>());
            notebookButtonTexts.Add(notebookButtonObjects[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>());

        }

        for (int i = 0; i < buttonObjects.Count; i++)
        {
            resetChoice(i, false);
        }
        for (int i = 0; i < notebookButtonObjects.Count; i++)
        {
            resetChoiceNotebook(i, false);
        }
        //clearScreen
        eventPlaying = false;
        EventParent.SetActive(false);

        Debug.Log("EVentParent Start Sequence Finished");

        

    }
    private void Update()
    {
        if (!eventPlaying)
        {
            EventParent.SetActive(false);
            return;
        }
        //skip empty text
        if ( DescriptionText.text == "" || DescriptionText.text == " " || DescriptionText.text == "\n" || string.IsNullOrWhiteSpace(DescriptionText.text))
        {
            if (!playingNotebookEvent)
            {
                Debug.Log("Text is empty, go to next thing");
                //we still want the thing to continue
                StartCoroutine(skipEmptyText());
            }

        }
        if (currentStory.currentChoices.Count == 0 && canContinueToNextLine)
        {
            //Debug.Log("Current Choices is 0");
            //probably use different input system
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0))
            {
                Debug.Log("Continuing story");
                continueStory();
            }
        }
    }
    private IEnumerator skipEmptyText()
    {
        yield return new WaitForEndOfFrame();
        continueStory();
    }
    public static void StartEvent(TextAsset inkJSON, bool isNoteBookEvent = false)
    {
        instance.startEvent(inkJSON, isNoteBookEvent);
        GameManager.eventPassedIn();
        UIManager.endMapScreen();
    }

    void handleTags(List<string> currentTags)
    {
        //likely need a way to parse tags collectively
        foreach (string tag in currentTags)
        {
            string[] splitTag = tag.Split(':');
            if (splitTag.Length != 2)
            {
                Debug.LogError("tag could not be parsed" + tag);
            }
            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();

            switch (tagKey)
            {
                case SPEAKER_TAG:
                    displayName.text = tagValue;
                    break;
                case SPRITEPOSITION_TAG:
                    if (tagValue == "left")
                    {
                        
                    }
                    else if(tagValue == "right")
                    {

                    }
                    break;
                case SPRITE_TAG:
                    if (tagValue == "gatorhead")
                    {
                        rightSprite.sprite = spriteFromTag("gatorhead");
                    }
                    else if(tagValue == "you")
                    {
                        leftSprite.sprite = spriteFromTag("you");
                    }
                    else if(playingNotebookEvent){
                        if (tagValue == "PigRace1")
                        {
                            notebookEventSprite.sprite = pigRace1Sprite;
                        }
                        else if (tagValue== "PigRace2")
                        {
                            notebookEventSprite.sprite = pigRace2Sprite;

                        }
                        else if(tagValue == "PigRace1")
                        {
                            notebookEventSprite.sprite = pigRace3Sprite;

                        }
                    }
                    break;
                case EMOTION_TAG:
                    Debug.Log("EMOTING");
                    if (tagValue == "pigReact")
                    {
                        rightSprite.transform.parent.transform.DOPunchRotation(new Vector3(1.5f, 1.5f, 1.5f), 1.5f, 15, 1);
                    }
                    else if (tagValue == "playerReact")
                    {
                        leftSprite.transform.parent.transform.DOPunchRotation(new Vector3(1.5f,1.5f,1.5f), 1.5f, 15, 1);
                    }
                    break;
                case LAYOUT_TAG:

                    break;
                default:
                    Debug.LogWarning("Tag came in but is not currently being handled: " + tagKey + " -- "+ tag);
                    break;
            }
        }
    }

    public Sprite spriteFromTag(string spriteTag)
    {
        switch (spriteTag)
        {
            case "you":
                return playerSprite;
                break;
            case "gatorhead":
                return gatorheadSprite;
                break;
            default:
                Debug.LogWarning("Cannot find sprite");
                return null;
                break;
        }
    }
    public void startEvent(TextAsset inkJSON, bool notebookEvent = false)
    {
        //takes in the ink's json file.
        //if there is no option present, display just the text

        //setting the currentStory is necessary whenever starting a new dialogue event.
        
        if (notebookEvent == false)
        {
            currentStory = new Story(inkJSON.text);
            eventPlaying = true;
            EventParent.SetActive(true);
            //currentStory.BindExternalFunction("giveResource", (int resourceID, int amount) => GameManager.addResource(resourceID, amount));
        }
        else
        {
            currentStory = new Story(inkJSON.text);
            eventPlaying = true;
            notebookParent.SetActive(true);
            playingNotebookEvent = true;
        }

        currentStory.BindExternalFunction("causeEvent", (int eventID) => { eventReferences.instance.eventDesignator(eventID); });
        dialogueVariables.StartListening(currentStory);
        continueStory();
        GameManager.eventPassedIn();
        UIManager.endMapScreen();
    }

    private void continueStory()
    {
        //Debug.Log("Checking if can continue story");
        if (currentStory.canContinue)
        {
            if (displayLineCoroutine != null)
            {
                StopCoroutine(displayLineCoroutine);
            }

            //we now use a typewriter effect
            //DescriptionText.text = currentStory.Continue();
            displayLineCoroutine = StartCoroutine(displayLine(currentStory.Continue()));
            handleTags(currentStory.currentTags);
            Debug.Log("story can continue - updating choices");

        }
        else
        {
            Debug.Log("Event is over");
            exitEvent();
            return;
        }

    }
    
    private void displayChoices()
    {

        List<Choice> currentChoices = currentStory.currentChoices;
        //Debug.Log("Currentchoices amount : " + currentChoices.Count );
        //Debug.Log("Current choices "+currentChoices.Count);
        if (playingNotebookEvent)
        {

            if (currentChoices.Count > notebookButtonObjects.Count)
            {
                Debug.LogError("There are too many choices and not enough buttons. **Cannot display all choices** \n" + "Num of choices given: " + currentChoices.Count);
            }

            for (int i = 0; i < buttonObjects.Count; i++)
            {
                if (i >= currentChoices.Count || currentChoices.Count == 0)
                {
                    //disable the choice button
                    resetChoiceNotebook(i, false);
                }
                else
                {
                    notebookButtonObjects[i].SetActive(true);
                    notebookButtonTexts[i].text = currentChoices[i].text;
                }
            }
        }
        else
        {
            if (currentChoices.Count > buttonObjects.Count)
            {
                Debug.LogError("There are too many choices and not enough buttons. **Cannot display all choices** \n" + "Num of choices given: " + currentChoices.Count);
            }
            for (int i = 0; i < buttonObjects.Count; i++)
            {
                //loop through options. If we have an option present then set it active
                // otherwise set it inactive
                if (i >= currentChoices.Count || currentChoices.Count == 0)
                {
                    //disable the choice button
                    resetChoice(i, false);
                }
                else
                {
                    //add function to buttons
                    buttonObjects[i].SetActive(true);
                    buttonTexts[i].text = currentChoices[i].text;
                }
            }
        }
        //select first choice in list
        StartCoroutine(SelectFirstChoice());

    }

    private IEnumerator displayLine(string line)
    {
        if (playingNotebookEvent)
        {

            notebookDescriptionText.text = "";
            //pressContinueText.gameObject.SetActive(false);
            canContinueToNextLine = false;
            hideChoicesNotebook();
            //display each letter one at a time
            foreach (char letter in line.ToCharArray())
            {

                notebookDescriptionText.text += letter;
                yield return null;

            }
            displayChoices();

            canContinueToNextLine = true;
        }
        else
        {

            DescriptionText.text = "";
            pressContinueText.gameObject.SetActive(false);
            canContinueToNextLine = false;
            hideChoices();
            //display each letter one at a time
            bool isAddingRichTag = false;
            foreach (char letter in line.ToCharArray())
            {

                if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0))
                {
                    DescriptionText.text = line;
                    break;
                }

                if (isAddingRichTag || letter == '<')
                {
                    //remove the entire rich tag 
                    isAddingRichTag = true;
                    DescriptionText.text += letter;
                    if (letter == '>')
                    {
                        isAddingRichTag = false;
                    }
                }
                else
                {
                    //add normally
                    DescriptionText.text += letter;
                    yield return new WaitForSeconds(typingSpeed);
                }

            }
            pressContinueText.gameObject.SetActive(true);
            displayChoices();

            canContinueToNextLine = true;
        }
    }
    private void hideChoices()
    {
        foreach (GameObject button in buttonObjects)
        {
            button.SetActive(false);
        }
    }

    private void hideChoicesNotebook()
    {
        foreach (GameObject button in notebookButtonObjects)
        {
            button.SetActive(false);
        }
    }
    public void makeChoice(int choiceIndex)
    {

        //Debug.Log("Making choice on button " + choiceIndex);
        if (currentStory.currentChoices.Count == 0 || currentStory.currentChoices.Count < choiceIndex)
        {
            Debug.Log("Current Story has no choices or the index is too big");
            return;
        }

        if (canContinueToNextLine)
        {
            currentStory.ChooseChoiceIndex(choiceIndex);
            //currentStory.Continue();
            continueStory();
        }

    }

    private IEnumerator SelectFirstChoice()
    {
        //choices are selected by using arrow keys.
        //Trying to figure out way to make it more responsive to the mouse click instead

        //Event system requires that we clear it first, then wait
        //for at least one frame before we set the new frame
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        if (playingNotebookEvent)
        {
            EventSystem.current.SetSelectedGameObject(notebookButtonObjects[0]);
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(buttonObjects[0]);
        }
    }

    
    private void exitEvent()
    {
        eventPlaying = false;
        notebookParent.SetActive(false);
        EventParent.SetActive(false);
        notebookDescriptionText.text = "";
        DescriptionText.text = "";
        displayName.text = "";
        rightSprite.sprite = null;
        midRightSprite.sprite = null;
        midLeftSprite.sprite = null;
        leftSprite.sprite = null;
        dialogueVariables.StopListening(currentStory);
        //remove text and listeners from each button
        for (int i = 0; i < buttonObjects.Count; i++)
        {
            resetChoice(i);
        }
        for (int i = 0; i < notebookButtonObjects.Count; i++)
        {
            resetChoiceNotebook(i);
        }

        Debug.Log("event Exited");
        GameManager.eventOver();
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
    private void resetChoiceNotebook(int i, bool setActive = true)
    {

        if (i < buttonObjects.Count)
        {
            notebookButtonScripts[i].onClick.RemoveAllListeners();
            notebookButtonTexts[i].text = "";
            notebookButtonScripts[i].onClick.AddListener(delegate { makeChoice(i); }); //Using delegates for now: these wont show up in the inspector but it should be fine still. If you think setting it up with UnityEvents would be better lemme know
        }
        if (!setActive)
        {
            notebookButtonObjects[i].SetActive(false);
        }
        else
        {
            notebookButtonObjects[i].SetActive(true);
        }
    }


    public Ink.Runtime.Object getVariableState(string variableName)
    {
        Ink.Runtime.Object variableValue = null;
        dialogueVariables.variables.TryGetValue(variableName, out variableValue);
        if (variableValue == null)
        {
            Debug.LogWarning("Ink variable was found to be null :( - " + variableName);
        }
        Debug.Log("Variable Value - " + variableValue);
        return variableValue;
    }

    public void callInkFunction(string functionName, int value)
    {
        //ideally this will change the value to 14
        currentStory.EvaluateFunction(functionName,value);
    }
    public void callInkFunction(string functionName, float value)
    {
        //ideally this will change the value to 14
        currentStory.EvaluateFunction(functionName, value);
    }
    public void callInkFunction(string functionName, string value)
    {
        //ideally this will change the value to whatever the string is
        currentStory.EvaluateFunction(functionName, value);
    }
    public void sendCharacterInformation()
    {
        //send the party's character information to Ink
        //ink needs a list of the characters we have and appropriate bools for them
        //essentially loop through the party member list and set it to true
        
    }

    //need a way to interpret
    public void tempFunctionCharactersSpawnIn()
    {

        rightSprite.sprite = spriteFromTag("gatorhead");

        leftSprite.sprite = spriteFromTag("you");
       
    }
}
