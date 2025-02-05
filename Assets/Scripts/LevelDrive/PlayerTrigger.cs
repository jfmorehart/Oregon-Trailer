using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerTrigger : MonoBehaviour
{
	public UnityEvent onTrigger;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player")) {
			onTrigger?.Invoke();
			Debug.Log("invoked!");
		}
		else {
			Debug.Log(collision.tag + "trigger");
		}
	}
	private void OnCollisionEnter2D(Collision2D collision)
	{
		Debug.Log(collision.otherCollider.tag);
	}
}
