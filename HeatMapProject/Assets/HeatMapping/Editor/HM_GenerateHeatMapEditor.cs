using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

[CustomEditor(typeof(HM_GenerateHeatMap))]
public class HM_GenerateHeatMapEditor : Editor 
{
	string[] heatMapSessions, heatMapSessionFilePaths, objectList, eventList;
	int _sessionsIndex = 0, _objectIndex = 0, _eventIndex = 0;

	public override void OnInspectorGUI()
	{
		HM_GenerateHeatMap generateHeatMapScript = (HM_GenerateHeatMap)target;

		var directory = Directory.GetCurrentDirectory() + "\\HeatMapData";

		DrawDefaultInspector();

		// Get the list of session files and their filepaths.
		var tempSessionFilePaths = new List<string>();
		tempSessionFilePaths.Add(string.Empty);
		tempSessionFilePaths.AddRange(Directory.GetFiles(directory, "*xml").ToArray());
		var tempSessions = new List<string>();
		tempSessions.Add("None");
		tempSessions.AddRange(Directory.GetFiles(directory, "*xml").Select(path => Path.GetFileName(path)).ToArray());

		// Make dropdown menu for selecting files.
		heatMapSessionFilePaths = tempSessionFilePaths.ToArray();
		heatMapSessions = tempSessions.ToArray();
		GUILayout.BeginHorizontal();
		GUILayout.Label(new GUIContent("Session Data", "The session folder to load data from."), GUILayout.Width(115));
		_sessionsIndex = EditorGUILayout.Popup(_sessionsIndex, heatMapSessions);
		generateHeatMapScript.HeatMapFile = heatMapSessions[_sessionsIndex];
		GUILayout.EndHorizontal();

		if (!heatMapSessions[_sessionsIndex].Equals("None"))
		{
			// Create XMLReader for finding objects/events in the given file
			XmlReader reader = XmlReader.Create(heatMapSessionFilePaths[_sessionsIndex]);
			XmlDocument xd = new XmlDocument();
			xd.Load(reader);

			XmlElement xdRoot = xd.DocumentElement;
			XmlNodeList xmlNodes = xdRoot.SelectNodes("/TrackingData/*");

			// Get the objects that have been tracked in the given file
			var tempObjectList = new List<string>();
			foreach(XmlNode objectNode in xmlNodes)
			{
				tempObjectList.Add(objectNode.Name);
			}
			tempObjectList.Sort();
			tempObjectList.Insert(0, "All");
			objectList = tempObjectList.ToArray();

			if (_objectIndex > objectList.Length) _objectIndex = 0;

			// Create dropdown menu of the objects.
			GUILayout.BeginHorizontal();
			GUILayout.Label(new GUIContent("Object", "The object to load data for."), GUILayout.Width(115));
			_objectIndex = EditorGUILayout.Popup(_objectIndex, objectList);
			generateHeatMapScript.HeatMapObject = objectList[_objectIndex];
			GUILayout.EndHorizontal();

			// Get the events for the chosen object.
			var actualObject = (objectList[_objectIndex].Equals("All")) ? "*" : objectList[_objectIndex];
			xmlNodes = xdRoot.SelectNodes("/TrackingData/" + actualObject + "/*");
			var tempEventList = new List<string>();
			foreach(XmlNode eventNode in xmlNodes)
			{
				if (tempEventList.Contains(eventNode.Name)) continue;
				tempEventList.Add(eventNode.Name);
			}
			tempEventList.Sort();
			tempEventList.Insert(0, "All");
			eventList = tempEventList.ToArray();
			
			if (_eventIndex > eventList.Length) _eventIndex = 0;

			// Create a dropdown menu for the events.
			GUILayout.BeginHorizontal();
			GUILayout.Label(new GUIContent("Event", "The event of the object to load data for."), GUILayout.Width(115));
			_eventIndex = EditorGUILayout.Popup(_eventIndex, eventList);
			generateHeatMapScript.HeatMapEvent = eventList[_eventIndex];
			GUILayout.EndHorizontal();
		}

		// Create a button for generating the heatmap.
		GUILayout.BeginHorizontal();
		if(GUILayout.Button("Generate Heatmap", GUILayout.Height(30)))
		{
			generateHeatMapScript.Generate();
		}
		GUILayout.EndHorizontal();

		// Create a button for clearing the markers of the heatmap.
		GUILayout.BeginHorizontal();
		if(GUILayout.Button("Clear Heatmap Markers", GUILayout.Height(30)))
		{
			generateHeatMapScript.Clear();
		}
		GUILayout.EndHorizontal();

	}
}
