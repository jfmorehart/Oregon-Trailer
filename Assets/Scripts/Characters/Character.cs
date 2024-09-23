using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character
{
    // This class is what stores the active version of the data that makes up a character
    // 'active' meaning malleable— anything that needs to be modified and saved. 

    //unmodifiable
    public CharacterBase baseCharacter; //used for spriteLookups etc

	//data that we need modifiable copies of
	public int health;
    public int rations;
    public int vices;

    public Character(CharacterBase original) {
        baseCharacter = original;

        health = original.baseHp;
        rations = original.baseConstitution;
        vices = original.baseVice;
	}

    //public Item[] inventory;

}
