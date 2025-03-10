using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustStorm : MonoBehaviour
{
    public int numFX;
    public LineParticle lprefab;
    public float diff = 0.3f;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < numFX; i++) {
            LineParticle particle = Instantiate(lprefab, transform);
            particle.turb.x = 1 + i * diff;
	    }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
