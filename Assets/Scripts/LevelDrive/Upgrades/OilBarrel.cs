using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilBarrel : MonoBehaviour
{
    public GameObject oilPrefab;

    float lastFire;
    float rechamberTime = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TryShoot()
    {
        if (Time.time - lastFire > rechamberTime)
        {
            lastFire = Time.time;
            Shoot();
        }
    }

    void Shoot()
    {
        Instantiate(oilPrefab, transform.position, transform.rotation, Pool.instance.transform);
    }
}
