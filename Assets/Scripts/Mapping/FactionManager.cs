using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactionManager : MonoBehaviour
{
    //we can probably have a dictionary and change faction to int if needed
    //the problem is that we wont be able to change this on the fly in the inspector
    //we likely wont even have a ton of factions implemented later on so it makes less sense
    //to create a huge sophisticated solution to a problem we dont have
    Dictionary<faction, int> factionOpinions = new Dictionary<faction, int>();

    private int neutralsRel = 0;
    private int fratRel = 0;
    private int rebelsRel = 0;

    public static FactionManager instance;

    private void Awake()
    {
        /*
        int amountoffactions = Enum.GetNames(typeof(faction)).Length;
        for (int j = 0;
        */

        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }


    }

    public void changeRelationship(int f, int amnt)
    {
        if (f > Enum.GetNames(typeof(faction)).Length || f < 0)
        {
            return;
        }
        faction fac = (faction) f; 
        switch (fac)
        {
            case faction.Neutral:
                neutralsRel += amnt;
                break;
            case faction.Frat:
                fratRel += amnt;
                break;
            case faction.Rebels:
                rebelsRel += amnt;
                break;
            case faction.Gamblers:
                
                break;
            case faction.SunCult:
                
                break;
            default:
                break;
        }
    }



    public int getRelationship(faction f)
    {
        switch (f)
        {
            case faction.Neutral:
                return neutralsRel;
            case faction.Frat:
                return fratRel;
            case faction.Rebels:
                return rebelsRel;
            case faction.Gamblers:
                break;
            case faction.SunCult:
                break;
            default:
                break;
        }
        return 0;
    }
}
public enum faction
{
    Neutral,
    Frat,
    Rebels,
    Gamblers,
    SunCult
}