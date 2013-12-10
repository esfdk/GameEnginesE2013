using UnityEngine;
using System.Collections;

public class HM_HeatMarkerScript : MonoBehaviour {

	/// <summary>
	/// The amount of heat markers within the allowed distance.
	/// </summary>
	/// <value>The density.</value>
	public float Density { get; private set;}

	/// <summary>
	/// Calculates the density of this heat marker.
	/// </summary>
	/// <returns>The density.</returns>
	/// <param name="allowedDistance">The distance which to check for heat markers.</param>
	public float GetDensity(float allowedDistance)
	{
		Density = 0;
		foreach(var hm in GameObject.FindGameObjectsWithTag("HeatMarker"))
		{
			var distance = Vector3.Distance(hm.transform.position, this.transform.position);
			if(distance <= allowedDistance && !hm.Equals(this))	Density++;
		}

		return Density;
	}

	/// <summary>
	/// Sets the color of the heat marker.
	/// </summary>
	/// <param name="color">Color.</param>
	public void SetColor(Color color)
	{
		var tempMat = new Material(Shader.Find ("Transparent/Diffuse"));
		tempMat.color = color;
		renderer.material = tempMat;
	}
}
