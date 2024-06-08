using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class EventNode
{
    public string eventText;
    public List<EventResponse> eventResponses;
    public List<int> actionID;//instead of storing actions here, store actions in the root node so they can be easily referenced.
                                        //Simply store an ID For the interaction here
}
//TODO: add a requirement system so some options are not selectable unless you have the necessary stats or items
