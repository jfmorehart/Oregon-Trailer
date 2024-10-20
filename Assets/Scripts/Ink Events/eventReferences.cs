using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eventReferences : MonoBehaviour
{
    public static eventReferences instance;
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
    }


    [SerializeField]
    private Sprite playerSpriteProt;
    [SerializeField]
    List<Sprite> notebooksprites = new List<Sprite>();


    //this holds all the events we need a reference to
    public IEnumerator eventDesRoutine(int ID)
    {
        Debug.Log("Event Routine Adding Something");
        yield return new WaitForEndOfFrame();
        Debug.Log("Event Routine Adding Something After frame");
        switch (ID)
        {
            case 0:
                testEvent();
                break;
            case 1:
                testEvent2();
                break;
            case 2:
                //enter jack's fight scene
                SaveManager.instance.simpleSave();
                CombatantsStatic.LoadCombat(null);
                break;
            case 3:
                //remove 10 fuel
                GameManager.addResource(1, -10);
                break;
            case 4:
                //remove 10 rations
                GameManager.addResource(2, -10);
                break;
            case 5:
                //add 10 fuel
                GameManager.addResource(1, 10);
                break;
            case 6:
                //add 10 rations
                GameManager.addResource(2, 10);
                break;
            case 7:
                //add 10000 money
                GameManager.addResource(3, 10000);
                break;
            case 8:
                //lose 900 money
                GameManager.addResource(3, -900);
                break;
            case 9:
                testInkVariables.instance.UpdateChoice();
                break;
            case 10:
                //casue player to spawn in
                centralEventHandler.instance.displayNotebookImage(notebooksprites[0]);
                break;
            case 11:
                centralEventHandler.instance.displayNotebookImage(notebooksprites[1]);
                break;
            case 12:
                GameManager.addResource(3, -5);
                break;
            default:
                Debug.Log("EVENT DEFAULTED");
                break;
        }

    }
    public void eventDesignator(int ID)
    {
        Debug.Log("Adding something");
        StartCoroutine(eventDesRoutine(ID));
        /*
        switch (ID)
        {
            case 0:
                testEvent();
                break;
            case 1:
                testEvent2();
                break;
            case 2:
                //enter jack's fight scene
                SaveManager.instance.simpleSave();
                CombatantsStatic.LoadCombat(null);
                break;
            case 3:
                //remove 10 fuel
                GameManager.addResource(1, -10);
                break;
            case 4:
                //remove 10 rations
                GameManager.addResource(2, -10);
                break;
            case 5:
                //add 10 fuel
                GameManager.addResource(1, 10);
                break;
            case 6:
                //add 10 rations
                GameManager.addResource(2, 10);
                break;
            case 7:
                //add 10000 money
                GameManager.addResource(3, 10000);
                break;
            case 8:
                //lose 900 money
                GameManager.addResource(3, -900);
                break;
            case 9:
                testInkVariables.instance.UpdateChoice();
                break;
            default:
                Debug.Log("EVENT DEFAULTED");
                break;
        }*/
    }



    public void testEvent()
    {
        Debug.Log("OPTION CHOSEN");
    }
    public void testEvent2()
    {
        Debug.Log("SECOND OPTION CHOSEN");
    }
    public void testEvent3()
    {
        Debug.Log("THIRD OPTION CHOSEN");
    }
    public void testEvent4()
    {
        Debug.Log("FOURTH OPTION CHOSEN");
    }

}
