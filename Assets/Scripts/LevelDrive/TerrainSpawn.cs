using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainSpawn : MonoBehaviour
{
    public float spawnChance;


    // Start is called before the first frame update
    void Start()
    {
        if (Random.value > spawnChance) {
            Destroy(gameObject);
	    }
    }
}
