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

	private List<HM_Event> trackedEvents;

	private FileStream fileStream;
	private StreamWriter sw;
	private XmlTextWriter writer;

	private GameObject controlObject = null;

	/// <summary>
	/// Starts this object.
	/// </summary>
	void Start () 
	{
		trackedEvents = new List<HM_Event>();
		CreateXMLTextWriter();
	}
	
	/// <summary>
	/// Update this object.
	/// </summary>
	void Update () 
	{
		saveTimer += Time.deltaTime;
		
		if (Time.time > _lastBreadCrumb + breadCrumbInterval)
		{
			_lastBreadCrumb = Time.time;
			trackedEvents.Add(new HM_Event(HM_EventTypes.BreadCrumb, transform.position));
		}
		
		if(saveTimer >= saveInterval)
		{
			saveTimer = 0;
			SaveToFile();
			trackedEvents.Clear();
		}
	}
	
	void Awake()
	{
		if (TrackedEvents.Contains(HM_EventTypes.Awake))
		{
			trackedEvents.Add(new HM_Event(HM_EventTypes.Awake, transform.position));
		}
	}

	void onDestroy()
	{
		if (TrackedEvents.Contains(HM_EventTypes.Destroy))
		{
			trackedEvents.Add(new HM_Event(HM_EventTypes.Destroy, transform.position));
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
		foreach(var trackedEvent in trackedEvents)
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
}
