using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RevealTextAfterTime : MonoBehaviour
{
	public float Delay = 0.0f;

	private float StartTime = 0.0f;

	// Use this for initialization
	void Start ()
	{
		this.StartTime = Time.time;
		Color color = this.gameObject.GetComponent<RawImage>().color;
		color.a = 0.0f;
		this.gameObject.GetComponent<RawImage>().color = color;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if ((Time.time - this.StartTime) > this.Delay)
		{
			Color color = this.gameObject.GetComponent<RawImage>().color;
			color.a = 1.0f;
			this.gameObject.GetComponent<RawImage>().color = color;
			//this.enabled = false;
		}
	}
}
