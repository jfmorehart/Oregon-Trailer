using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MapArrowUI : MonoBehaviour
{

    public static MapArrowUI instance;
    public Transform arrow;
    private GameObject van;
    private List<QuestPoint> points;
    Transform nearestQuest;

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

    public void startLevel()
    {
        Debug.Log("LevelStartRoutine");
        //records the player's start position
        van = GameObject.Find("Van(Clone)");
        if (van == null)
        {
            Debug.Log("Van is not Found");
        }
        Transform nearestpoint = GameObject.Find("goal(Clone)").transform;

    }
    // Update is called once per frame
    void Update()
    {
        if (van != null && nearestQuest != null)
        {

            for (int i = 0; i < points.Count; i++)
            {
                //find the closest point
                if (Vector2.Distance(van.transform.position, points[i].transform.position) < Vector2.Distance(van.transform.position, nearestQuest))
                {
                    nearestQuest = points[i].transform;
                }
            }
            
            //rotate arrow


        }
        else
        {

        }
    }
    public void addQuestPoint(QuestPoint qp)
    {
        points.Add(qp);
    }
    



    public void levelDone()
    {
        Debug.Log("Level Done Routine");
        van = null;
        nearestQuest = null;
        points.Clear();
    }

}
