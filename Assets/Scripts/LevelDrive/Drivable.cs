using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drivable : MonoBehaviour
{
	public static bool cars_paused;

	[SerializeField] float baseAcceleration; //unmodified value;
	[SerializeField]
	protected float acceleration, topSpeed, drag, turnRate, sway, deadzone, swayfreq, swayamp, rotationDrag, velConservation;
	public Rigidbody2D _rb;

	bool boostActive;

	public bool boostActiveReference
	{
		get{return boostActive;}
	}

	public float boostStr, boostLength, boostCooldown;
	float lastBoostTime, boostRemaining;

	public float collisionDamage;
	Breakable breaker;

	public GameObject scrapPrefab;

	public int pickupValue; //currency im carrying

	//lists all of the stuff we're currently driving on
	//will modify the tire performance
	public List<TerrainModifier> terrainModifiers = new List<TerrainModifier>();
	protected virtual void Awake()
	{
		_rb = GetComponent<Rigidbody2D>();
		breaker = GetComponent<Breakable>();
		breaker.bar.maxHp = breaker.hp;
		breaker.onKill += OnKill;
	}

	void OnKill()
	{ 
		breaker.onKill -= OnKill;
		for (int i = 0; i < pickupValue; i++)
		{
			GameObject go = Instantiate(scrapPrefab, transform.position, transform.rotation, Pool.instance.transform);
			go.GetComponent<Rigidbody2D>().velocity = _rb.velocity + Random.insideUnitCircle * 4;
		}
	}
	protected virtual void FixedUpdate()
	{
		acceleration = baseAcceleration * TotalTerrainGrip();
		_rb.velocity *= 1 - Time.fixedDeltaTime * drag * TotalTerrainDrag();
		if (boostActive)
		{
			boostRemaining -= Time.fixedDeltaTime;
			_rb.AddForce(boostStr * Time.fixedDeltaTime * transform.right);
			//Debug.Log(boostRemaining);
			//Debug.Log("adding force");
			if (boostRemaining < 0)
			{
				boostActive = false;
			}
		}
	}
	protected virtual void TryStartBoost()
	{
		if (Time.time - lastBoostTime > boostCooldown)
		{
			//Debug.Log("boosting");
			lastBoostTime = Time.time;
			boostActive = true;
			boostRemaining = boostLength;
		}
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
		turnAngle = Mathf.Min(Mathf.Abs(theta), turnAngle * TotalTerrainGrip());

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
		sway = swayamp * (Mathf.PerlinNoise1D(Time.time * swayfreq) - 0.5f);// * TotalTerrainGrip();
		sway *= Mathf.Lerp(0, swayamp, _rb.velocity.magnitude / topSpeed); //scale the sway

		transform.Rotate(Vector3.forward, sway);
		//rotationThisFrame += sway; //maybe dont count the swaying

		rotationThisFrame = Mathf.Abs(rotationThisFrame);
		//remove some velocity for turning drag
		_rb.velocity *= 1 - Time.fixedDeltaTime * Mathf.Max(1, rotationThisFrame * rotationDrag) * TotalTerrainDrag();

		//add some back in to preserve momentum in new direction
		_rb.velocity += Time.fixedDeltaTime * rotationThisFrame * velConservation * (Vector2)transform.right;// * TotalTerrainGrip();
	}
	protected float ThetaToPoint(Vector2 point) {
		Vector2 delta = point - (Vector2)transform.position;
		delta.Normalize();
		float theta = Vector2.SignedAngle(transform.right, delta);
		return theta;
	}
	public virtual void OnCollisionEnter2D(Collision2D collision)
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

		//if (collision.collider.TryGetComponent(out TerrainModifier tm))
		//{
		//	terrainModifiers.Add(tm);
		//}
	}
    public virtual void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
		Debug.Log("Enter");
        if (collision.TryGetComponent(out TerrainModifier tm))
        {
            terrainModifiers.Add(tm);
        }

		if(collision.TryGetComponent(out Pickup pi))
		{
			pickupValue	+= pi.Collect();
		}
    }
    public virtual void OnTriggerExit2D(UnityEngine.Collider2D collision)
    {
		Debug.Log("Exit");
		if (collision.TryGetComponent(out TerrainModifier tm))
		{
			terrainModifiers.Remove(tm);
		}
	}
	protected float TotalTerrainDrag()
	{
		float total = 1;
		for (int i = 0; i < terrainModifiers.Count; i++)
		{
			total *= terrainModifiers[i].dragModifier;
		}
		return total;
	}
	protected float TotalTerrainGrip()
	{
		float total = 1;
		for (int i = 0; i < terrainModifiers.Count; i++)
		{
			total *= terrainModifiers[i].gripModifier;
		}
		return total;
	}
}
