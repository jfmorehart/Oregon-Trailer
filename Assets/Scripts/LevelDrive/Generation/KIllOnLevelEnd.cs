using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KIllOnLevelEnd : MonoBehaviour
{
    //hack until aaron uses the event system for this lol

    // Update is called once per frame
    void Update()
    {
        if (!mapUI.instance.inLevel) {
            Destroy(gameObject);
	    }
    }
}
