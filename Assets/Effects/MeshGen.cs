using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGen : MonoBehaviour
{
	//creates a custom mesh between points Start and Dest with influence point Infl


	public static (Vector3[], Vector2[], int[]) GenerateMesh(Transform me, Transform influ, Transform desti, Vector3 prev, Vector3 next, int steps = 10, float extrusion = 5)
	{
		Vector2 start = Vector2.zero;// me.localPosition;
		Vector2 inf = influ.localPosition;
		Vector2 dest = 2f * (desti.position - me.position);//.localPosition;

		Vector3[] coreSpline = MeshGen.GenerateCoreSpline(start, dest, inf, steps);
		Vector3[] rightSpline = MeshGen.GenerateExtrusion(coreSpline, extrusion);
		Vector3[] leftSpline = MeshGen.GenerateExtrusion(coreSpline, -extrusion);
		//fix left/right extrusions on first and last indices

		List<Vector3> verts = new List<Vector3>();
		List<Vector2> uvs = new List<Vector2>();
		List<int> tris = new List<int>();
		Vector2 half = new Vector2(0.5f, 0);
		Vector2 onehalf = new Vector2(0.5f, 1);
		for (int i = 0; i < steps; i++)
		{
			verts.Add(leftSpline[i]);
			verts.Add(coreSpline[i]);
			verts.Add(rightSpline[i]);
			//if(i + 1 < steps) {
			//	verts.Add(leftSpline[i + 1]);
			//	verts.Add(coreSpline[i + 1]);
			//	verts.Add(rightSpline[i + 1]);
			//}
		}
		for (int i = 0; i < verts.Count; i += 6)
		{
			//bottom layer
			uvs.Add(Vector2.zero);
			uvs.Add(half);
			uvs.Add(Vector2.right);

			//top layer
			uvs.Add(Vector2.up);
			uvs.Add(onehalf);
			uvs.Add(Vector2.one);
		}

		for (int i = 0; i + 5 < verts.Count; i += 3)
		{
			//bottom layer
			//uvs.Add(Vector2.zero);
			//uvs.Add(half);
			//uvs.Add(Vector2.right);

			////top layer
			//uvs.Add(Vector2.up);
			//uvs.Add(onehalf);
			//uvs.Add(Vector2.one);

			//tri
			//clockwise?
			//tris.Add(i + 0); tris.Add(i + 3); tris.Add(i + 4);
			//tris.Add(i + 4); tris.Add(i + 1); tris.Add(i + 0);
			//tris.Add(i + 4); tris.Add(i + 5); tris.Add(i + 2);
			//tris.Add(i + 2); tris.Add(i + 1); tris.Add(i + 4);

			//right hand rule
			tris.Add(i + 0); tris.Add(i + 1); tris.Add(i + 4);
			tris.Add(i + 1); tris.Add(i + 2); tris.Add(i + 4);
			tris.Add(i + 2); tris.Add(i + 5); tris.Add(i + 4);
			tris.Add(i + 0); tris.Add(i + 4); tris.Add(i + 3);
		}

		return (verts.ToArray(), uvs.ToArray(), tris.ToArray());
	}

	static List<Vector3> points= new List<Vector3>();
	static Vector2 lerpSI, lerpID, lerpSD;
	public static Vector3[] GenerateCoreSpline(Vector2 start, Vector2 dest, Vector2 infl, int steps = 10) {

		float lerpval;
		points.Clear();
		steps -= 2;
		points.Add(start);
		for (int i = 0; i < steps; i++) {
			lerpval = i / (float)steps;
			lerpSI = Vector2.Lerp(start, infl, lerpval);
			lerpID = Vector2.Lerp(infl, dest, lerpval);
			lerpSD = Vector2.Lerp(lerpSI, lerpID, lerpval);
			points.Add(lerpSD);
		}
		points.Add(dest);
		return points.ToArray();
    }

	static Vector3 prev, next, average;
	static float angle;

	public static Vector3[] GenerateExtrusion(Vector3[] centerLine, float extrustionLength = 10) {
		points.Clear();
		for(int i = 0; i < centerLine.Length; i++) {
			if(i ==0) {
				next = centerLine[i + 1] - centerLine[i];

				//deconstruct and rotate vector
				average = next;
				angle = Mathf.Atan2(average.y, average.x);
				angle += 90 * Mathf.Deg2Rad;

				//rebuild rotated vector
				average = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle));
				points.Add(centerLine[i] + average * extrustionLength);
				continue;
			}
			if (i == centerLine.Length - 1)
			{
				prev = centerLine[i] - centerLine[i - 1];

				//deconstruct and rotate vector
				average = prev;
				angle = Mathf.Atan2(average.y, average.x);
				angle += 90 * Mathf.Deg2Rad;
				//if (angle < 0) angle += 2 * Mathf.PI;
				//rebuild rotated vector
				average = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle));
				points.Add(centerLine[i] + average * extrustionLength);
				continue;
			}
			//average prev and next vector
			prev = centerLine[i] - centerLine[i - 1];
			next = centerLine[i + 1] - centerLine[i];

			//deconstruct and rotate vector
			average = (prev + next) * 0.5f;
			angle = Mathf.Atan2(average.y, average.x);
			angle += 90 * Mathf.Deg2Rad;

			//rebuild rotated vector
			average = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle));
			points.Add(centerLine[i] + average * extrustionLength);
		}
		return points.ToArray();
    }

}
