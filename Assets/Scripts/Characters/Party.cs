using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Party 
{
	//A party represents the current gang in the van.
	//This will be loaded in for combat or shops or whatever.

	public List<Character> members;

	public Party(params Character[] mems) {
		members = mems.ToList();
	}
}
