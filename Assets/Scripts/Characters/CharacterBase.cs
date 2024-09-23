using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CharacterBase : ScriptableObject
{
	// A CharacterBase is the designed component of the character—
	// This is only used in generating the character at the beginning of the game:
	// After this, the character is written to a save file.
	public string character_name;
	public int baseHp = 100;
	public int baseConstitution = 100;//food-ness
	public int baseVice = 100; //withdrawal-health
	public Sprite combat_sprite;
	public bool isFacingRight;
}
