using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkEventCharacterManager : MonoBehaviour
{
    
    //this should spawn in all the characters into their appropriate locations
    //takes in the characters it should spawn and spawns them off screen
    //this should be able to move a character on screen


    public void init()
    {

    }
    public void addCharacter(string charName)
    {

    }

    public void moveCharacter()
    {
        //moves a character to a certain position for now

    }
    public void eventOver()
    {
        //exits the event

    }
    public void changeMood()
    {
        //takes in a character and changes their sprite if that character is in the scene

    }
    public void doEffect()
    {
        //makes a certain character become happy for now

    }

}
struct eventcharacter
{
    //name
    //position
    //sprite
    public Sprite spr;
    public Vector2 pos;
    public string characterName;
    public eventcharacter(string name, Vector2 position, Sprite sprite)
    {
        spr = sprite;
        pos = position;
        characterName = name;
    }
}