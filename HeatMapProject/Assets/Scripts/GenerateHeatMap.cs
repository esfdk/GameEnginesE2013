using UnityEngine;
using System.IO;
using System.Xml;
using System.Globalization;

public class GenerateHeatMap : MonoBehaviour
{
	public string HeatMapData;
	public GameObject HeatMarker;

	public void Generate()
	{
		if(HeatMapData != null && HeatMapData.Equals("HeatMapData"))
		{
			var directory = Directory.GetCurrentDirectory() + "\\HeatMapData";
			if(!Directory.Exists(directory))
			{
				Directory.CreateDirectory(directory);
			}
			var filename = directory + "\\" + HeatMapData;

			if(!filename.Substring(filename.Length - 4, 4).Equals(".xml"))
			{
				filename += ".xml";
			}

			XmlReader reader = XmlReader.Create(filename);
			XmlDocument xd = new XmlDocument();
			xd.Load(reader);

			foreach(XmlNode bc in xd.DocumentElement.GetElementsByTagName("BreadCrumb"))
			{
				var x = float.Parse(bc.ChildNodes[0].InnerText);
				var y = float.Parse(bc.ChildNodes[1].InnerText);
				var z = float.Parse(bc.ChildNodes[2].InnerText);
				Instantiate(HeatMarker, new Vector3(x, y, z), new Quaternion(0,0,0,0));
			}
		}
	}
}