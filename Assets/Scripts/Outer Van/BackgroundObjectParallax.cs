using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundObjectParallax : MonoBehaviour
{
    //object will come into view and stop depending on if the van is stopped
    [SerializeField]
    private float speed = 2;

    [SerializeField]
    private Vector2 startPos;
    [SerializeField]
    private Vector2 endPos;
    private void Start()
    {
        
    }
    void Update()
    {
        //move to the right depending on if the van is running.
        if (GameManager.VanRunning && endPos.x > transform.position.x)
        {
            transform.position = Vector2.MoveTowards(transform.position, endPos, speed *Time.deltaTime);
        }
        else if(endPos.x < transform.position.x)
        {
            transform.position = startPos;
        }
    }

}
