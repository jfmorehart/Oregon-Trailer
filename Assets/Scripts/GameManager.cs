using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //this generally handles resource management, group members, and other miscellanious information


    //choose what screen to show
        //Map screen, Game Screen, Van Screen
    //is the van stopped? Should the van keep rumbling on?
        //probably have the van animations off
    bool vanRunning = false;


    //group members
    //daily resource drain based on each character
    //expected resource drain
    //Should be structured like this: 3 Water (-2 water)
    //Next day should show the change in this

    //singleton
    

    private void Awake()
    {
    }

    private void Update()
    {
        if (vanRunning)
        {

        }
    }

}
