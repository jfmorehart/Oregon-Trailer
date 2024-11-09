using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseDriving : MonoBehaviour
{
	public static Transform vanTransform;
    Vector2 mousePos;

	[SerializeField]
    float acceleration, topSpeed, drag, turnRate, sway, deadzone, swayfreq, swayamp, rotationDrag, velConservation;
	public static Rigidbody2D rb;
    public static MouseDriving instance;
    public bool canMove = true;
	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		vanTransform = transform;
        instance = this;
        canMove = true;
	}

	private void FixedUpdate()
	{
		transform.Rotate(Vector3.forward, -sway); //undo previous frame's rotation


		//where are we going?
		mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector2 delta = mousePos - (Vector2)transform.position;
		delta.Normalize();


        //gas?
        if (canMove) {
            if (Input.GetMouseButton(0))
            {
                if (rb.velocity.magnitude < topSpeed)
                {
                    //Accelerate
                    rb.AddForce(acceleration * Time.fixedDeltaTime * transform.right);
                }
            }
            if (Input.GetMouseButton(1))
            {
                if (rb.velocity.magnitude < topSpeed)
                {
                    //Accelerate
                    rb.AddForce(acceleration * Time.fixedDeltaTime * -transform.right);
                }
            }
        }

		//how much do we need to turn?
		float theta = Vector2.SignedAngle(transform.right, delta);
		//Debug.Log(theta);

		//mousketool for later
		float rotationThisFrame = 0;

		//turn size
		float turnAngle = turnRate * Time.fixedDeltaTime;
		//gotta be moving to turn
		turnAngle = Mathf.Lerp(0, turnAngle, rb.velocity.magnitude / topSpeed); 
		//dont oversteer
		turnAngle = Mathf.Min(Mathf.Abs(theta), turnAngle);

		if (theta > deadzone) {
			
			transform.Rotate(Vector3.forward, turnAngle);
			rotationThisFrame += turnAngle;
		}
		else if(theta < -deadzone) {
			transform.Rotate(Vector3.forward, -turnAngle);
			rotationThisFrame -= turnAngle;
		}

		//wiggle the car
		sway = swayamp * (Mathf.PerlinNoise1D(Time.time * swayfreq) - 0.5f);
		sway *= Mathf.Lerp(0, swayamp, rb.velocity.magnitude / topSpeed); //scale the sway

		transform.Rotate(Vector3.forward, sway);
		//rotationThisFrame += sway; //maybe dont count the swaying

		rb.velocity *= 1 - Time.fixedDeltaTime * drag;

		rotationThisFrame = Mathf.Abs(rotationThisFrame);
		//remove some velocity for turning drag
		rb.velocity *= 1 - Time.fixedDeltaTime * Mathf.Max(1, rotationThisFrame * rotationDrag);

		//add some back in to preserve momentum in new direction
		rb.velocity += Time.fixedDeltaTime * rotationThisFrame * velConservation * (Vector2)transform.right;
	}
}
