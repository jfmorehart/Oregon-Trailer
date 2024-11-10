using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drivable : MonoBehaviour
{
	[SerializeField]
	protected float acceleration, topSpeed, drag, turnRate, sway, deadzone, swayfreq, swayamp, rotationDrag, velConservation;
	public Rigidbody2D _rb;

	public float collisionDamage;
	Breakable breaker;

	protected virtual void Awake()
	{
		_rb = GetComponent<Rigidbody2D>();
		breaker = GetComponent<Breakable>();
		breaker.bar.maxHp = breaker.hp;
	}
	protected virtual void FixedUpdate()
	{
		_rb.velocity *= 1 - Time.fixedDeltaTime * drag;
	}
	protected float ThetaToPoint(Vector2 point) {
		Vector2 delta = point - (Vector2)transform.position;
		delta.Normalize();
		float theta = Vector2.SignedAngle(transform.right, delta);
		return theta;
	}
	protected void DriveTowards(Vector2 point) {
		DrivingLogic(ThetaToPoint(point));
	}
	protected void DrivingLogic(float theta)
	{
		transform.Rotate(Vector3.forward, -sway); 
	
		//undo previous frame's rotation
		//how much do we need to turn?
		//Debug.Log(theta);

		//mousketool for later
		float rotationThisFrame = 0;

		//turn size
		float turnAngle = turnRate * Time.fixedDeltaTime;
		//gotta be moving to turn
		turnAngle = Mathf.Lerp(0, turnAngle, _rb.velocity.magnitude / topSpeed);
		//dont oversteer
		turnAngle = Mathf.Min(Mathf.Abs(theta), turnAngle);

		if (theta > deadzone)
		{

			transform.Rotate(Vector3.forward, turnAngle);
			rotationThisFrame += turnAngle;
		}
		else if (theta < -deadzone)
		{
			transform.Rotate(Vector3.forward, -turnAngle);
			rotationThisFrame -= turnAngle;
		}

		//wiggle the car
		sway = swayamp * (Mathf.PerlinNoise1D(Time.time * swayfreq) - 0.5f);
		sway *= Mathf.Lerp(0, swayamp, _rb.velocity.magnitude / topSpeed); //scale the sway

		transform.Rotate(Vector3.forward, sway);
		//rotationThisFrame += sway; //maybe dont count the swaying

		rotationThisFrame = Mathf.Abs(rotationThisFrame);
		//remove some velocity for turning drag
		_rb.velocity *= 1 - Time.fixedDeltaTime * Mathf.Max(1, rotationThisFrame * rotationDrag);

		//add some back in to preserve momentum in new direction
		_rb.velocity += Time.fixedDeltaTime * rotationThisFrame * velConservation * (Vector2)transform.right;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.collider.TryGetComponent(out Breakable br))
		{
			Debug.Log("smack " + _rb.velocity.magnitude);
			if (_rb.velocity.magnitude > 1)
			{
				br.Damage(collisionDamage);
				breaker.Damage(collisionDamage * 0.5f);
				Pool.smokes.GetObject().Fire(collision.contacts[0].point, Vector2.zero, Vector2.zero);
			}
		}
	}
}
