using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCamera : MonoBehaviour
{

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = MouseDriving.vanTransform.position - Vector3.forward;
    }
}
