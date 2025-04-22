using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class SplineEditor : MonoBehaviour
{
	[HideInInspector]
	public bool ResetTools; //used for editorbutton



	public GameObject nodePrefab;
	public GameObject influencePrefab;
	public GameObject meshPrefab;

	public float spacer, segments;

	public List<GameObject> all_tools = new List<GameObject>();
	public List<GameObject> mesh_nodes = new List<GameObject>();
	public List<MeshFilter> mesh_filters = new List<MeshFilter>();
	public List<GameObject> influence_nodes = new List<GameObject>();

	public bool EnforceContinuity;

	private void Update()
	{
		if (EnforceContinuity) {
			UpdateHandles();
		}
		UpdateMeshes();
	}
	public void UpdateHandles() { 
		for(int i = 1; i + 1 < influence_nodes.Count; i++) {
			//float angle = 
			Vector3 deltaOne = mesh_nodes[i].transform.position - influence_nodes[i - 1].transform.position;
			influence_nodes[i].transform.position = mesh_nodes[i].transform.position + deltaOne;
		}
    }
	public void UpdateMeshes() {
		Debug.Log("update");
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
			if (i + 2 < mesh_filters.Count)
			{
				if(mesh_filters[i + 1].sharedMesh != null) {
					if (mesh_filters[i + 1].sharedMesh.vertices.Length > 0)
					{
						next = mesh_filters[i + 1].sharedMesh.vertices[1];
					}
				}
			}


			var meshData = MeshGen.GenerateMesh(mesh_nodes[i].transform, influence_nodes[i].transform, mesh_nodes[i + 1].transform, prev, next);
			//Debug.Log(meshData.Item1.Length + " " + m);
			m.vertices = meshData.Item1;
			m.uv = meshData.Item2;
			m.triangles = meshData.Item3;
			mesh_filters[i].sharedMesh = m;
			Debug.Log("assign");
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
		influence_nodes.Clear();
		mesh_filters.Clear();

		Vector2 pos = Vector2.zero;
		for(int i = 0; i < segments; i++) {
			GameObject go = Instantiate(nodePrefab, pos, Quaternion.identity, transform);
			all_tools.Add(go);
			mesh_nodes.Add(go);
			pos += Vector2.right * spacer;
			GameObject inf = Instantiate(influencePrefab, pos, Quaternion.identity, go.transform);
			all_tools.Add(inf);
			influence_nodes.Add(inf);
			pos += Vector2.right * spacer;

			if(i + 1 < segments) {
				GameObject meshGo = Instantiate(meshPrefab, mesh_nodes[i].transform.position, Quaternion.identity, go.transform);
				all_tools.Add(meshGo);
				MeshFilter mf = meshGo.GetComponent<MeshFilter>();
				mesh_filters.Add(mf);
			}

		}
    }
}
