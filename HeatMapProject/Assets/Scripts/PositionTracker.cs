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

	private List<Vector3> positions;

	private FileStream fileStream;
	private StreamWriter sw;
	private XmlTextWriter writer;

	void Start()
	{
		positions = new List<Vector3>();


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
			
			positions.Add(transform.position);
		}

		if(saveTimer >= saveInterval)
		{
			saveTimer = 0;
			SaveToFile();
			positions.Clear();
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
		foreach(var p in positions)
		{
			writer.WriteStartElement("BreadCrumb");
			writer.WriteElementString("x", p.x.ToString());
			writer.WriteElementString("y", p.y.ToString());
			writer.WriteElementString("z", p.z.ToString());
			writer.WriteEndElement();
		}
	}

	void StorePosition()
	{
		timer = 0;
		
		positions.Add(transform.position);
	}
}
