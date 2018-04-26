using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

	public static AudioManager instance;
	void Awake()
	{
		if (instance == null) {
			instance = this;
		} else if (instance != null) {
			Destroy (this);
		}
	}

	//Audio Tracks
	public AudioSource backgroundMusic;
	public AudioClip[]	musicTacks;
	private int trackNumber = 0;
	//DestructionSFX
	public AudioSource destructionSource;
	public AudioClip[] buildingsSFX;
	private int choice;
	//WeaponSFX
	public AudioSource sFXSource;
	public AudioClip autocannonFX;
	public AudioClip chaingunFX;
	public AudioClip scattercannonFX;
	public AudioClip snubCannonFX;
	// Use this for initialization
	void Start () {
		
		
	}
	
	// Update is called once per frame
	void Update () {

		if (backgroundMusic.isPlaying == false) {
			SwapBackgroundTrack (trackNumber);
		}


//		if (Input.GetKeyDown (KeyCode.Z)) {
//			PlayAutocannon ();
//		}
//		if (Input.GetKeyDown (KeyCode.X)) {
//			PlayChaingun ();
//		}if (Input.GetKeyDown (KeyCode.C)) {
//			PlayScattercannon ();
//		}if (Input.GetKeyDown(KeyCode.V)) {
//			PlaySnubcannon ();
//		}
	}

	void SwapBackgroundTrack(int track)
	{
		backgroundMusic.clip = musicTacks [trackNumber];
		trackNumber++;
		if (trackNumber < musicTacks.Length)
			trackNumber = 0;
	}
	public void PlayBuildingDestruction()
	{
		choice = Random.Range(0,buildingsSFX.Length)-1;
		destructionSource.clip = buildingsSFX[choice];
	}

	public void PlayAutocannon()
	{
		sFXSource.clip = autocannonFX;
		sFXSource.Play ();
	}public void PlayChaingun()
	{
		sFXSource.clip = chaingunFX;
		sFXSource.Play ();
	}public void PlayScattercannon()
	{
		sFXSource.clip = scattercannonFX;
		sFXSource.Play ();
	}public void PlaySnubcannon()
	{
		sFXSource.clip = snubCannonFX;
		sFXSource.Play ();
	}


}
