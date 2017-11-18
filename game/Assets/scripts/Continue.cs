using UnityEngine;
using System.Collections;

public class Continue : MonoBehaviour
{
	
	public AudioSource audio1;
	public AudioSource audio2;
	public float FadeDelta = 0.1f;
	public GameObject canvas;

	private bool fadeVolumeAndTransition;
	private float volume1;
	private float volume2;
	
	// Use this for initialization
	void Start ()
	{
		fadeVolumeAndTransition = false;
	}

	public void startTransition(string bgType)
	{
		if (!fadeVolumeAndTransition)// && Input.GetMouseButtonDown(0))
		{
			//Debug.Log("start transition");
			fadeVolumeAndTransition = true;
			volume1 = audio1.volume;
			volume2 = audio2.volume;
			canvas.transform.Translate (100,0,0);

			//GameObject.Find("GLOBALS").GetComponent<GlobalInfo>().VRMission
			GlobalInfo.VRMission = (bgType=="vrmission");
		}
	}
	
	// Update is called once per frame
	void Update ()
	{		
		if (fadeVolumeAndTransition)
		{
			audio1.volume -= FadeDelta * Time.deltaTime;
			audio2.volume -= FadeDelta * Time.deltaTime;
			if (audio1.volume < 0.01f)
			{
				audio1.volume = 0.0f;
			}
			if (audio2.volume < 0.01f)
			{
				audio2.volume = 0.0f;
			}
		}
		
		if (audio1.volume == 0.0f && audio2.volume == 0.0f)
		{
			//Application.LoadLevel("gamescene");
            UnityEngine.SceneManagement.SceneManager.LoadScene("gamescene");
			audio1.Stop();
			audio2.Stop();
			audio1.volume = volume1;
			audio2.volume = volume2;
		}
	}
	
	
	
}
