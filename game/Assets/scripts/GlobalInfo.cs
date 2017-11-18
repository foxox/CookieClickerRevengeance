using UnityEngine;
using System.Collections;

public class GlobalInfo : MonoBehaviour
{
	public static bool VRMission = false;


	void Awake ()
	{
		DontDestroyOnLoad(this);
	}

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}
