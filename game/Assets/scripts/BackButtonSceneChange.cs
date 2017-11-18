using UnityEngine;
using System.Collections;

public class BackButtonSceneChange : MonoBehaviour
{

	public string GoToThisScene;

//	// Use this for initialization
//	void Start ()
//	{
//	
//	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.LoadLevel(GoToThisScene);
		}
	}
}
