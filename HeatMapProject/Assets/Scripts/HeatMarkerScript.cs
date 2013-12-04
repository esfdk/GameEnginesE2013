using UnityEngine;
using System.Collections;

public class HeatMarkerScript : MonoBehaviour {

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
		foreach(var hm in GameObject.FindGameObjectsWithTag("HeatMarker"))
		{
			var distance = Vector3.Distance(hm.transform.position, this.transform.position);
//			var offset = hm.transform.position - this.transform.position;
//			var distance = offset.sqrMagnitude;
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

	#region SetColor
	/*
	public void SetColor()
	{
		if(!cubeFinished)
		{
			var tempMat = new Material(Shader.Find ("Transparent/Diffuse"));
			tempMat.color = ColorCold;
			tempMat.color = new Color(ColorCold.r, ColorCold.g, ColorCold.b, 0.5f);
			hs = HeatState.Cold;

//			var heatAdjust = (1 / GameObject.FindGameObjectsWithTag("HeatMarker").Length) * 5 * 100;
			foreach(var hm in GameObject.FindGameObjectsWithTag("HeatMarker"))
			{
//				var distance = Vector3.Distance(hm.transform.position, this.transform.position);
				var offset = hm.transform.position - this.transform.position;
				var distance = offset.sqrMagnitude;
				if(distance <= allowedDistance)
				{
					switch(hs)
					{
						case HeatState.Cold:
							tempMat.color = Colorx.Slerp(tempMat.color, ColorChilly, heatAdjust);
							if(tempMat.color.ToString() == ColorChilly.ToString())
							{
								hs = HeatState.Chilly;
							}
							break;
						case HeatState.Chilly:
						  	tempMat.color = Colorx.Slerp(tempMat.color, ColorAverage, heatAdjust);
							if(tempMat.color.ToString() == ColorAverage.ToString())
							{
								hs = HeatState.Average;
							}
							break;
						case HeatState.Average:
							tempMat.color = Colorx.Slerp(tempMat.color, ColorWarm, heatAdjust);
							if(tempMat.color.ToString() == ColorWarm.ToString())
							{
								hs = HeatState.Warm;
							}
							break;
						case HeatState.Warm:
							tempMat.color = Colorx.Slerp(tempMat.color, ColorHot, heatAdjust);
							if(tempMat.color.ToString() == ColorHot.ToString())
							{
								hs = HeatState.Hot;
							}
							break;
						case HeatState.Hot:
							// Do nothing
							break;
						default:
							break;
					}

				}
			}
			this.renderer.material = tempMat;
			cubeFinished = true;
		}
	}
	*/
	#endregion
}
