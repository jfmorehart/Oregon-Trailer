using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerVan : Drivable
{
	public static Transform vanTransform;
    public static PlayerVan vanInstance;
    Vector2 mousePos;
	bool levelFinished = false;

	//ill replace these with a base class or interface or smth
	public VanGun TankGun;
	public Launcher GrenadeLauncher;
	public OilBarrel barrel;
	public bool canMove = true;

	protected override void Awake()
	{
		base.Awake();
		vanTransform = transform;
        vanInstance = this;
    }

    private void Update()
    {
		//frame-input goes in update
		if (Input.GetKeyDown(KeyCode.R))
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}

		if (canMove == false)
			return;

		if (Input.GetKeyDown(KeyCode.Q))
        {
            ActivateUpgrade(UpgradeManager.instance.q_upgrade);   
		}
        if (Input.GetKeyDown(KeyCode.E))
        {
			ActivateUpgrade(UpgradeManager.instance.e_upgrade);
        }
		//if (Input.GetKeyDown(KeyCode.L))
		//{
		//	transform.position = new Vector2(ChunkManager.instance.transform.Find("goal(Clone)").position.x, ChunkManager.instance.transform.Find("goal(Clone)").position.y + 4);
		//}
		//if (Input.GetKeyDown(KeyCode.LeftAlt))
		//{
		//	GetComponent<Breakable>().Damage(50);
		//}
    
		if(UpgradeManager.instance.e_upgrade == Upgrade.TankGun || UpgradeManager.instance.q_upgrade == Upgrade.TankGun) {
			//tank gun
			TankGun.gameObject.SetActive(true);
		}
		else {
			//no tank gun
			TankGun.gameObject.SetActive(false);
		}

		if (UpgradeManager.instance.e_upgrade == Upgrade.GrenadeLauncher|| UpgradeManager.instance.q_upgrade == Upgrade.GrenadeLauncher)
		{
			//launcher
			GrenadeLauncher.gameObject.SetActive(true);
		}
		else
		{
			//no launcher
			GrenadeLauncher.gameObject.SetActive(false);
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
			case Upgrade.GrenadeLauncher:
				GrenadeLauncher.TryShoot();
				break;
		}
	}
    protected override void FixedUpdate()
	{

        base.FixedUpdate();
        //where are we going?

        if (canMove == false)
            return;
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
		bool forward = true;
		if ((Input.GetKey(KeyCode.S)))
		{
			forward = false;
			if (_rb.velocity.magnitude < topSpeed)
			{
				//Accelerate
				_rb.AddForce(acceleration * Time.fixedDeltaTime * -transform.right);
			}
		}
		//bool forward = Vector2.Dot(transform.right, _rb.velocity) > 0;

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
				MapManager.playerArrived(Breaker.hp);
				levelEnding();
				levelFinished = true;
			}
			else {
				Debug.LogError("no mapmanger instance, cannot unload level");
			}

		}
		else if (collision.collider.TryGetComponent(out Breakable b) && !levelFinished)
		{
			if (b.target == true)
			{
                Debug.Log("finished scene");
                MapManager.playerArrived(Breaker.hp);
                levelEnding();
				//MapManager.winConditionReached();
                levelFinished = true;
            }
				
		}
	}
    public void levelEnding()
	{
		//sends all necessary info to any necessary managers
		MapManager.instance.AddMoney(pickupValue);
        MapManager.playerArrived(breaker.hp);

    }
}
