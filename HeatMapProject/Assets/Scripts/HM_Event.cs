using UnityEngine;
using System.Collections;
using System.Xml;

#if UNITY_EDITOR
using UnityEditor;
using System.IO;
#endif

public class HM_Event
{
	private string eventType;
	private Vector3 position;

	public HM_Event(string eventType, Vector3 position)
	{
		this.eventType = eventType;
		this.position = position;
	}

	public void WriteToFile(XmlTextWriter writer)
	{
		writer.WriteStartElement(eventType);
		writer.WriteAttributeString("x", position.x.ToString());
		writer.WriteAttributeString("y", position.y.ToString());
		writer.WriteAttributeString("z", position.z.ToString());
		writer.WriteEndElement();
	}
}
