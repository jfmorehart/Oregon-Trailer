using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

[System.Serializable]
public class Character
{
    // This class is what stores the active version of the data that makes up a character
    // 'active' meaning malleable— anything that needs to be modified and saved. 

    //unmodifiable

    [NonSerialized]
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
        playerScalableSkills = new int[Enum.GetValues(typeof(ScalableSkill)).Length];
		playerBooleanSkills = new bool[Enum.GetValues(typeof(BooleanSkill)).Length];
	}

    //public Item[] inventory;

    public bool[] playerBooleanSkills;
    public int[] playerScalableSkills;

    public enum BooleanSkill {  //this needs to match up with the spreadsheet
        FastRunner, 
        SmoothTalking,
        AnimalHandling,
        QuickDraw,
        TerrifyingPresence
    }
    public enum ScalableSkill { 
        Constitution,
        Charisma,
        Wisdom,
        Moxie, 
        Gumption
    }

    //Boolean skills
    public bool HasSkill(BooleanSkill sk) {
        return playerBooleanSkills[(int)sk];
    }
	public bool HasBooleanSkill(int skillInt) 
	{
		return playerBooleanSkills[skillInt];
	}
	public void SetSkill(BooleanSkill sk, bool value) {
        playerBooleanSkills[(int)sk] = value;
    }

    //Scalable Skills
    public int SkillCheck(ScalableSkill sk)
    {
        return playerScalableSkills[(int)sk];
    }
    public int SkillCheck(int index)
    {
        return playerScalableSkills[index];
    }
    public void SetScalableSkill(ScalableSkill sk, int value)
    {
        playerScalableSkills[(int)sk] = value;
    }
}
