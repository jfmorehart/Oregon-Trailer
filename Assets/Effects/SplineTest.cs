using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class SplineTest : MonoBehaviour
{
    LineRenderer lr;
	Mesh mesh;

	public int steps =10;

	private void Awake()
	{
		lr = GetComponent<LineRenderer>();
		mesh = GetComponent<MeshFilter>().mesh;
	}
	// Start is called before the first frame update

	private void Update()
	{
		//mesh = MeshGen.GenerateMesh(transform.GetChild(0), transform.GetChild(1), steps);
	}
}
