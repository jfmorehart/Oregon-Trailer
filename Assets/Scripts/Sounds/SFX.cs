using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SFX : MonoBehaviour
{
	public static SFX instance;

	AudioSource music_src;
	public AudioClip menu, driving;
	bool isdriving;


	public AudioClip[] road;
	public AudioClip[] sand;
	public AudioClip[] engine;
	public AudioClip[] reversing;
	public AudioClip[] revdown;
	public AudioClip[] revup;

	public SoundPool shoot;
	public SoundPool carImpact;
	public SoundPool rockImpact;
	public SoundPool houseImpact;

	//[Header("for jack use only")]
	[HideInInspector]
	public bool reloadAudio;

	public static float SFX_volume {
		get { return AudioSettings.settings_sfx_volume; }
    }

	public GameObject oneShotSourcePrefab;
	public GameObject pooledSourcePrefab;

	public static float noStereoRange = 4f;


	private void Awake()
	{
		instance = this;

		//Create all the necessary audiosources for rapid sounds like bullet impacts
		PopulateSoundPool(shoot);
		PopulateSoundPool(carImpact);
		PopulateSoundPool(houseImpact);
		PopulateSoundPool(rockImpact);

		music_src = GetComponent<AudioSource>();
	}

	private void Update()
	{
		if (isdriving)
		{
			if (PlayerVan.vanInstance == null)
			{
				isdriving = false;
				music_src.clip = menu;
				music_src.Play();
			}
		}
		else
		{
			if (PlayerVan.vanInstance != null)
			{
				isdriving = true;
				music_src.clip = driving;
				music_src.Play();
			}
		}
	}
	void PopulateSoundPool(SoundPool p) {
		p.sourcePool = new PooledSource[p.sourcePoolSize];
		for (int i = 0; i < p.sourcePoolSize; i++)
		{
			p.sourcePool[i] = Instantiate(pooledSourcePrefab, transform).GetComponent<PooledSource>();
		}
	}

	public static float Falloff(Vector2 pos) {
		float dist = Vector2.Distance(pos, Camera.main.transform.position);
		dist += Camera.main.orthographicSize;
		return Mathf.Clamp(1000 / Mathf.Pow(dist, 2), 0.1f, 0.8f);
	}

	public static OneShotSource GetOneShotSource()
	{
		return Instantiate(instance.oneShotSourcePrefab, instance.transform).GetComponent<OneShotSource>();

	}

	public static AudioClip RandomClip(AudioClip[] pool) {
		return pool[Random.Range(0, pool.Length)];
    }

	public static Coroutine LerpBlend(OneShotSource s1, OneShotSource s2, float blendDuration, Coroutine lastRoutine = null, bool intern = false) {

		if(lastRoutine != null) {
			instance.StopCoroutine(lastRoutine);
		}
		return instance.StartCoroutine(nameof(LerpBlendCoroutine), (s1, s2, blendDuration, intern));
    }

	public IEnumerator LerpBlendCoroutine( (OneShotSource, OneShotSource, float, bool) blendData ) {
		float st = Time.time;
		float v1 = blendData.Item1.exteralBlend;
		float v2 = blendData.Item2.exteralBlend;

		while (Time.time - st < blendData.Item3)
		{
			float lerpVal = (Time.time - st) / blendData.Item3;
			//Debug.Log(lerpVal);
			if (blendData.Item4)
			{
				blendData.Item1.internalBlend = 1 - lerpVal; Mathf.Lerp(v1, 0.1f, lerpVal);
				blendData.Item2.internalBlend = lerpVal; Mathf.Lerp(v2, 0.9f, lerpVal);
			}
			else {
				blendData.Item1.exteralBlend = 1 - lerpVal; Mathf.Lerp(v1, 0, lerpVal);
				blendData.Item2.exteralBlend = lerpVal; Mathf.Lerp(v2, 1, lerpVal);
			}

			yield return null;
		}
		yield break;
	}
	//public static void LerpBlend(AudioSource s1, AudioSource s2, float blendDuration) {

	//code adapted from Kiloton
	public void LoadAllAudioFiles()
	{
		road = LoadFolder("DrivingOnRoadSound");
		sand = LoadFolder("DrivingOnSandySoilSound");
		engine = LoadFolder("EngineSoundConstant");
		reversing = LoadFolder("EngineSoundReversingBack");
		revdown = LoadFolder("EngineSoundRevvingDown");
		revup = LoadFolder("EngineSoundRevvingUp");

		Debug.Log("SFX: All Files Loaded");
		shoot = new SoundPool();
		shoot.clipPool = LoadFolder("ShootingSound");
		carImpact = new SoundPool();
		carImpact.clipPool = LoadFolder("BulletHittingOnCar");
		houseImpact = new SoundPool();
		houseImpact.clipPool = LoadFolder("BulletHittingOnHouse");
		rockImpact = new SoundPool();
		rockImpact.clipPool = LoadFolder("BulletHittingOnRock");
		Debug.Log("SFX: All SoundPools created");
	}

	public AudioClip[] LoadFolder(string nof)
	{
		DirectoryInfo dir = new DirectoryInfo(Application.dataPath + "/Resources/Sounds/" + nof);
		if (!dir.Exists) {
			Debug.LogError("failed to load folder:" + Application.dataPath + "/Resources/Sounds/" + nof);
			return null;
		}
		FileInfo[] f = dir.GetFiles("*.wav");
		AudioClip[] arr = new AudioClip[f.Length];
		for (int i = 0; i < f.Length; i++)
		{
			arr[i] = LoadAudio(f[i].Name, nof);
		}
		return arr;
	}
	AudioClip LoadAudio(string name, string arrayname)
	{
		name = name.Replace(".wav", "");
		Debug.Log("loading " + "Sounds/" + arrayname + "/" + name);
		return Resources.Load<AudioClip>("Sounds/" + arrayname + "/" + name);
	}
}
