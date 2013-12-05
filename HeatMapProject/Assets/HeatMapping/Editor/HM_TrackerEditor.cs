using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(HM_Tracker))]
public class HM_TrackerEditor : Editor 
{
	private bool DefaultTrackedEvents_FoldOut = false;
	private GUIContent defaultTrackedEvents = new GUIContent("Tracked Events", "The events that can be tracked by default");

	private Dictionary<string, string> _eventTooltips = new Dictionary<string, string>()
	{
		{ HM_EventTypes.Awake, "Send an event when object is initialized." },
		{ HM_EventTypes.Destroy, "Send an even when the object is destroyed." }
	};
	
	override public void OnInspectorGUI ()
	{
		HM_Tracker hmT = (HM_Tracker)target;
	
		// Creates a group that lets the user select the position interval.
		GUILayout.BeginHorizontal();
		GUILayout.Label(new GUIContent("Position interval", "How often the object's position is tracked."), GUILayout.Width(150));
		hmT.breadCrumbInterval = EditorGUILayout.FloatField(hmT.breadCrumbInterval);
		hmT.breadCrumbInterval = Mathf.Max(1.0f, hmT.breadCrumbInterval);
		GUILayout.EndHorizontal();
		
		// Creates a group that lets the user select the save interval.
		GUILayout.BeginHorizontal();
		GUILayout.Label(new GUIContent("Save interval", "How often the tracked data is saved to a file."), GUILayout.Width(150));
		hmT.saveInterval = EditorGUILayout.FloatField(hmT.saveInterval);
		hmT.saveInterval = Mathf.Max(1.0f, hmT.saveInterval);
		GUILayout.EndHorizontal();

		// Creates a toggle box for each of the default events.
		DefaultTrackedEvents_FoldOut = EditorGUILayout.Foldout(DefaultTrackedEvents_FoldOut, defaultTrackedEvents);
		if(DefaultTrackedEvents_FoldOut)
		{
			List<string> eventKeys = new List<string>(_eventTooltips.Keys);

			foreach (var key in eventKeys)
			{
				bool eventSelected = hmT.TrackedEvents.Contains(key);
				
				GUILayout.BeginHorizontal();
				GUILayout.Label("", GUILayout.Width(10));
				GUILayout.Label(new GUIContent(key, _eventTooltips[key]), GUILayout.Width(135));
				eventSelected = EditorGUILayout.Toggle("", eventSelected, GUILayout.Width(27));				
				GUILayout.EndHorizontal();
				
				if (eventSelected && !hmT.TrackedEvents.Contains(key))
					hmT.TrackedEvents.Add(key);
				
				if (!eventSelected && hmT.TrackedEvents.Contains(key))
					hmT.TrackedEvents.Remove(key);
			}
		}
	}
}
