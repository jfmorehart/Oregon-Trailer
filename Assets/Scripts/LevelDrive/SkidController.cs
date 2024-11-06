using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkidController : MonoBehaviour
{
    TrailRenderer tren;

    Vector3 lastPos;

    public float skidvmax;
    Color skcol;

    float lastcheck, delay = 0.05f;

    // Start is called before the first frame update
    void Start()
    {
        tren = GetComponent<TrailRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastcheck < delay) return;
        lastcheck = Time.time;

        Vector2 velo = (lastPos - transform.position) / delay;

        Debug.Log(velo.magnitude);

        skcol.a = Mathf.Max(0.1f, Mathf.Lerp(0, 0.5f, velo.magnitude / skidvmax));
        tren.startColor = skcol;
        lastPos = transform.position;
    }
}
