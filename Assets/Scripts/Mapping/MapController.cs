using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{

    [SerializeField]
    List<Level> levels = new List<Level>();
    int levelsPos = 0;

    public static MapController instance;


    public void generateLevel()
    {
        levelsPos++;

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

[System.Serializable]
public struct Level
{
    [Tooltip("If this is not null, then nothing else will generate in this map. Use this for preset storyline quests")]
    public GameObject MapToGenerate;


    [Tooltip("Required Quests to generate - do not put required quests in this")]
    public TextAsset[] QuestsToGenerate;
    public int numOfNodesToGenerate;
}
