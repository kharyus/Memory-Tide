using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {

	public static MusicManager myMusic;

	// Use this for initialization
	void Start () 
	{
		if (myMusic != null)
		{
			Destroy(gameObject);
		}
		else
		{
			myMusic = this;
		}	

		DontDestroyOnLoad(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
