using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerVan : Drivable
{
	public static Transform vanTransform;
    Vector2 mousePos;
	bool levelFinished = false;

	//ill replace these with a base class or interface or smth
	public VanGun TankGun;
	public OilBarrel barrel;

	protected override void Awake()
	{
		base.Awake();
		vanTransform = transform;
	}

    private void Update()
    {
		//frame-input goes in update
		if (Input.GetKeyDown(KeyCode.R))
		{
			//SceneManager.LoadScene(SceneManager.GetActiveScene)
		}
		if (Input.GetKeyDown(KeyCode.Q))
        {
            ActivateUpgrade(UpgradeManager.instance.q_upgrade);   
		}
        if (Input.GetKeyDown(KeyCode.E))
        {
			ActivateUpgrade(UpgradeManager.instance.e_upgrade);
        }
		if (Input.GetKeyDown(KeyCode.L))
		{
			transform.position = new Vector2(ChunkManager.instance.transform.Find("goal(Clone)").position.x, ChunkManager.instance.transform.Find("goal(Clone)").position.y + 4);
		}
    }
	void ActivateUpgrade(Upgrade up)
	{
		switch (up)
		{
			case Upgrade.Booster:
                TryStartBoost();
				break;
			case Upgrade.OilBarrel:
				barrel.TryShoot();
				break;
			case Upgrade.TankGun:
				TankGun.TryShoot();
				break;
        }
	}
    protected override void FixedUpdate()
	{
		base.FixedUpdate();
		//where are we going?
		mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		
		//held input can live in fixedupdate
		//gas?
		if (Input.GetKey(KeyCode.W))
		{
			if (_rb.velocity.magnitude < topSpeed)
			{   
				//Accelerate
				_rb.AddForce(acceleration * Time.fixedDeltaTime * transform.right);
			}
		}
		if ((Input.GetKey(KeyCode.S)))
		{
			if (_rb.velocity.magnitude < topSpeed)
			{
				//Accelerate
				_rb.AddForce(acceleration * Time.fixedDeltaTime * -transform.right);
			}
		}
		bool forward = Vector2.Dot(transform.right, _rb.velocity) > 0;

		if (Input.GetKey(KeyCode.A)){
			//DriveTowards(transform.position + transform.up);
			DrivingLogic(turnRate * (forward ? 1 : -1)); ;
		}
		if (Input.GetKey(KeyCode.D))
		{
			DrivingLogic(-turnRate * (forward ? 1 : -1));
		}

	}
	public override void OnCollisionEnter2D(Collision2D collision)
	{
		base.OnCollisionEnter2D(collision);
		//Debug.Log(collision.collider.tag);
		if (collision.collider.CompareTag("Finish") && !levelFinished) {
			if (MapManager.instance != null) {
				Debug.Log("finished scene");
				MapManager.playerArrived();
				levelEnding();
				levelFinished = true;
			}
			else {
				Debug.LogError("no mapmanger instance, cannot unload level");
			}

		}
	}
    public void levelEnding()
	{
		//sends all necessary info to any necessary managers
		MapManager.instance.AddMoney(pickupValue);

	}

}
