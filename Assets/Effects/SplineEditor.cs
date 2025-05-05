using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

[ExecuteInEditMode]
public class SplineEditor : MonoBehaviour
{
	//[HideInInspector]

	public GameObject nodePrefab;
	public GameObject influencePrefab;
	public GameObject influenceBPrefab;
	public GameObject meshPrefab;

	public float spacer, segments;
	public int steps;
	public float width = 10;
	public Material roadMat;

	public List<GameObject> all_tools = new List<GameObject>();
	public List<GameObject> mesh_nodes = new List<GameObject>();
	public List<MeshFilter> mesh_filters = new List<MeshFilter>();
	public List<GameObject> influenceA_nodes = new List<GameObject>();
	public List<GameObject> influenceB_nodes = new List<GameObject>();

	public bool EnforceContinuity;
	public bool ResetTools; //used for editorbutton
	public bool CreateColliders;


	[ExecuteInEditMode]
	private void Update()
	{
		if(Application.isPlaying)return;

		if (ResetTools) {
			ResetTools = false; //this will avoid infinite errors
			KillTools();
			CreateTools();
		}
		if (CreateColliders) {
			CreateColliders = false;
			BuildMeshColliders();
		}
		if (EnforceContinuity) {
			UpdateHandles();
		}
		UpdateMeshes();

	}
	public void BuildMeshColliders() {
		Debug.Log("building colliders");
		List<Vector2> tris = new List<Vector2>();
		for (int i = 0; i + 1 < mesh_filters.Count; i++)
		{
			Debug.Log("building collider  " + i);
			MeshFilter mf = mesh_filters[i];
			Mesh m = mf.sharedMesh;
			GameObject go = mesh_filters[i].gameObject;
			PolygonCollider2D col;
			if (go.TryGetComponent(out MeshCollider mcol))
			{
				DestroyImmediate(mcol);
			}
			if (go.TryGetComponent(out PolygonCollider2D collider))
			{
				col = collider;
			}
			else
			{
				col = go.AddComponent<PolygonCollider2D>();
			}
			TerrainModifier terr;
			if(go.TryGetComponent(out TerrainModifier tm)) {
				terr = tm;
			}
			else {
				terr = go.AddComponent<TerrainModifier>();
			}
			terr.gripModifier = 1.4f;
			terr.dragModifier = 0.9f;
			if (col == null) {
				Debug.Log("failed, no collider on object " + i);
			}
			col.isTrigger = true;
			Debug.Log(col);
			col.pathCount = (int)steps;
			int pathNumber = 0;
			//left side


			for (int vert = 0; vert + 5 < m.vertices.Length; vert += 3)
			{
				tris.Clear();
				tris.Add(m.vertices[vert + 0]);
				tris.Add(m.vertices[vert + 2]);
				tris.Add(m.vertices[vert + 5]);
				tris.Add(m.vertices[vert + 3]);
				tris.Add(m.vertices[vert + 0]);
				//foreach(Vector2 tri in tris) {
				//	Debug.Log(tri);
				//}
				col.SetPath(pathNumber, tris.ToArray());

				Debug.Log("path " + pathNumber + " = " + (vert + 0) + " " + (vert + 2) + " " + (vert + 5) + "  " + (vert + 0));
				pathNumber++;
			}
			//for (int segs = 0; segs + 1 < steps; segs++)
			//{
			//	Debug.Log("testing" + (segs * 3) + "  " + (segs * 3 + 3) + "out of " + m.vertices.Length);
			//	triangle = new Vector2[2] { m.vertices[segs * 3], m.vertices[segs * 3 + 3]};
			//	Debug.Log("path= " + pathNumber + " " + triangle[0] + " " + triangle[1]);
			//	Debug.DrawLine(triangle[0], triangle[1], Color.red, 10);
			//	col.SetPath(segs, triangle);
			//	pathNumber++;
			//}
			////top
			//triangle = new Vector2[2] { m.vertices[m.vertices.Length - 3], m.vertices[m.vertices.Length - 1] };
			//Debug.Log(triangle[0] + " " + triangle[1]);
			//Debug.DrawLine(triangle[0], triangle[1], Color.red, 10);
			//col.SetPath(pathNumber, triangle);
			//pathNumber++;

			////right side
			//for (int seg = steps - 1; seg > 1; seg--)
			//{
			//	triangle = new Vector2[2] { m.vertices[seg * 3 + 2], m.vertices[seg * 3 - 1] };
			//	Debug.Log(triangle[0] + " " + triangle[1]);
			//	Debug.DrawLine(triangle[0], triangle[1], Color.red, 10);
			//	col.SetPath(seg, triangle);
			//	pathNumber++;
			//}
			//triangle = new Vector2[2] { m.vertices[2], m.vertices[0] };
			//Debug.Log(triangle[0] + " " + triangle[1]);
			//Debug.DrawLine(triangle[0], triangle[1], Color.red, 10);
			//col.SetPath(pathNumber, triangle);
			//pathNumber++;
		}
	}
	public void UpdateHandles() { 
		for(int i = 1; i < influenceA_nodes.Count; i++) {
			//float angle = 
			Vector3 deltaOne = mesh_nodes[i].transform.position - influenceB_nodes[i - 1].transform.position;
			influenceA_nodes[i].transform.position = mesh_nodes[i].transform.position + deltaOne;
		}
    }
	public void UpdateMeshes() {

		for(int i = 0; i + 1 < mesh_filters.Count; i++) {
			MeshFilter mf = mesh_filters[i];
			Mesh m = mf.sharedMesh;
			if (m == null)
			{
				mf.sharedMesh = new Mesh();
				m = mf.sharedMesh;
			}
			if(i > 0) {
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
			if (i + 1 < mesh_filters.Count)
			{
				if(mesh_filters[i + 1].sharedMesh != null) {
					if (mesh_filters[i + 1].sharedMesh.vertices.Length > 0)
					{
						next = mesh_filters[i + 1].sharedMesh.vertices[1];
					}
				}
			}
			mesh_filters[i].transform.eulerAngles = new Vector3(0, 0, 0);// -transform.eulerAngles.z);

			var meshData = MeshGen.GenerateMesh(mesh_nodes[i].transform, influenceA_nodes[i].transform, influenceB_nodes[i].transform, mesh_nodes[i + 1].transform, prev, next, steps, width);
			//Debug.Log(meshData.Item1.Length + " " + m);
			m.vertices = meshData.Item1;
			m.uv = meshData.Item2;
			m.triangles = meshData.Item3;
			mesh_filters[i].sharedMesh = m;

			mesh_filters[i].gameObject.GetComponent<MeshRenderer>().material = roadMat;
		}
	}
	public void KillTools() { 
		foreach(GameObject pre in all_tools) {
			DestroyImmediate(pre);
		}
		all_tools.Clear();
    }
	public void CreateTools() {

		mesh_nodes.Clear();
		influenceA_nodes.Clear();
		influenceB_nodes.Clear();
		mesh_filters.Clear();

		Vector2 pos = transform.position - Vector3.right * (segments * spacer * 1.5f);
		for(int i = 0; i < segments + 1; i++) {
			GameObject go = Instantiate(nodePrefab, pos, Quaternion.identity, transform);
			if (i > 0)
			{
				//parent the last guy's B node to us!
				influenceB_nodes[^1].transform.parent = go.transform;
			}
			all_tools.Add(go);
			mesh_nodes.Add(go);

			if(i  < segments) {
				pos += Vector2.right * spacer;
				GameObject inf = Instantiate(influencePrefab, pos, Quaternion.identity, go.transform);
				all_tools.Add(inf);
				influenceA_nodes.Add(inf);

				pos += Vector2.right * spacer;
				GameObject infB = Instantiate(influenceBPrefab, pos, Quaternion.identity, go.transform);
				all_tools.Add(infB);
				influenceB_nodes.Add(infB);
				pos += Vector2.right * spacer;
			}
			GameObject meshGo = Instantiate(meshPrefab, mesh_nodes[i].transform.position, Quaternion.identity, go.transform);
			all_tools.Add(meshGo);
			MeshFilter mf = meshGo.GetComponent<MeshFilter>();
			mesh_filters.Add(mf);
		}
    }
}
