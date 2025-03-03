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

	[Header("Sounds")]
	//public float fadeOutTime;
	//public bool fadingOut;
	public OneShotSource enginesrc;
	public OneShotSource roadSource;
	public OneShotSource sandSource;
	public Coroutine roadSandBlend;

	float terrainGrip;

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

	private void Start()
	{
		roadSource = SFX.GetOneShotSource().LoopFromPool(SFX.instance.road);
		roadSource.Track(transform, 2, topSpeed);
		roadSource.clipVolume = 0.4f;
		sandSource = SFX.GetOneShotSource().LoopFromPool(SFX.instance.sand);
		sandSource.Track(transform, 2, topSpeed);
		sandSource.clipVolume = 0.4f;
		enginesrc = SFX.GetOneShotSource();
		enginesrc.exteralBlend = 0.2f;
		enginesrc.blendDuration = 5;
		enginesrc.LoopFromPool(SFX.instance.engine);
		enginesrc.Track(transform, 2);
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
		//if (enginesrc == null)
		//{
		//	enginesrc = SFX.GetOneShotSource();
		//	enginesrc.Play(SFX.RandomClip(SFX.instance.engine));
		//	enginesrc.Track(transform, 2);
		//	Debug.Log("set engine src");
		//}
		enginesrc.src.pitch = 1 + Mathf.Lerp(0f, 0.5f, _rb.velocity.magnitude / topSpeed);

		float oldterrainGrip = terrainGrip;
		terrainGrip = TotalTerrainGrip();
		if (Mathf.Abs(terrainGrip - oldterrainGrip) > 0.3)
		{

			if (terrainGrip > 1.1)
			{
				Debug.Log("OnRoad");
				roadSandBlend = SFX.LerpBlend(sandSource, roadSource, 1f, roadSandBlend);
			}
			else
			{
				Debug.Log("OnSand");
				roadSandBlend = SFX.LerpBlend(roadSource, sandSource, 1f, roadSandBlend);
			}

			//}
		}

		acceleration = baseAcceleration * terrainGrip;
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
		turnAngle = turnAngle * Mathf.Min(1, _rb.velocity.magnitude / (topSpeed * 0.3f) );
		//dont oversteer
		//turnAngle = Mathf.Min(Mathf.Abs(theta), turnAngle * TotalTerrainGrip());

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
		else {
			transform.Rotate(Vector3.forward, theta * 0.5f);
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
