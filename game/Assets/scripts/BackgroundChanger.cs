using UnityEngine;
using System.Collections;

public class BackgroundChanger : MonoBehaviour
{
	public Material VRMissionBGMaterial;
	public Material NormalBGMaterial;

	// Use this for initialization
	void Start ()
	{
		//if (GlobalInfo.VRMission)
		//{
			//(GameObject.FindObjectOfType(typeof(Camera)) ).backgroundColor = Color.yellow;
		//}

		if (GlobalInfo.VRMission)
		{
			this.GetComponent<MeshRenderer>().material = VRMissionBGMaterial;
		}
		else
		{
			this.GetComponent<MeshRenderer>().material = NormalBGMaterial;
		}
	}
	
//	// Update is called once per frame
//	void Update ()
//	{
//	
//	}
}
