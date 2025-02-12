using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCamera : MonoBehaviour
{
	public float shakestr, shakeAmp, shakeFreq, shakeDecay;
	public Vector2 shake;
	public float shakeDistMult;

	// Update is called once per frame
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.R))
		{
			//SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
	}
	void FixedUpdate()
    {
		if (PlayerVan.vanTransform == null) return;
		transform.position = PlayerVan.vanTransform.position - Vector3.forward;
		Vector3 pos = transform.position - (Vector3)shake;

		shake = shakestr * new Vector3(shakeAmp * (Mathf.PerlinNoise1D(Time.time * shakeFreq) - 0.5f), shakeAmp * (Mathf.PerlinNoise1D(Time.time * shakeFreq + 1) - 0.5f));
		shake *= Camera.main.orthographicSize;
		//transform.position = pos + (Vector3)shake;
		shakestr *= 1 - Time.fixedDeltaTime * shakeDecay;
		if (shakestr < 0.05f) shakestr = 0;
	}

	public void Shake(float amt, Vector2 pos) {
		float dist = (pos - (Vector2)transform.position).magnitude;
		float lterm = shakeDistMult / Mathf.Pow(dist, 2);
		amt = Mathf.Lerp(0, amt, lterm);
		shakestr += amt;
    }
}
