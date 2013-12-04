using UnityEngine;
using System.IO;
using System.Xml;
using System.Globalization;

public class GenerateHeatMap : MonoBehaviour
{
	private string heatMapData;
	private string sessionFolder;
	public GameObject HeatMarker;

	public string HeatMapData
	{
		get { return heatMapData; }
		set { heatMapData = value; }
	}
	
	public string SessionFolder
	{
		get { return sessionFolder; }
		set { sessionFolder = value; }
	}

	public void Generate()
	{

		if(HeatMapData != null)
		{
			var directory = Directory.GetCurrentDirectory() + "\\HeatMapData";
			if(!Directory.Exists(directory))
			{
				Directory.CreateDirectory(directory);
			}
			var filename = directory;

			if (SessionFolder != null)
			{
				filename = filename + "\\" + SessionFolder;
			}

			filename = filename + "\\" + HeatMapData;

			if(!filename.Substring(filename.Length - 4, 4).Equals(".xml"))
			{
				filename += ".xml";
			}

			XmlReader reader = XmlReader.Create(filename);
			XmlDocument xd = new XmlDocument();
			xd.Load(reader);

			foreach(XmlNode bc in xd.DocumentElement.GetElementsByTagName("BreadCrumb"))
			{
				var x = float.Parse(bc.Attributes["x"].Value);
				var y = float.Parse(bc.Attributes["y"].Value);
				var z = float.Parse(bc.Attributes["z"].Value);
				var hm = (GameObject) Instantiate(HeatMarker, new Vector3(x, y, z), new Quaternion(0,0,0,0));
				hm.transform.parent = this.transform;
			}

			foreach(var hm in GameObject.FindGameObjectsWithTag("HeatMarker"))
			{
				HeatMapCubeScript hmcs = hm.GetComponent<HeatMapCubeScript>();
				hmcs.SetColor();
			}
		}
	}

	public void Clear()
	{
		foreach(var hm in GameObject.FindGameObjectsWithTag("HeatMarker"))
		{
			DestroyImmediate(hm);
		}
	}
}