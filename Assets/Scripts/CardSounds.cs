using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSounds : MonoBehaviour {

	private AudioSource myAudio;

	public AudioClip cardFlipSound;
	public AudioClip jingleSound;

	float startPitch;

	// Use this for initialization
	void Start () 
	{
		myAudio = GetComponent<AudioSource>();
		startPitch = myAudio.pitch;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void PlayCardFlipSound()
	{
		myAudio.pitch = startPitch;
		myAudio.PlayOneShot(cardFlipSound);
	}

	public void PlayJingleSound()
	{
		myAudio.pitch = startPitch;
		myAudio.PlayOneShot(jingleSound);
	}


	public void PlayCardFlop()
	{
		myAudio.pitch = .6f;
		myAudio.PlayOneShot(cardFlipSound);

	}
}
