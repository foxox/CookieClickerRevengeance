using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ActivateObjectAfterTime : MonoBehaviour
{
	public GameObject ActivateMe;

	public float Delay = 0.0f;
	
	private float StartTime = 0.0f;
	
	// Use this for initialization
	void Start ()
	{
		this.StartTime = Time.time;
		ActivateMe.SetActive(false);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if ((Time.time - this.StartTime) > this.Delay)
		{
			ActivateMe.SetActive(true);
		}
	}
}
