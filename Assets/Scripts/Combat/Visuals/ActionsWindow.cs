using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ActionsWindow : MonoBehaviour
{
    public static ActionsWindow Instance;

    Vector2 origin;
    Vector2 stowed;

    public float v;
    public float accel, drag;

    public TMP_Text nameText;

    public GameObject OptionsMenu;

	private void Awake()
	{
        Instance = this;
	}
	// Start is called before the first frame update
	void Start()
    { 
        origin = transform.position;
        stowed = origin - Vector2.up * 300;
        transform.position = stowed;
    }

    // Update is called once per frame
    void Update()
    {
        if (CombatManager.Instance.selectedNode != -1) {
            if (v < accel) v = accel;
            v += accel * Time.deltaTime;
        }
        else {
            if (v > -accel) v = -accel;
			v -= accel * Time.deltaTime;
		}
        v *= 1 - Time.deltaTime * drag;

        transform.position += Time.deltaTime * v * Vector3.up;

        if (transform.position.y > origin.y) {
            transform.position = new Vector3(transform.position.x, origin.y, 0);
            v = 0;
		}
		if (transform.position.y < stowed.y)
		{
			transform.position = new Vector3(transform.position.x, stowed.y, 0);
			v = 0;
		}
	}
}
