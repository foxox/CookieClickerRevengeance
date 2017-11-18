using UnityEngine;
using System.Collections;

public class PlayOtherAudioOnThisOneDone : MonoBehaviour
{

	public AudioSource thisAudioSource;
	public AudioSource otherAudioSource;

	// Use this for initialization
	void Start ()
	{
		//schedule end time
		double t0 = AudioSettings.dspTime + 0.5f;
		double clipTime1 = thisAudioSource.clip.samples / thisAudioSource.clip.frequency;
		thisAudioSource.SetScheduledEndTime(clipTime1);
		otherAudioSource.SetScheduledStartTime(clipTime1);
		thisAudioSource.PlayScheduled(t0);
		otherAudioSource.PlayScheduled(t0+clipTime1);
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}


}
