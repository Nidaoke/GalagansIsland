using UnityEngine;
using System.Collections;

public class BGMVolumeController : MonoBehaviour 
{
	public bool waitForPlayingMusic;
	public float waitTime;

//	bool tempAudioFade;

	public GameObject secondAudioSource;

	[HideInInspector] public  float mStartingVolume;
	float mFadeInVolume = 0f;
	[SerializeField] private  bool mDoFadeIn = true;
	// Use this for initialization
	void Start () 
	{

		//GetComponent<AudioSource>().enabled = true;
		
		mStartingVolume = GetComponent<AudioSource>().volume;
		//		PlayerPrefs.SetFloat("SFXVolume", 0.8f);
		//		PlayerPrefs.SetFloat("BGMVolume", 0.8f);
		GetComponent<AudioSource>().ignoreListenerVolume = true;
		if(mDoFadeIn)
		{
			GetComponent<AudioSource>().volume = mFadeInVolume * PlayerPrefs.GetFloat("BGMVolume");
		}
		else
		{
			GetComponent<AudioSource>().volume = mStartingVolume * PlayerPrefs.GetFloat("BGMVolume");
		}

		if (!waitForPlayingMusic) {

			GetComponent<AudioSource>().enabled = true;
		}

		if (waitForPlayingMusic && secondAudioSource != null) {

			secondAudioSource.SetActive (true);
		}
	}
	
	// Update is called once per frame
	void Update () 
	{



		if (waitForPlayingMusic) {

			waitTime -= Time.deltaTime;
		}

		if (waitTime <= 0f && waitForPlayingMusic) {

			GetComponent<AudioSource>().enabled = true;

			secondAudioSource.SetActive(false);
		}

		mFadeInVolume = Mathf.Lerp (mFadeInVolume, mStartingVolume, 0.01f);
		if(mFadeInVolume < mStartingVolume *0.95f && mDoFadeIn)
		{
			GetComponent<AudioSource>().volume = mFadeInVolume * PlayerPrefs.GetFloat("BGMVolume");
		}
		else
		{
			GetComponent<AudioSource>().volume = mStartingVolume * PlayerPrefs.GetFloat("BGMVolume");
		}
		AudioListener.volume = PlayerPrefs.GetFloat("SFXVolume");

		//Mostly temporary, some people were complaining that the SFX were too loud and they were too lazy to turn them down ~ Jonathan
		if (Application.loadedLevel == 0) { //If on main menu ~ Jonathan

			/*//Debug.Log(PlayerPrefs.GetFloat("SFXVolume"));
			if(PlayerPrefs.GetFloat("SFXVolume") > .8f)
				AudioListener.volume = PlayerPrefs.GetFloat("SFXVolume") - .9f;*/

			if (PlayerPrefs.GetFloat ("SFXVolume") > .05f) {

				AudioListener.volume = 0.05f;
			}
		} else {

			AudioListener.volume = PlayerPrefs.GetFloat("SFXVolume");
		}
	}
}
