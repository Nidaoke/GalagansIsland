using UnityEngine;
using System.Collections;

public class IndieGameReturn : MonoBehaviour 
{
	[SerializeField] private float mReturnTimer = 5f;
	[SerializeField] private GameObject mGameOverScreen;

	[SerializeField] private GameObject mPlayerShip;
	[SerializeField] private GameObject mScoreManager;
	[SerializeField] private GameObject mGameHUD;
	[SerializeField] private GameObject mAsteroidspawn;
	[SerializeField] private GameObject mIcicleStorm;




	// Use this for initialization
	void Start () 
	{
		//Find and hide yet preserve the objects we need for going back to the main game ~Adam
		mPlayerShip = GameObject.Find("Ship");
		mScoreManager = GameObject.Find("ScoreManager");
		mGameHUD = GameObject.Find("Game_HUD");
		mAsteroidspawn = GameObject.Find("Asteroidspawn");
		mIcicleStorm = GameObject.Find("Icicle storm");

		mPlayerShip.SetActive (false);
		mScoreManager.SetActive (false);
		mGameHUD.SetActive (false);
		mAsteroidspawn.SetActive (false);
		mIcicleStorm.SetActive (false);

		foreach(AsteroidScript asteroid in FindObjectsOfType<AsteroidScript>())
		{
			Destroy (asteroid.gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Count down to leave when it's game over ~Adam
		if(mGameOverScreen.activeInHierarchy)
		{
			mReturnTimer -= Time.deltaTime;

			if(mReturnTimer <= 0f)
			{
				//Make sure these objects persist ~Adam
				mPlayerShip.SetActive (true);
				mScoreManager.SetActive (true);
				mGameHUD.SetActive (true);
				mAsteroidspawn.SetActive (true);
				mIcicleStorm.SetActive (true);

				PlayerPrefs.SetInt("GoingToGame", 1);

				Application.LoadLevel("IndieGameTransition");
			}
		}
	}
}
