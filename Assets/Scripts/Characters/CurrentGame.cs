using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CurrentGame
{
	public static Party activeParty;
	public static Character gatorHead;

	public static void NewParty(params Character[] chars) {
		activeParty = new Party(chars);
    }
}
