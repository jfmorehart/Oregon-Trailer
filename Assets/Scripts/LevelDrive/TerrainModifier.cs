using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainModifier : MonoBehaviour
{
    public enum NoDuplicate { 
        None, 
        Road
    }
    public NoDuplicate terrainType;

    public float gripModifier;
    public float dragModifier;
}
