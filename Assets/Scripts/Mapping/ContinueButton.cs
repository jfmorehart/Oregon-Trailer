using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContinueButton : MonoBehaviour
{
    bool active;
    Button ren;
    GameObject kid;
    Image image;

	private void Awake()
	{
        image = GetComponent<Image>();
        kid = transform.GetChild(0).gameObject;
        ren = GetComponent<Button>();
	}
	// Update is called once per frame
	void Update()
    {
        if(GameManager.VanRunning != active) {
            ToggleActive(GameManager.VanRunning); //set it to match
	    }
    }

    void ToggleActive(bool setactive) {
        active = setactive;
        image.enabled = setactive;
        kid.SetActive(setactive);
        ren.enabled = setactive; 
    }
}
