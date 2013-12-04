using UnityEngine;
using System.Collections;

public class HM_ControlObjectScript : MonoBehaviour {

	private string sessionName;

	public string SessionName
	{
		get { return sessionName; }
	}

	public void CreateSession()
	{
		sessionName = "Session " + System.DateTime.Now.ToString(@"yyyy.MM.dd HH.mm.ss");
	}
	
	void OnApplicationQuit()
	{
		Destroy (this);
	}
}
