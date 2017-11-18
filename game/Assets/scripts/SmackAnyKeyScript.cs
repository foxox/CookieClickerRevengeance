using UnityEngine;
using System.Collections;

public class SmackAnyKeyScript : MonoBehaviour
{
	public GameObject cont;
	public GameObject vr;

	// Use this for initialization
	void Start ()
	{

	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetMouseButtonDown (0))
		{
			cont.SetActive (true);
			//Debug.Log(cont.activeInHierarchy + " and " + cont.activeSelf);
			vr.SetActive (true);
			this.gameObject.SetActive(false);
			//Debug.Log("disabling touch anywhere text, enabled continue and vrmissions");
		}
	}

}
