using UnityEngine;
using System.Collections;
using System.Xml;

#if UNITY_EDITOR
using UnityEditor;
using System.IO;
#endif

public class HM_Event
{
	private string eventName;
	private Vector3 position;

	/// <summary>
	/// Initializes a new instance of the <see cref="HM_Event"/> class.
	/// </summary>
	/// <param name="eventName"> The name of the event. </param>
	/// <param name="position"> The position where the event happened. </param>
	public HM_Event(string eventName, Vector3 position)
	{
		this.eventName = eventName.Replace(" ", string.Empty);
		this.position = position;
	}

	/// <summary>
	/// Writes the event to the given XMLTextWriter.
	/// </summary>
	/// <param name="writer"> The writer to write to. </param>
	public void WriteToFile(XmlTextWriter writer)
	{
		writer.WriteStartElement(eventName);
		writer.WriteAttributeString("x", position.x.ToString());
		writer.WriteAttributeString("y", position.y.ToString());
		writer.WriteAttributeString("z", position.z.ToString());
		writer.WriteEndElement();
	}
}
