using System.Collections;
using System.Collections.Generic;
using Ink.Runtime;
using UnityEditor;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField] float spinSpeed;
    [SerializeField] float squishAmp, squishFreq;
    public int value;
    Rigidbody2D rb;
    [SerializeField] float drag;
    public Collider2D col;
    float startTime;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        col.enabled = false;
        startTime = Time.time;
    }
    public int Collect()
    {
        //play sound, animate, whatever
        Destroy(gameObject);
        return value;
    }
    private void Update()
    {
        if (Time.time - startTime > 0.5f && col.enabled == false)
        {
            col.enabled = true;
        }
        transform.Rotate(0, 0, spinSpeed * Time.deltaTime);
        transform.localScale = (Mathf.Sin(Time.time * squishFreq)) * squishAmp * Vector3.one + Vector3.one;
    }

    public void FixedUpdate()
    {
        rb.velocity *= 1 - Time.deltaTime * drag;
    }
}
