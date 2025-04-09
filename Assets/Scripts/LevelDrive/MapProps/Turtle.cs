using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turtle : MonoBehaviour
{
	Rigidbody2D rb;
    Renderer renderer;
	public float speed;

	private void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		renderer = GetComponent<Renderer>();
	}
	// Update is called once per frame
	void Update()
    {
		if (renderer.isVisible) {
			rb.AddForce(speed * Time.deltaTime * transform.right);
			//transform.Translate(speed * Time.deltaTime * transform.right, Space.World);
		}
    }
}
