using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostEffect : MonoBehaviour
{
    public Drivable carObject; 
    public GameObject boostEffect;
    bool isBoosting;
    void Start()
    {
        carObject = carObject.GetComponent<Drivable>();
        isBoosting = carObject.boostActiveReference;
    }

    // Update is called once per frame
    void Update()
    {
        isBoosting = carObject.boostActiveReference;
        boostEffect.SetActive(isBoosting); //set the effects object to be on or off depending on if youre boosting
    }
}
