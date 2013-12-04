using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Linq;

[CustomEditor(typeof(GenerateHeatMap))]
public class GenerateHeatMapEditor : Editor 
{
	string[] heatMapSessions;
	string[] heatMapfiles;
	int _sessionsChoiceIndex = 0;
	int _filesChoiceIndex = 0;

	public override void OnInspectorGUI()
	{
		GenerateHeatMap generateHeatMapScript = (GenerateHeatMap)target;

		var directory = Directory.GetCurrentDirectory() + "\\HeatMapData";

		DrawDefaultInspector();
		
		heatMapSessions = Directory.GetDirectories(directory);
		for (int i = 0; i < heatMapSessions.Length; i++)
		{
			heatMapSessions[i] = heatMapSessions[i].Remove(0, directory.Length + 1);
		}
		GUILayout.BeginHorizontal();
		GUILayout.Label(new GUIContent("Session Folder", "The session folder to load data from."), GUILayout.Width(115));
		_sessionsChoiceIndex = EditorGUILayout.Popup(_sessionsChoiceIndex, heatMapSessions);
		GUILayout.EndHorizontal();
		
		heatMapfiles = Directory.GetFiles(directory + "\\" + heatMapSessions[_sessionsChoiceIndex], "*.xml").Select(path => Path.GetFileName(path)).ToArray();		
		GUILayout.BeginHorizontal();
		GUILayout.Label(new GUIContent("Heat Map Data", "The data file to make a heat map for."), GUILayout.Width(115));
		_filesChoiceIndex = EditorGUILayout.Popup(_filesChoiceIndex, heatMapfiles);
		generateHeatMapScript.HeatMapData = heatMapfiles[_filesChoiceIndex].Replace(".xml", string.Empty);
		generateHeatMapScript.SessionFolder = heatMapSessions[_sessionsChoiceIndex];
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		if(GUILayout.Button("Generate Heatmap", GUILayout.Height(30)))
		{
			generateHeatMapScript.Generate();
		}
		GUILayout.EndHorizontal();
		
		GUILayout.BeginHorizontal();
		if(GUILayout.Button("Clear Heatmap Markers", GUILayout.Height(30)))
		{
			generateHeatMapScript.Clear();
		}
		GUILayout.EndHorizontal();

	}
}
