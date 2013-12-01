using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class PositionTracker : MonoBehaviour {

	public float interval = 1;

	private float timer = 0;

	private List<Vector3> positions;

	void Start()
	{
		positions = new List<Vector3>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		timer += Time.deltaTime;

		if(timer >= interval)
		{
			timer = 0;
			
			positions.Add(transform.position);
		}
	}

	void OnApplicationQuit()
	{
		var directory = Directory.GetCurrentDirectory() + "\\HeatMapData";
		if(!Directory.Exists(directory))
		{
			Directory.CreateDirectory(directory);
		}

		var file = new StreamWriter(directory + "\\HeatMapData.txt", true);

		var sb = new StringBuilder();

		foreach(var v in positions)
		{
			sb.Append(v.x + "," + v.y + "," + v.z + "|");
		}

		file.WriteLine(sb);
		file.Close();
	}

	void StorePosition()
	{
		timer = 0;
		
		positions.Add(transform.position);
	}
}
