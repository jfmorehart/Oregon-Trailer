using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilSlick : MonoBehaviour
{
    public float lifeTime;
    float startTime;
    SpriteRenderer ren;
    private void Awake()
    {
        ren = GetComponent<SpriteRenderer>();
    }
    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - startTime > lifeTime)
        {
            Destroy(gameObject);
        }
        Color c = ren.color;
        c.a = Mathf.Lerp(1, 0, (Time.time - startTime) / lifeTime);
        ren.color = c;
    }
}
