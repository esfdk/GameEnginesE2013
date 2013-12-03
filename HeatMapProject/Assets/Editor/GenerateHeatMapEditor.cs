using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(GenerateHeatMap))]
public class GenerateHeatMapEditor : Editor {

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		GenerateHeatMap generateHeatMapScript = (GenerateHeatMap)target;

		if(GUILayout.Button("Generate Heatmap", GUILayout.Width(150), GUILayout.Height(30)))
		{
			generateHeatMapScript.Generate();
		}
	}
}
