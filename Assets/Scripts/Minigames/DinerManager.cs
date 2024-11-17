using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DinerManager : MonoBehaviour
{

    GameObject DinerObj;

    public static DinerManager instance;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void displayDiner()
    {
        DinerObj.SetActive(true);

    }
    public void hideDiner()
    {
        DinerObj.SetActive(false);
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
