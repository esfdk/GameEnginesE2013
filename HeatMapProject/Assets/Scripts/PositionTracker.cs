using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

public class PositionTracker : MonoBehaviour {

	public float interval = 1;
	public float saveInterval = 20;

	private float timer = 0;
	private float saveTimer = 0;

	private List<HM_Event> trackedEvents;

	private FileStream fileStream;
	private StreamWriter sw;
	private XmlTextWriter writer;

	void Start()
	{
		trackedEvents = new List<HM_Event>();

		var directory = Directory.GetCurrentDirectory() + "\\HeatMapData";
		if(!Directory.Exists(directory))
		{
			Directory.CreateDirectory(directory);
		}
		var filename = directory + "\\HeatMapData.xml";

		fileStream = new FileStream(filename, FileMode.Create);
		sw = new StreamWriter(fileStream);
		writer = new XmlTextWriter(sw);
		writer.Formatting = Formatting.Indented;
		writer.Indentation = 4;

		writer.WriteStartDocument();
		writer.WriteStartElement("TrackingData");
		
		writer.WriteStartElement(this.gameObject.name.Replace(" ", string.Empty));

	}
	
	// Update is called once per frame
	void Update () 
	{
		timer += Time.deltaTime;
		saveTimer += Time.deltaTime;

		if(timer >= interval)
		{
			timer = 0;

			trackedEvents.Add(new HM_Event(HM_EventTypes.BreadCrumb, transform.position));
		}

		if(saveTimer >= saveInterval)
		{
			saveTimer = 0;
			SaveToFile();
			trackedEvents.Clear();
		}
	}

	void OnApplicationQuit()
	{
		writer.WriteEndElement();
		writer.WriteEndElement();
		writer.WriteEndDocument();

		writer.Close();
		sw.Close();
		fileStream.Close();
	}

	void SaveToFile()
	{
		foreach(var trackedEvent in trackedEvents)
		{
			trackedEvent.SaveToFile(writer);
		}
	}
}
