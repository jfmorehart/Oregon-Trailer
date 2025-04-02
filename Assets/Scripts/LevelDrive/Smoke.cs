using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smoke : PooledObject
{
    Color col;
    Color startcol;
    Vector2 defaultScale;
    Vector2 startScale;

    Renderer ren;


    bool live;
    public float lifeTime, fadeSpeed;
    float startTime;

    private void Awake()
	{
        ren = GetComponent<Renderer>();
        col = CopyColor(ren.material.color);
        startcol = CopyColor(col);
        defaultScale = transform.localScale;
        Hide();
	}

    public override void Hide() {
        live = false;
        ren.enabled = false;

    }

    public override void Fire(Vector2 pos, Vector2 scale, Vector2 __) {
        col = CopyColor(startcol);
        live = true;

		//often zeroed out, but usable as a scale modifier
		if (scale != Vector2.zero) {

            startScale = scale;
        }
        else {
            startScale = defaultScale;
	    }
        transform.localScale = startScale;
	    transform.position = pos;
        startTime = Time.time;
        ren.enabled = true;

    }

    // Update is called once per frame
    void Update()
    {
        if (!live) return;
        if (Time.time - startTime > lifeTime) {
            Hide();
            return;
	    }
        float lval = (Time.time - (startTime - 0.01f)) / lifeTime;
        float scale = (1 - lval) * 2;
        transform.localScale = startScale * scale;
  //      if(Time.time - startTime < lifeTime * 0.5f) {
  //	col.a *= 1 + Time.deltaTime * fadeSpeed;
  //	transform.localScale *= 1 + Time.deltaTime * fadeSpeed;
  //}
  //      else {
  //	col.a *= 1 - Time.deltaTime * fadeSpeed * 2;
  //	transform.localScale *= 1 - Time.deltaTime * fadeSpeed * 2;
  //}

	}

    Color CopyColor(Color old) {
        //i think these are pass by reference so
        return new Color(old.r, old.g, old.b, old.a);
    }
}
