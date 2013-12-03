using UnityEngine;
using System.Collections;

//[ExecuteInEditMode]
public class HeatMapCubeScript : MonoBehaviour {

	// Distance between 
	public float allowedDistance;

	// Amount to change heat color
	public float heatAdjust;

	// Texture to use
	public Texture texture;

	// If the cube is correctly colored
	private bool cubeFinished;

	// Use this for initialization
	void Start () 
	{
		renderer.material = new Material(Shader.Find ("Self-Illumin/Diffuse"));
		renderer.material.mainTexture = texture;
		renderer.material.color = Color.blue;
	}

	void Update()
	{
		var color = renderer.material.color;
		color.r += heatAdjust;
		color.b -= heatAdjust;
		renderer.material.color = color;
	}

	void setColor()
	{
		if(!cubeFinished)
		{
			var heatCubes = GameObject.FindGameObjectsWithTag("HeatCube");
			foreach(var hc in heatCubes)
			{
				Vector2 distance = hc.transform.position - this.transform.position;
				var distSqrMag = distance.sqrMagnitude;
				if(distSqrMag <= allowedDistance)
				{
					var color = renderer.material.color;
					color.r += heatAdjust;
					color.b -= heatAdjust;
					renderer.material.color = color;
				}
			}
			cubeFinished = true;
		}
	}
}
