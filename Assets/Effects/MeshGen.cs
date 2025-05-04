using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGen : MonoBehaviour
{
	//creates a custom mesh between points Start and Dest with influence point Infl


	public static (Vector3[], Vector2[], int[]) GenerateMesh(Transform me, Transform influA, Transform influB, Transform desti, Vector3 prev, Vector3 next, int steps = 3, float extrusion = 5)
	{
		Vector2 start = me.transform.position;
		Vector2 infA = influA.position;//.localPosition * 2;
		Vector2 infB = influB.position;//.localPosition * 2;
		Vector2 dest = desti.position;// 2f * (desti.position - me.position);//.localPosition;

		Vector3[] coreSpline = MeshGen.GenerateCoreSpline(me.root.position, start, dest, infA, infB, steps);
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
	static Vector2 lerpSA, lerpAB, lerpBD, lerpm1, lerpm2, lerpfinal;
	public static Vector3[] GenerateCoreSpline(Vector2 origin, Vector2 start, Vector2 dest, Vector2 inflA, Vector2 inflB, int steps = 10) {

		float lerpval;
		points.Clear();
		steps -= 2;
		points.Add(Vector3.zero);// - start);
		for (int i = 0; i < steps; i++) {
			lerpval = i / (float)steps;

			//start = start;
			//tier one
			lerpSA = Vector2.Lerp(start, inflA, lerpval);
			//Debug.DrawLine(start, inflA);
			lerpAB = Vector2.Lerp(inflA, inflB, lerpval);
			//Debug.DrawLine(start, inflB);
			lerpBD = Vector2.Lerp(inflB, dest, lerpval);

			//tier two
			lerpm1 = Vector2.Lerp(lerpSA, lerpAB, lerpval);
			//Debug.DrawLine(lerpSA, lerpAB);
			lerpm2 = Vector2.Lerp(lerpAB, lerpBD, lerpval);
			//Debug.DrawLine(lerpAB, lerpBD);
			//tier 3
			lerpfinal = Vector2.Lerp(lerpm1, lerpm2, lerpval);
			//Debug.DrawLine(lerpm1, lerpm2, Color.red);
			points.Add((lerpfinal - start) );
		}
		points.Add((dest - start) );
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
