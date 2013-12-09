using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

public class HM_Tracker : MonoBehaviour 
{
	public List<string> TrackedEvents = new List<string>();

	public float breadCrumbInterval = 1;
	public float saveInterval = 10;

	private float saveTimer = 0;
	private float _lastBreadCrumb = 0;

	private string fileName;

	private List<HM_Event> eventsLogged;

	private FileStream fileStream;
	private StreamWriter sw;
	private XmlTextWriter writer;

	private GameObject controlObject = null;

	/// <summary>
	/// Starts this object.
	/// </summary>
	void Start () 
	{
		eventsLogged = new List<HM_Event>();
		CreateXMLTextWriter();
	}
	
	/// <summary>
	/// Update this object.
	/// </summary>
	void Update () 
	{
		saveTimer += Time.deltaTime;

		// Tracks the object's position every interval.
		if (Time.time > _lastBreadCrumb + breadCrumbInterval)
		{
			_lastBreadCrumb = Time.time;
			eventsLogged.Add(new HM_Event(HM_EventTypes.BreadCrumb, transform.position));
		}
		
		// Tracks the object if OnMouseUp is to be tracked.
		if (TrackedEvents.Contains(HM_EventTypes.OnMouseUp))
		{
			if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1) || Input.GetMouseButtonUp(2))
			{
				eventsLogged.Add(new HM_Event(HM_EventTypes.OnMouseUp, transform.position));
			}
		}

		// Saves the events to a file.
		if(saveTimer >= saveInterval)
		{
			saveTimer = 0;
			SaveToFile();
			eventsLogged.Clear();
		}
	}
	
	void Awake()
	{
		// Tracks the object if Awake is to be tracked.
		if (TrackedEvents.Contains(HM_EventTypes.Awake))
		{
			eventsLogged.Add(new HM_Event(HM_EventTypes.Awake, transform.position));
		}
	}

	void onDestroy()
	{
		// Tracks the object if onDestroy is to be tracked.
		if (TrackedEvents.Contains(HM_EventTypes.Destroy))
		{
			eventsLogged.Add(new HM_Event(HM_EventTypes.Destroy, transform.position));
		}
	}

	void OnTriggerEnter()
	{
		// Tracks the object if OnTiggerEnter is to be tracked.
		if (TrackedEvents.Contains(HM_EventTypes.OnTriggerEnter))
		{
			eventsLogged.Add(new HM_Event(HM_EventTypes.OnTriggerEnter, transform.position));
		}
	}

	/// <summary>
	/// Saves the remaining tracked data and closes the writers.
	/// </summary>
	void OnApplicationQuit()
	{
		this.SaveToFile();

		writer.WriteEndElement();
		writer.WriteEndElement();
		writer.WriteEndDocument();
		writer.Close();
		sw.Close();
		fileStream.Close();
		
		var controlObjectScript = controlObject.GetComponent<HM_ControlObjectScript>();
		controlObjectScript.DeactivateWriter();
		controlObjectScript.CombineFiles();
	}

	/// <summary>
	/// Saves the tracked data to a file.
	/// </summary>
	void SaveToFile()
	{
		foreach(var trackedEvent in eventsLogged)
		{
			trackedEvent.WriteToFile(writer);
		}
	}

	/// <summary>
	/// Creates the XML text writer.
	/// </summary>
	void CreateXMLTextWriter()
	{
		// Checks if the control object exists. If not, creates it.
		controlObject = GameObject.Find("HM_ControlObject");
		if (controlObject == null)
		{
			var newControlObject = new GameObject();
			newControlObject.name = "HM_ControlObject";
			newControlObject.AddComponent<HM_ControlObjectScript>();
			newControlObject.GetComponent<HM_ControlObjectScript>().CreateSession();
			controlObject = GameObject.Find("HM_ControlObject");
		}

		// Tells the control object that a writer has been activated.
		var controlObjectScript = controlObject.GetComponent<HM_ControlObjectScript>();
		controlObjectScript.ActivateWriter();

		// Creates writers, streams, etc.
		fileName = controlObjectScript.DataDirectory + "\\" + controlObjectScript.SessionName + "\\HeatMapData" + this.transform.name.Replace(" ", string.Empty) + ".xml";
		fileStream = new FileStream(fileName, FileMode.Create);
		sw = new StreamWriter(fileStream);
		writer = new XmlTextWriter(sw);
		writer.Formatting = Formatting.Indented;
		writer.Indentation = 4;

		// Begins the xml document.
		writer.WriteStartDocument();
		writer.WriteStartElement("TrackingData");
		
		writer.WriteStartElement(this.gameObject.name.Replace(" ", string.Empty));
	}

	/// <summary>
	/// Adds the given event to the tracker.
	/// </summary>
	/// <param name="gameEvent"> The event to track. </param>
	public void AddEvent(HM_Event gameEvent)
	{
		this.eventsLogged.Add(gameEvent);
	}
}
