using UnityEngine;
using System.Collections;

public class DeactivateOnStart : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
		this.gameObject.SetActive (false);
	}

	//void Awake ()
	//{
	//	this.gameObject.SetActive (false);
	//}
}
