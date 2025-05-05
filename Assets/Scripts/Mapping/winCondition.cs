using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;
public class winCondition : MonoBehaviour
{
    public string winConditionText;

    public enum winconditions
    {
        timeTrail, 
        chase,
        assassination
    }
    [SerializeField]
    winconditions Condition = winconditions.timeTrail;
    public winconditions getCondition => this.Condition;
    //added onto each level
    public List<Breakable> target = new List<Breakable>();
    int frameTimer = 0;
    public bool active = false;
    [SerializeField]
    MapNode node;
    private void Start()
    {
        if (node == null)
            node = GetComponent<MapNode>();
    }
    public void startLevel()
    {
        //check through all gameobjects in level for breakable
        Breakable[] allBreakables = GameObject.FindObjectsByType<Breakable>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        Debug.Log("Allbreakables: " + allBreakables.Length);
        //theres probably a better way of doing this
        target = allBreakables.Where(x => x.target == true).ToList();
        if(target.Count == 0)
            Debug.Log("Target count is null");
        else
            Debug.Log(target[0].transform.name);
        
        active = true;
    }

    void Update()
    {
        if(active)
           checkTargets();
        //loop through list
    }


    void checkTargets()
    {
        if (Condition == winconditions.timeTrail)
        {
            //Debug.Log("Time Trail");
            return;
        }

        frameTimer++;

        if (frameTimer >= 200)
        {
            Debug.Log("Frame Timer: "  + Condition + " " + frameTimer + " " + target.Count);

            if (target.Count == 0 && Condition == winconditions.assassination)
                levelWon();//win if you kill the target(s)
            if (target.Count == 0 && Condition == winconditions.chase)
            {
                Debug.Log("Level Lost");
                levelLost();//lose if you kill the target on a chase
            }

            for (int i = target.Count - 1; i >= 0; i--)
            {
                if (target[i] == null)
                {
                    //Debug.Log("Removing at the position " + i + " " + target[i].name);
                    target.RemoveAt(i);//reduce list to 0 once something is killed
                }
            }
            frameTimer = 0;
        }
    }

    void levelWon()
    {
        //send off to the map manager to signal that the ending has been reached
        MapManager.winConditionReached();
        Debug.Log("winCondition fulfilled");
        active = false;
    }
    public void levelLost()
    {
        PlayerVan.vanInstance.canMove = false;
        InLevelCarSlider.instance.levelFailed();
        Debug.Log("winCondition failed");
        active = false;
    }
}
