using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ClearTextOnStart : MonoBehaviour
{
	// Use this for initialization
	void Start ()
	{
		this.GetComponent<Text>().text = "";
		this.enabled = false;
	}
}
