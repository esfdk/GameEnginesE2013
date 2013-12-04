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
	private bool cubeFinished = false;

	// Use this for initialization
	void Start () 
	{

	}

	void Update()
	{
	}

	public void SetColor()
	{
		if(!cubeFinished)
		{
			var tempMat = new Material(Shader.Find ("Self-Illumin/Diffuse"));
			//		tempMat.texture = texture;
			tempMat.color = Color.blue;

			var heatCubes = GameObject.FindGameObjectsWithTag("HeatMarker");
			foreach(var hc in heatCubes)
			{
				var distance = Vector3.Distance(hc.transform.position, this.transform.position);
				if(distance <= allowedDistance)
				{
					var color = tempMat.color;
					color.r += heatAdjust;
					color.b -= heatAdjust;
					tempMat.color = color;
				}
			}
			this.renderer.material = tempMat;
			cubeFinished = true;
		}
	}
}
