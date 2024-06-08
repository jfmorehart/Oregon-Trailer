using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Event")]
public class EventObject : ScriptableObject
{
    public string Title;
    public EventNode RootNode;
    public List<TEMPitemAction> actions;

}

public enum TEMPitemList
{
    godRod,
    dick,
    coochie,
    pintle,
    TipOfMyMachine
}
[System.Serializable]
public struct TEMPitemAction
{
    //holds the type of item and the amount of it
    public TEMPitemList item;
    public float itemAmount;
    public behaviors behavior;
    //this specifically deals with how we are treating the items in regards to the players inventory
    public enum behaviors
    {
        add,//remove the item from the inventory completely
        remove,//adds the items to the inventory
        addIndescriminately,//potentially add item to the inventory without caring if the inventory is full 
        removeAll,//remove all of the items up to the amount listed(IE: If there was 2 items of X, and we want 10 removed, all would be removed)

    }

    public TEMPitemAction(TEMPitemList item, float amnt, behaviors behavior)
    {
        this.item = item;
        itemAmount = amnt;
        this.behavior = behavior;
    }
}