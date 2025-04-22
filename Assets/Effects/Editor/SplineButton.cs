using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(SplineEditor))]
public class SplineEditorButton : Editor
{
	public SplineEditor targetScript;
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		targetScript = GameObject.Find("SplineEditor").GetComponent<SplineEditor>();
		var myScript = targetScript as SplineEditor;
		if (targetScript == null) return;

		myScript.ResetTools = EditorGUILayout.Toggle("[Editor] Create Tools", targetScript.ResetTools); //Returns true when user clicks
																										//myScript.reloadAudio = EditorGUILayout.Toggle("[Editor] Reload Audio Button", targetScript.reloadAudio); //Returns true when user clicks

		if (myScript.ResetTools)
		{
			myScript.ResetTools = false; //this will avoid infinite errors
			Debug.Log("Spline: Rebuilding tools");
			myScript.KillTools();
			myScript.CreateTools();
			EditorUtility.SetDirty(myScript);
		}
	}
}
