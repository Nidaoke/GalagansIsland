using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using InControl;

public class IndieGameTransition : MonoBehaviour 
{
	GameObject mPlayerShip;
	GameObject mScoreManager;
	GameObject mGameHUD;
	GameObject mAsteroidspawn;
	GameObject mIcicleStorm;

	public Image mStartScreenImage;
	public List<Sprite> mGameImages = new List<Sprite>();
	[SerializeField] private GameObject mStaticEffect;
	float mStaticTimer = 0f;
	bool mPlayingStatic = false;
	[SerializeField] private GameObject mPressStartMessage;
	bool mWaitForInput = false;

	[SerializeField] private Text mMiniGameHighScoreText;

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

		mStartScreenImage.sprite = mGameImages[PlayerPrefs.GetInt ("IndieGameKey")-1];

		if(PlayerPrefs.GetInt("GoingToGame") == 0)
		{
			Debug.Log("Going to mini game");
			GetComponent<Animator>().Play("TransitionAnimation");
		}
		else
		{
			Debug.Log("Returning to main game");
			GetComponent<Animator>().Play("ReturnAnimation");
		}
	}//END of Start()
	
	// Update is called once per frame
	void Update () 
	{
		//For doing the screen static effect ~Adam
		if(mStaticTimer > 0f)
		{
			mStaticEffect.SetActive (true);

			if(!mPlayingStatic)
			{	if(PlayerPrefs.GetInt("GoingToGame") == 0)
				{
					mStaticEffect.GetComponent<Animator>().Play ("StaticOn");
				}
				else
				{
					mStaticEffect.GetComponent<Animator>().Play ("StaticOff");
				}
			}
			mPlayingStatic =true;

			mStaticTimer -= Time.deltaTime;
		}
		else
		{
			mStaticEffect.SetActive (false);
			mPlayingStatic = false;
		}

		//Show the high score of the game you're about to play ~Adam
		switch (PlayerPrefs.GetInt ("IndieGameKey"))
		{
		case 1:
			mMiniGameHighScoreText.text = "High Score: " + PlayerPrefs.GetInt ("ToEHighScore");
			break;
		default:
			mMiniGameHighScoreText.text = "High Score: 0";
			break;
		}

		//For waiting for the player to press a button to start the minigame ~Adam
		if (mWaitForInput)
		{
			mPressStartMessage.SetActive(true);
			if(Input.anyKeyDown || InputManager.ActiveDevice.AnyButton.WasPressed)
			{
				GetComponent<Animator>().speed = 1f;
				mWaitForInput = false;
				mPressStartMessage.SetActive(false);

			}
		}

	}//END of Update()

	public void GoToIndieGame()
	{
		//Make sure these objects persist ~Adam
		mPlayerShip.SetActive (true);
		mScoreManager.SetActive (true);
		mGameHUD.SetActive (true);
		mAsteroidspawn.SetActive (true);
		mIcicleStorm.SetActive (true);

		switch (PlayerPrefs.GetInt ("IndieGameKey"))
		{
		case 1:
			Application.LoadLevel ("TowerOfElementsScene");
			break;
		default:
			Application.LoadLevel ("TowerOfElementsScene");
			break;
		}
	}//END of GoToIndieGame()

	public void ReturnToMainGame()
	{
		//Make sure these objects persist ~Adam
		mPlayerShip.SetActive (true);
		mScoreManager.SetActive (true);
		mGameHUD.SetActive (true);
		mAsteroidspawn.SetActive (true);
		mIcicleStorm.SetActive (true);


		Application.LoadLevel(PlayerPrefs.GetInt ("MainLevelLeft"));
	}//END of ReturnToMainGame()

	public void DoScreenStatic()
	{
		//Camera.main.GetComponent<CameraShader>().mShaderTimer = 2f;
		mStaticTimer = 3f;

	}

	public void AnimationWait()
	{
		GetComponent<Animator>().speed = 0f;
		mWaitForInput = true;
	}
}
