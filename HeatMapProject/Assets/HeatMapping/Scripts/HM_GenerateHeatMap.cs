using UnityEngine;
using System.IO;
using System.Xml;
using System.Globalization;

public class HM_GenerateHeatMap : MonoBehaviour
{
	private string heatMapFile, heatMapObject, heatMapEvent;

	/// <summary>
	/// The object to instantiate as heat markers.
	/// </summary>
	public GameObject HeatMarker;

	/// <summary>
	/// The scale of the heat markers.
	/// </summary>
	public float HeatMarkerScale;
	
	/// <summary>
	/// The distance between nodes used for density calculation.
	/// </summary>
	public float AllowedDistance;

	// Highest density in the heatmap
	private float highestDensity;

	// Colours used for the different heat states
	private Color ColorCold = new Color(Color.blue.r, Color.blue.g, Color.blue.b, 0.25f);
	private Color ColorChilly = new Color32(160, 32, 240, 64);
	private Color ColorAverage = new Color(Color.yellow.r, Color.yellow.g, Color.yellow.b, 0.25f);
	private Color ColorWarm = new Color32(255,127,80, 64);
	private Color ColorHot = new Color(Color.red.r, Color.red.g, Color.red.b, 0.25f);

	/// <summary>
	/// Gets or sets the file that contains the data for the heatmap.
	/// </summary>
	/// <value>The heat map data file.</value>
	public string HeatMapFile
	{
		get { return heatMapFile; }
		set { heatMapFile = value; }
	}

	/// <summary>
	/// Gets or sets the chosen object for the heat map.
	/// </summary>
	/// <value>The heat map object.</value>
	public string HeatMapObject
	{
		get { return heatMapObject; }
		set { heatMapObject = value; }
	}

	/// <summary>
	/// Gets or sets the chosen event for the heat map.
	/// </summary>
	/// <value>The heat map event.</value>
	public string HeatMapEvent
	{
		get { return heatMapEvent; }
		set { heatMapEvent = value; }
	}

	/// <summary>
	/// Instantiates a heatmap based on an XML-file.
	/// </summary>
	public void Generate()
	{
		if(HeatMapFile != null)
		{
			if (HeatMapFile.Equals("None"))
			{
				Debug.Log("Select valid data before trying to generate the heatmap");
				return;
			}

			// This part seems strange is prolly not necessary anymore, but meh.
			var directory = Directory.GetCurrentDirectory() + "\\HeatMapData";
			if(!Directory.Exists(directory))
			{
				Directory.CreateDirectory(directory);
			}

			// Get the filename and ensure it ends in .xml
			var filename = directory + "\\" + HeatMapFile;
			if(!filename.Substring(filename.Length - 4, 4).Equals(".xml"))
			{
				filename += ".xml";
			}

			// Check if the object/event is "All" or not.
			var actualObject = (HeatMapObject.Equals("All")) ? "*" : HeatMapObject;
			var actualEvent = (HeatMapEvent.Equals("All")) ? "*" : HeatMapEvent;

			// Create an XML reader for the file.
			XmlReader reader = XmlReader.Create(filename);
			XmlDocument xd = new XmlDocument();
			xd.Load(reader);

			// Get the chosen events for the chosen object.
			XmlElement xelRoot = xd.DocumentElement;
			XmlNodeList xnlNodes = xelRoot.SelectNodes("/TrackingData/" + actualObject + "/" + actualEvent);

			// Iterate over all the event nodes and create markers for them.
			foreach(XmlNode node in xnlNodes)
			{
				var x = float.Parse(node.Attributes["x"].Value);
				var y = float.Parse(node.Attributes["y"].Value);
				var z = float.Parse(node.Attributes["z"].Value);
				var hm = (GameObject) Instantiate(HeatMarker, new Vector3(x, y, z), new Quaternion(0,0,0,0));
				hm.transform.localScale = new Vector3(HeatMarkerScale, HeatMarkerScale, HeatMarkerScale);
				hm.transform.parent = this.transform;
			}

			CalculateDensity();
			SetColors();
		}
	}

	/// <summary>
	/// Makes all heatmarkes calculate their density values.
	/// </summary>
	public void CalculateDensity()
	{
		highestDensity = 0;
		
		foreach(var hm in GameObject.FindGameObjectsWithTag("HeatMarker"))
		{
			var hms = hm.GetComponent<HM_HeatMarkerScript>();
			var d = hms.GetDensity(AllowedDistance);
			highestDensity = d > highestDensity ? d : highestDensity;
		}
	}

	/// <summary>
	/// Sets the color of the heat markers based on their density.
	/// </summary>
	public void SetColors()
	{

		foreach(var hm in GameObject.FindGameObjectsWithTag("HeatMarker"))
		{
			var hms = hm.GetComponent<HM_HeatMarkerScript>();
			var densityPercent = hms.Density / highestDensity;
			hms.SetColor(SlerpedColor(densityPercent));
		}
	}

	private Color SlerpedColor(float densityPercent)
	{
		Color newColor;
		if(densityPercent < 0.25f) { newColor = Colorx.Slerp(ColorCold, ColorChilly, Normalize(densityPercent, 0.0f, 0.25f, 0.0f, 1.0f));}
		else if(0.25f <= densityPercent & densityPercent < 0.5f){ newColor = Colorx.Slerp(ColorChilly, ColorAverage, Normalize(densityPercent, 0.25f, 0.5f, 0.0f, 1.0f));}
		else if(0.5f <= densityPercent & densityPercent < 0.75f){ newColor = Colorx.Slerp(ColorAverage, ColorWarm, Normalize(densityPercent, 0.5f, 0.75f, 0.0f, 1.0f));}
		else { newColor = Colorx.Slerp(ColorWarm, ColorHot, Normalize(densityPercent, 0.75f, 1.0f, 0.0f, 1.0f));}

		return newColor;
	}
	
	/// <summary>
	/// Normalize the specified value to a new range based on an old range.
	/// </summary>
	/// <param name="value">Value.</param>
	/// <param name="oldMin">Old minimum.</param>
	/// <param name="oldMax">Old max.</param>
	/// <param name="newMin">New minimum.</param>
	/// <param name="newMax">New max.</param>
	private float Normalize(float value, float oldMin, float oldMax, float newMin, float newMax)
	{
		return ((value - oldMin) / (oldMax - oldMin)) * (newMax - newMin) + newMin;
	}

	/// <summary>
	/// Destroys all heat markers.
	/// </summary>
	public void Clear()
	{
		foreach(var hm in GameObject.FindGameObjectsWithTag("HeatMarker"))
		{
			DestroyImmediate(hm);
		}
	}
}