using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class CombatantsStatic
{
	//this is a simple class for loading up a combat scene with the desired enemy makeup


	public static Character[] combatants;


	public static void LoadCombat(params Character[] enemies) {
		combatants = enemies;
		SceneManager.LoadScene(1);
    }
}
