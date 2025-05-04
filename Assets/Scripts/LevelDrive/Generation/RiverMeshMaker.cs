using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiverMeshMaker : MonoBehaviour
{
	public GameObject riverMesh;
	public GameObject debug_point;

	MeshFilter[] mesh_filters;
	Transform[] mesh_nodes;
	Transform[]	influenceA_nodes;
	Transform[] influenceB_nodes;

	public void Generate(Vector2[] points) {
		mesh_filters = new MeshFilter[points.Length];
		mesh_nodes = new Transform[points.Length];
		influenceA_nodes = new Transform[points.Length];
		influenceB_nodes = new Transform[points.Length];

		for (int i = 1; i < points.Length + 1; i++)
		{

			GameObject go = Instantiate(debug_point, points[i], Quaternion.identity, transform);
			mesh_nodes[i] = go.transform;
			if (i < points.Length + 1)
			{
				Vector2 infPoint;
				if (i > 1) {
					//driven 
					Vector3 deltaOne = points[i] - (Vector2)influenceB_nodes[i - 1].transform.position;
					infPoint = points[i] + (Vector2)deltaOne;

				}
				else {
					infPoint = points[i] - Vector2.right;
				}

				GameObject inf = Instantiate(debug_point, infPoint, Quaternion.identity);//, go.transform);
				influenceA_nodes[i] = inf.transform;

				infPoint = points[i + 1] + Vector2.right;
				GameObject infB = Instantiate(debug_point, infPoint, Quaternion.identity);
				influenceB_nodes[i] = infB.transform;
			}
			GameObject meshGo = Instantiate(riverMesh, mesh_nodes[i].transform.position, Quaternion.identity);
			MeshFilter mf = meshGo.GetComponent<MeshFilter>();
			mesh_filters[i] = mf;
		}

		Render();
	}

	public void Render() {
		for (int i = 0; i + 1 < mesh_filters.Length; i++)
		{
			MeshFilter mf = mesh_filters[i];
			Mesh m = mf.sharedMesh;
			if (m == null)
			{
				mf.sharedMesh = new Mesh();
				m = mf.sharedMesh;
			}
			if (i > 0)
			{
				//mesh_filters[i].transform.position = mesh_nodes[i - 1].transform.position;
			}
			Vector3 prev = Vector3.zero;
			Vector3 next = Vector3.zero;

			if (i > 0)
			{
				if (mesh_filters[i - 1].sharedMesh.vertices.Length > 0)
				{
					prev = mesh_filters[i - 1].sharedMesh.vertices[^2];
				}

			}
			if (i + 1 < mesh_filters.Length)
			{
				if (mesh_filters[i + 1].sharedMesh != null)
				{
					if (mesh_filters[i + 1].sharedMesh.vertices.Length > 0)
					{
						next = mesh_filters[i + 1].sharedMesh.vertices[1];
					}
				}
			}
			mesh_filters[i].transform.eulerAngles = new Vector3(0, 0, 0);// -transform.eulerAngles.z);
	
			var meshData = MeshGen.GenerateMesh(mesh_nodes[i], influenceA_nodes[i].transform, influenceB_nodes[i].transform, mesh_nodes[i + 1], prev, next);
			//Debug.Log(meshData.Item1.Length + " " + m);
			m.vertices = meshData.Item1;
			m.uv = meshData.Item2;
			m.triangles = meshData.Item3;
			mesh_filters[i].sharedMesh = m;
		}

	}
}
