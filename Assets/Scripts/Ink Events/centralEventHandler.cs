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
using System;

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
    private List<GameObject> notebookButtonObjects = new List<GameObject>();
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
    TextMeshProUGUI displayName;
    [SerializeField]
    private float typingSpeed = 0.04f;

    private Coroutine displayLineCoroutine;
    private bool canContinueToNextLine = false;

    private bool playingNotebookOnlyEvent = false;
    private bool shouldShowNotebook = false;//temporary tag 
    [SerializeField]
    private Image notebookEventSprite;

    private const string MOVE_TAG = "move";
    private const string CHANGE_SPRITE_TAG = "spr";
    private const string EMOTE_TAG = "emote";
    private const string ENTER_TAG = "enter";//temporary, until we get the character movement system online
    private const string EXIT_TAG = "exit";
    private const string STATE_TAG = "state";
    private const string NOTEBOOK_TAG = "notebook";
    private const string NOTEBOOKONLY_TAG = "notebookonly";
    private const string SPEAKER_TAG = "speaker";
    //#move charactername movement_type stage_destination
    //#move charactername direction number
    //#spr charactername image_name
    //#enter charactername entrance_type stage_destination
    //#emote charactername emotion_type
    //#exit charactername exit_type
    //#state character state_type

    [Header("Stage")]
    [SerializeField]
    List<SpriteRenderer> stageCharacters = new List<SpriteRenderer>();
    private const string BLANK_CHARACTER_NAME = "temp";//the name each character sprite renderer resets to 
    [SerializeField]
    Transform offStageLeft, stageLeft, stageMidLeft, stageMiddle, stageMidRight, stageRight, offStageRight;
    Dictionary<SpriteRenderer, Transform> StageCharacterFinalDestination = new Dictionary<SpriteRenderer, Transform>();
    [SerializeField]
    Image eventBackground, fadetoblackBG;

    [SerializeField]
    CharacterBase mc;

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

        EventParent.transform.localPosition = new Vector2(0, -158.67f);//the clutter of the scene is driving me insane

        dialogueVariables = new DialogueVariables(globalsInkFile.filePath);
    }


    private void Start()
    {   
        if(CurrentGame.activeParty == null)
            CurrentGame.NewParty(new Character(mc));

        DOTween.Init();
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

        //add each to the dictionary
        foreach (SpriteRenderer sr in stageCharacters)
        {
            sr.transform.position = offStageLeft.position;
            StageCharacterFinalDestination.Add(sr, offStageLeft);
        }

    }
    private void Update()
    {
        if (!eventPlaying)
        {
            EventParent.SetActive(false);
            notebookParent.SetActive(false);
            return;
        }
        //skip empty text
        if (DescriptionText.text == "" || DescriptionText.text == " " || DescriptionText.text == "\n" || string.IsNullOrWhiteSpace(DescriptionText.text))
        {
            if (!playingNotebookOnlyEvent && !shouldShowNotebook)
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
    public static void StartEvent(TextAsset inkJSON, bool isNoteBookEvent = false, Sprite bgSprite = null)
    {
	    Debug.Log(inkJSON);
        if (bgSprite == null)
        {
            instance.startEvent(inkJSON, isNoteBookEvent);
        }
        else
        {
            instance.StartCoroutine(instance.showbackground(bgSprite, inkJSON, isNoteBookEvent));
        }
        GameManager.eventPassedIn();
        UIManager.endMapScreen();
    }

    private void passInCharacterStats()
    {
        //throw new NotImplementedException();

        //CurrentGame.activeParty.members[0].SkillCheck(Character.ScalableSkill.Charisma);

        //callInkFunction("setGatorHeadInParty", );
        callInkFunction("setTestValue", 30);
        if ( CurrentGame.activeParty.members == null)
        {
            Debug.Log("Crassus");
        }
        Debug.Log(currentStory.variablesState["testValue"]);
        callInkFunction("setCharisma", CurrentGame.activeParty.members[0].SkillCheck(Character.ScalableSkill.Charisma));
        callInkFunction("setWisdom", CurrentGame.activeParty.members[0].SkillCheck(Character.ScalableSkill.Wisdom));
        callInkFunction("setConstitution", CurrentGame.activeParty.members[0].SkillCheck(Character.ScalableSkill.Constitution));
        callInkFunction("setMoxie", CurrentGame.activeParty.members[0].SkillCheck(Character.ScalableSkill.Moxie));
        callInkFunction("setGumption", CurrentGame.activeParty.members[0].SkillCheck(Character.ScalableSkill.Gumption));

        //Debug.Log(dialogueVariables.variables["testValue"]);
        //Constitution, 0 
        //Charisma, 1 
        //Wisdom, 2
        //Moxie,  3
        //Gumption 4
        

    }
    public bool TrySetInkStoryVariable(string variable, object value)
    {
        if (currentStory != null &&
            currentStory.variablesState.GlobalVariableExistsWithName(variable))
        {
            currentStory.variablesState[variable] = value;
            return true;
        }

        return false;
    }

    void handleTags(List<string> currentTags)
    {
        if(currentTags.Count != 0)
            Debug.Log("Handling tags "+ currentTags[0]);


        foreach (string tag in currentTags)
        {
            string[] splitTag = tag.Split(" ");
            
            string actionType = splitTag[0];

            //Debug.Log("tag: " + actionType);
            switch (actionType)
            {
                case MOVE_TAG:
                    //parse the movement tag
                    //check to see if there are the right amount of tags first
                    //#move charactername movement_type stage_destination
                    //#move charactername direction number
                    if (splitTag.Length != 4)
                        return;

                    float _moveAmount;
                    if(float.TryParse(splitTag[3], out _moveAmount))
                    {
                        moveCharacter(splitTag[1], _moveAmount, splitTag[2]);
                    }
                    else
                    {
                        moveCharacter(splitTag[1], splitTag[2], splitTag[3]);
                    }
                    break;
                case CHANGE_SPRITE_TAG:
                    //#spr charactername image_name
                    if (splitTag.Length == 3)
                        changeSprite(splitTag[1], splitTag[2]);
                    if(playingNotebookOnlyEvent || shouldShowNotebook)
                    {
                        notebookEventSprite.sprite = Resources.Load<Sprite>("Sprites/" + splitTag[1]);
                    }
                    break;
                case ENTER_TAG:
                    //#enter charactername entrance_type stage_destination
                    if (splitTag.Length != 4)
                        return;
                    enterCharacter(splitTag[1], splitTag[2], splitTag[3]);
                    break;
                case EMOTE_TAG:
                    //#emote charactername emotion_type
                    if (splitTag.Length != 3)
                        return;
                    characterEmote(splitTag[1], splitTag[2]);
                    break;
                case EXIT_TAG:
                    //#exit charactername exit_type
                    if (splitTag.Length != 3)
                    {
                        Debug.Log("Tag cannot be parsed - " + tag);
                        return;
                    }
                    exitCharacter(splitTag[1], splitTag[2]);
                    break;
                case STATE_TAG:
                    //#state character state_type
                    if (splitTag.Length != 3)
                        return;
                    characterState(splitTag[1], splitTag[2]);
                    break;
                case NOTEBOOK_TAG:
                    //make this string become a notebook
                    //set notebook bool to be active
                    Debug.Log("Notebook tag found");
                    shouldShowNotebook = true;
                    //StopCoroutine(displayLineCoroutine);
                    //StartCoroutine(displayLine(currentStory.Continue()));
                    break;
                case NOTEBOOKONLY_TAG:
                    Debug.Log("Notebook tag found");
                    playingNotebookOnlyEvent = true;
                    //StopCoroutine(displayLineCoroutine);
                    //StartCoroutine(displayLine(currentStory.Continue()));
                    break;
                case SPEAKER_TAG:
                    displayName.text = splitTag[1];
                    break;
                default:
                    Debug.LogWarning("Tag came in but is not currently being handled: " + actionType + " -- "+ tag);
                    break;
            }
        }
    }
    private void startEvent(TextAsset inkJSON, bool notebookEvent = false)
    {
		//takes in the ink's json file.
		//if there is no option present, display just the text
		//setting the currentStory is necessary whenever starting a new dialogue event.
		Debug.Log("notebook event = " + notebookEvent);
		if (notebookEvent == false)
        {
            shouldShowNotebook = false;
            playingNotebookOnlyEvent = false;

            currentStory = new Story(inkJSON.text);
            eventPlaying = true;
            EventParent.SetActive(true);
            //notebookParent.SetActive(true);
            //currentStory.BindExternalFunction("giveResource", (int resourceID, int amount) => GameManager.addResource(resourceID, amount));
        }
        else
        {
            currentStory = new Story(inkJSON.text);
            eventPlaying = true;
            notebookParent.SetActive(true);
            playingNotebookOnlyEvent = true;
        }

        currentStory.BindExternalFunction("causeEvent", (int eventID) => { eventReferences.instance.eventDesignator(eventID); });
        instance.passInCharacterStats();
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
            skippingthrough = false;
            passInCharacterStats();
            if (displayLineCoroutine != null)
            {
                StopCoroutine(displayLineCoroutine);
            }
            shouldShowNotebook = false;

            //we now use a typewriter effect
            //DescriptionText.text = currentStory.Continue();

            handleNotebookEvent();

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
    void handleNotebookEvent()
    {
        if (shouldShowNotebook || playingNotebookOnlyEvent)
        {

            Debug.Log("Notebook to be displayed");
            //set the notebook to be active and disable regular textbox
            notebookParent.SetActive(true);
            EventParent.SetActive(false);
        }
        else
        {
            notebookParent.SetActive(false);
            EventParent.SetActive(true);
        }

    }
    private void displayChoices()
    {

        List<Choice> currentChoices = currentStory.currentChoices;
        //NOTE: it doesnt hurt to always update this section just in case

        
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

            for (int i = 0; i < buttonObjects.Count; i++)
            {
                if (i >= currentChoices.Count || currentChoices.Count == 0)
                {
                    //disable the choice button
                    resetChoiceNotebook(i, false);
                }
                else
                {
                    if(playingNotebookOnlyEvent || shouldShowNotebook)
                        notebookButtonObjects[i].SetActive(true);
                    notebookButtonTexts[i].text = currentChoices[i].text;
                }
            }
        
        //select first choice in list
        StartCoroutine(SelectFirstChoice());

    }
    private void skipmovement()
    {
        Debug.Log("skippinmg movement");
        foreach (SpriteRenderer sr in stageCharacters)
        {
            sr.DOKill();
            sr.transform.position = StageCharacterFinalDestination[sr].position;
        }
    }

    bool skippingthrough = false;

    private IEnumerator displayLine(string line)
    {
        //handleNotebookEvent();
        skipmovement();

        DescriptionText.text = line;
		DescriptionText.enableAutoSizing = true;
        yield return null;
		float sz = DescriptionText.fontSize;
        DescriptionText.alignment = TextAlignmentOptions.Left;
        DescriptionText.text = "";
        DescriptionText.enableAutoSizing = false;
        DescriptionText.fontSize = sz;


            DescriptionText.text = "";
            pressContinueText.gameObject.SetActive(false);
            canContinueToNextLine = false;
            hideChoices();
            hideChoicesNotebook();
            //display each letter one at a time
            bool isAddingRichTag = false;
            foreach (char letter in line.ToCharArray())
            {

                //if ((Input.GetKeyDown(KeyCode.Space) && !skippingthrough )|| (Input.GetKeyDown(KeyCode.Mouse0) && !skippingthrough))


                if (isAddingRichTag || letter == '<')
                {
                    //remove the entire rich tag 
                    isAddingRichTag = true;
                    DescriptionText.text += letter;
                    notebookDescriptionText.text += letter;
                    if (letter == '>')
                    {
                        isAddingRichTag = false;
                    }
                }
                else
                {
                    //add normally
                    DescriptionText.text += letter;
                    notebookDescriptionText.text += letter;
                    yield return new WaitForSeconds(typingSpeed);
                }
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0))
            {
                DescriptionText.text = line;
                notebookDescriptionText.text = line;
                //skippingthrough = true;
                Debug.Log("Skipping throguh text");
                break;
            }
        }
            pressContinueText.gameObject.SetActive(true);
            displayChoices();
            
            canContinueToNextLine = true;
        


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
        if (playingNotebookOnlyEvent || shouldShowNotebook)
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
        StartCoroutine(removeBackground());
        Debug.Log("event Exited");
        resetStage();
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
        //ideally this will change the value to 14
        currentStory.EvaluateFunction(functionName, value);
    }

    //characters must be instantiated by using character entrance first

    //to move a character to a certain stage position use this syntax (stage_destination should be left, l, midleft, ml, middle, m, midright, mr, right, r):
    //#move charactername movement_type stage_destination

    //to move a character precisely use this syntax (direction must either be left, right, l , or r): 
    //#move charactername direction number

    //to change a sprite use this syntax (character has to be instantiated first, just move the character to a position):
    //#spr charactername image_name

    //to make a character enter use this syntax (entrance_type should be oneword - fadeout/fastappearright):
    //#enter charactername entrance_type stage_destination

    //to make a character emote: 
    //#emote charactername emotion_type

    //to make a character exit:
    //#exit charactername exit_type

    //to change a character state:
    //#state character state_type

    public void enterCharacter(string characterName, string movementType, string stagePosition)
    {
        //instantiates character
        
        SpriteRenderer refChar = null;

        for (int i = 0; i < stageCharacters.Count; i++)
        {
            //loop through all until we find a blank character
            if (refChar == null && stageCharacters[i].name == BLANK_CHARACTER_NAME)
            {
                refChar = stageCharacters[i];
                stageCharacters[i].name = characterName;
            }
        }
        if (refChar == null)
        {
            Debug.LogWarning("No character slots available");
            return;
        }
        Debug.Log("ENTER CHARCTER CALLED to " + stagePosition + " " + getStagePosition(stagePosition));

        //move the character
        doEntranceMovement(refChar.transform, movementType, getStagePosition(stagePosition));
    }
    private void exitCharacter(string characterName, string movementType)
    {
        //instantiates character
        SpriteRenderer refChar = null;

        for (int i = 0; i < stageCharacters.Count; i++)
        {
            //loop through all until we find a blank character
            if (refChar == null && stageCharacters[i].name == BLANK_CHARACTER_NAME)
            {
                refChar = stageCharacters[i];
                stageCharacters[i].name = characterName;
            }
        }
        if (refChar == null)
        {
            Debug.LogWarning("No character slots available");
            return;
        }

        doExitMovement(refChar.transform, movementType);
    }
    public void changeSprite(string characterName, string spriteName)
    {
        SpriteRenderer refChar = null;
        for (int i = 0; i < stageCharacters.Count; i++)
        {
            if (refChar != null)
                break;
            if (refChar == null && stageCharacters[i].name == characterName)
            {
                refChar = stageCharacters[i];
                //all sprites should be in resources
                //sprite 01 should be in (Assets/Resources/Sprites/sprite01.png)
                var spr = Resources.Load<Sprite>("Sprites/"+spriteName);
                refChar.sprite = spr;
            }
        }
    }
    private void changeSprite(string spriteName)
    {
        //changes the sprite in the notebook event
        var spr = Resources.Load<Sprite>("Sprites/" + spriteName);
        notebookEventSprite.sprite = spr;
    }

    private void moveCharacter(string characterName, string movementType, string stagePosition)
    {
        SpriteRenderer refChar = null;
        //check through each position in our spriterenderer list, check the character names
        //if any match the character name present, have a reference to that character
        //if reference character is null, then we just take whatever character is available
        for (int i = 0; i < stageCharacters.Count; i++)
        {
            if (stageCharacters[i].name == characterName)
            {
                //clear out previously used blank character just in case we already assigned it
                if (refChar != null)
                    refChar.name = BLANK_CHARACTER_NAME;
                refChar = stageCharacters[i];
                break;
            }
            else if (refChar == null && stageCharacters[i].name == BLANK_CHARACTER_NAME)
            {
                //if we still havent found the character
                refChar = stageCharacters[i];
                stageCharacters[i].name = characterName;
            }
        }

        //get the final position, either a new stage position or its current position
        Transform stpos = getStagePosition(stagePosition);

        //add their final destination to the dictionary so if the player skips the dialogue
        //they instantly finish their movement at the position
        StageCharacterFinalDestination[refChar] = stpos;
        Debug.Log("MOVE CHARACTER CALLED");

        //parse the movement type
        doMoveToPosition(refChar.transform, stpos, movementType);

    }

    public void moveCharacter(string characterName, float movementAmount, string leftOrRight)
    {
        SpriteRenderer refChar = null;
        //check through each position in our spriterenderer list, check the character names
        //if any match the character name present, have a reference to that character
        //if reference character is null, then we just take whatever character is available
        for (int i = 0; i < stageCharacters.Count; i++)
        {
            if (stageCharacters[i].name == characterName)
            {
                //clear out previously used blank character just in case we already assigned it
                if (refChar != null)
                    refChar.name = BLANK_CHARACTER_NAME;
                refChar = stageCharacters[i];
                break;
            }
            else if (refChar == null && stageCharacters[i].name == BLANK_CHARACTER_NAME)
            {
                //if we still havent found the character
                refChar = stageCharacters[i];
                stageCharacters[i].name = characterName;
            }
        }

        //parse movement 
        doMoveToPosition(refChar.transform, movementAmount, leftOrRight);

    }

    public void resetStage()
    {
        //teleport each character off screen - reset sprites for each
        //check their transform names and see if any have been set, if 
        foreach (SpriteRenderer sr in stageCharacters)
        {
            sr.DOKill();
            sr.name = BLANK_CHARACTER_NAME;
            sr.transform.localPosition = offStageRight.localPosition;
            sr.sprite = null;
            sr.flipX = false;
        }
    }

    private IEnumerator resetCharacter(SpriteRenderer sr)
    {
        yield return new WaitForSeconds(1);
        sr.name = BLANK_CHARACTER_NAME;
        sr.transform.localPosition = offStageRight.localPosition;
        sr.sprite = null;
    }


    private void doMoveToPosition(Transform characterTransform, float movementAmount, string LorR)
    {
        switch (LorR)
        {
            case "l":
            case "left":
                movementAmount *= -1;
                break;
            default:
                break;
        }
        Vector2 newpos = new Vector2(characterTransform.position.x + movementAmount, characterTransform.position.y);
        //for right now we do not consider this as having fast movement
        characterTransform.DOMove(newpos, 1f, false);
    }
    private void doMoveToPosition(Transform characterTransform, Transform stagePosition, string movementType)
    {
        if(movementType == "fast")
            characterTransform.DOMove(stagePosition.position, 0.5f, false);
        else if(movementType == "normal")
            characterTransform.DOMove(stagePosition.position, 1f, false);

        //move the dictionary value
        StageCharacterFinalDestination[characterTransform.GetComponent<SpriteRenderer>()] = stagePosition;
    }

    //when the player presses continue,
    //we kill any tweens on the functions and move them to their final locations
    private Transform getStagePosition(string stagePos)
    {
        stagePos = stagePos.ToLower();
        Debug.Log("Going to a position" + stagePos);
        switch (stagePos)
        {

            case "left":
            case "l":
                return stageLeft;
            case "ml":
            case "midleft":
                return stageMidLeft;
            case "mid":
            case "middle":
            case "m":
                return stageMiddle;
            case "mr":
            case "midright":
            case "middleright":
                return stageMidRight;
            case "right":
            case "r":
                return stageRight;
            case "offstageleft":
            case "ol":
            case "osl":
                return offStageLeft;
            case "offstageright":
            case "or":
            case "osr":
                return offStageRight;
            default:
                Debug.Log("Cannot find stage position : " + stagePos);
                return offStageLeft;
        }
    }

    //#enter charactername entrance_type stage_destination
    private void doEntranceMovement(Transform characterTransform, string movementType, Transform destination)
    {
        movementType = movementType.ToLower();
        Debug.Log("destination  " + destination.name);
        switch (movementType)
        {
            case "appearleft":
                //set the position according to name, and then slide in accordingly
                characterTransform.localPosition = offStageLeft.localPosition;
                characterTransform.DOLocalMove(destination.localPosition, 2, false).SetEase(Ease.Linear);
                break;
            case "appearright":
                characterTransform.localPosition = offStageRight.localPosition;
                Debug.Log(characterTransform.localPosition + " " + offStageRight.localPosition);
                characterTransform.DOLocalMove(destination.localPosition, 2, false).SetEase(Ease.Linear);

                break;
            case "fastappearleft":
                characterTransform.localPosition = offStageLeft.localPosition;
                characterTransform.DOLocalMove(destination.localPosition, 0.5f, false).SetEase(Ease.Linear);
                break;
            case "fastappearright":
                characterTransform.localPosition = offStageRight.localPosition;
                characterTransform.DOLocalMove(destination.localPosition, 0.5f, false).SetEase(Ease.Linear);//inoutquad, outquad, InQuart
                break;
            case "fadein":
                Debug.Log("fading in");
                characterTransform.localPosition = destination.localPosition;
                characterTransform.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
                characterTransform.GetComponent<SpriteRenderer>().DOFade(1,0.5f);
                break;
            default:
                break;
        }
        //set the characters dictionary value
        StageCharacterFinalDestination[characterTransform.GetComponent<SpriteRenderer>()] = destination;

    }
    private void doExitMovement(Transform characterTransform, string movementType)
    {
        switch (movementType)
        {
            case "exitleft":
                characterTransform.DOLocalMove(offStageLeft.position, 1f, false);
                break;
            case "exitright":
                characterTransform.DOLocalMove(offStageRight.position, 1f, false);
                break;
            case "fastexitleft":
                characterTransform.DOLocalMove(offStageLeft.position, 0.5f, false);
                break;
            case "fastexitright":
                characterTransform.DOLocalMove(offStageRight.position, 0.5f, false);
                break;
            case "fadeout":
                characterTransform.GetComponent<SpriteRenderer>().DOFade(0, 0.5f);
                break;
            default:
                break;
        }

        //set the final destination to off stage right no matter the move
        StageCharacterFinalDestination[characterTransform.GetComponent<SpriteRenderer>()] = offStageRight;

        StartCoroutine(resetCharacter(characterTransform.GetComponent<SpriteRenderer>()));
    }

    private void characterEmote(string characterName, string emotionType)
    {
        SpriteRenderer refChar = null;
        //check through each position in our spriterenderer list, check the character names
        //if any match the character name present, have a reference to that character
        //if reference character is null, then we just take whatever character is available
        for (int i = 0; i < stageCharacters.Count; i++)
        {
            if (stageCharacters[i].name == characterName)
            {
                //clear out previously used blank character just in case we already assigned it
                if (refChar != null)
                    refChar.name = BLANK_CHARACTER_NAME;
                refChar = stageCharacters[i];
                break;
            }
            else if (refChar == null && stageCharacters[i].name == BLANK_CHARACTER_NAME)
            {
                //if we still havent found the character
                refChar = stageCharacters[i];
                stageCharacters[i].name = characterName;
            }
        }

        doEmote(refChar.transform, emotionType);
    }
    private void doEmote(Transform characterTransform, string emotionType)
    {
        //Pop - little scale up effect to create a pop - pop
        emotionType = emotionType.ToLower();
        switch (emotionType)
        {
            case "pop":
                Vector3 aimScale= characterTransform.localScale * 1.5f;
                characterTransform.DOPunchScale(characterTransform.localScale, 1, 10, 1);
                break;
            default:
                Debug.LogWarning("EmoteType is note available");
                break;
        }

    }
    private void characterState(string characterName, string stateType)
    {
        SpriteRenderer refChar = null;
        //check through each position in our spriterenderer list, check the character names
        //if any match the character name present, have a reference to that character
        //if reference character is null, then we just take whatever character is available
        for (int i = 0; i < stageCharacters.Count; i++)
        {
            if (stageCharacters[i].name == characterName)
            {
                //clear out previously used blank character just in case we already assigned it
                if (refChar != null)
                    refChar.name = BLANK_CHARACTER_NAME;
                refChar = stageCharacters[i];
                break;
            }
            else if (refChar == null && stageCharacters[i].name == BLANK_CHARACTER_NAME)
            {
                //if we still havent found the character
                refChar = stageCharacters[i];
                stageCharacters[i].name = characterName;
            }
        }

        //parse movement
        doState(refChar.transform, stateType);
    }
    private void doState(Transform characterTransform, string stateType)
    {
        stateType = stateType.ToLower();
        switch (stateType)
        {
            case "flip":
                characterTransform.GetComponent<SpriteRenderer>().flipX = !characterTransform.GetComponent<SpriteRenderer>().flipX;
                break;
            case "bounce":
                break;
            case "shake":
                Vector3 aimScale = characterTransform.localScale * 1.5f;
                characterTransform.DOPunchScale(characterTransform.localScale, 1, 10, 1);
                break;
            default:
                break;
        }
    }

    public void displayNotebookImage(Sprite spr)
    {
        notebookEventSprite.sprite = spr;
    }


    //to make a character emote: 
    //#emote charactername emotion_type

    //to change a character state:
    //#state character state_type
    public IEnumerator removeBackground()
    {
        if (eventBackground.isActiveAndEnabled)
        {
            float duration = 1f;
            fadetoblackBG.gameObject.SetActive(true);
            Color transparent = new Color(0, 0, 0, 0);
            Color full = new Color(0, 0, 0, 1);

            float time = 0;
            fadetoblackBG.color = transparent;
            while (time < duration)
            {
                fadetoblackBG.color = Color.Lerp(transparent, full, time / duration);
                time += Time.deltaTime;
                yield return null;
            }
            fadetoblackBG.color = new Color(0, 0, 0, 1);
            yield return new WaitForSeconds(0.5f);
            eventBackground.gameObject.SetActive(false);
            time = 0;
            while (time < duration)
            {
                fadetoblackBG.color = Color.Lerp(full, transparent, time / duration);
                time += Time.deltaTime;
                yield return null;
            }

            fadetoblackBG.gameObject.SetActive(false);
        }
    }
    public IEnumerator showbackground(Sprite bg, TextAsset inkJSON, bool isNotebookEvent)
    {
        /*
        //fadetoblackBG.DOFade(255, 0.5f);
        fadetoblackBG.DOFade(255, 2* Time.deltaTime);
        Debug.Log("pre "+fadetoblackBG.color);
        yield return new WaitForSeconds(2);
        Debug.Log("post " + fadetoblackBG.color);
        eventBackground.gameObject.SetActive(true);
        eventBackground.sprite = bg;
        fadetoblackBG.DOKill();
        yield return new WaitForSeconds(2);
        fadetoblackBG.DOKill();
        fadetoblackBG.DOFade(0, 2* Time.deltaTime);
        instance.startEvent(inkJSON, isNotebookEvent);
        fadetoblackBG.gameObject.SetActive(false);
        */
        float duration = 1f;
        eventBackground.sprite = bg;
        eventBackground.gameObject.SetActive(false);
        fadetoblackBG.gameObject.SetActive(true);
        Color transparent = new Color(0, 0,0, 0);
        Color full = new Color(0,0,0,1);

        float time = 0;
        fadetoblackBG.color = transparent;
        while (time < duration)
        {
            fadetoblackBG.color = Color.Lerp(transparent, full, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        fadetoblackBG.color = new Color(0,0,0,1);
        yield return new WaitForSeconds(0.5f);
        eventBackground.gameObject.SetActive(true);
        time = 0;
        while (time < duration)
        {
            fadetoblackBG.color = Color.Lerp(full,transparent , time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        fadetoblackBG.gameObject.SetActive(false);

        instance.startEvent(inkJSON, isNotebookEvent);

    }


}

//list of objects
//create object (string name. entrance , pos)
//move(movement type, object name)
//


//#char_gatorhead # verb #
//#char_gatorhead verb position movement_type


//characters talking with each other at pit stops
//   set two characters talking at a certain point

//loop through all 3 reference character objects. Change their sprite


/*
 * Visual novel effects list for programmers

Entrances:
Appear left- slide in from the left - appearleft
Appear right - slide in from the right - appearright
Fast appear left - fast slide in from left - fastappearleft
Fast appear right - fast slide in from right - fastappearright
Appear - fade in - fadein

Exits:
fade - fade out - fadeout
Exit left - slide out left - exitleft
Exit right - slide out right - exitright
Fast exit left - fast slide out left - fastexitleft
Fast exit right - fast slide out right - fastexitright

Emotion changes:
Pop - little scale up effect to create a pop - pop
Fade - fade transition - fade what the hell is this?

States:
Shake - make the character jitter a bit - shake
Bounce - make the character bounce up and down - bounce
Flip - flip the character - flip

Movement: 
Move left x amount - move character left x amount - 
Move right x amount - move character right x amount - 

Movement slide to position
move to stagePosition
#move character_name stage_position fast/normal

Text:
Pop in - similar to �STOP THE VAN�
Shake - make the text jitter a bit, still comes it normal typewriter 
Small - make the text a bit smaller
Big - make the text big
Slow - make the text appear slowly
*/