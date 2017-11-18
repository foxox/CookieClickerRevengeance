using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CounterMirrorBladeModeVisibility : MonoBehaviour
{

	public GameObject AnyBladeModeElement;
	private RawImage BladeModeElementRawImage;

	// Use this for initialization
	void Start ()
	{
		if (this.AnyBladeModeElement)
		{
			this.BladeModeElementRawImage = this.AnyBladeModeElement.GetComponent<RawImage>();
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!BladeModeElementRawImage) return;
		Color thecolor = this.GetComponent<RawImage>().color;
		thecolor.a = BladeModeElementRawImage.IsActive()?0:1;
		this.GetComponent<RawImage>().color = thecolor;
	}
}
