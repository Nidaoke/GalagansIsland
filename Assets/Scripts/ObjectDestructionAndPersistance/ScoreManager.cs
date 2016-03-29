using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using InControl;
using Assets.Scripts.Achievements;
//using XInputDotNetPure; // Required in C#

//This script tracks and modifies player scores, player life counts, and several elements of the game's HUD.

//It makes use of the open source version of the InControl Unity plugin for taking game pade input.  This plugin may be found at: "https://github.com/pbhogan/InControl" ~Adam

public class ScoreManager : MonoBehaviour 
{
	[SerializeField] private Texture2D mSideDisplayTex;

	public int mShieldLifeCount = 10;
	public int mShieldHits = 0;

	public int mScore = 0;
	public int mLivesRemaining = 24;
    public int mMaxLives = 24;
	public int mCurrentLevel; 
	public int mOriginalLevel = 0;
	//For giving the player an extra life every certain number of points (no longer used) ~Adam
	int mExtraLifeScore = 1000;
	int mExtraLifeInteraval = 1000;

	//For spawning an triple-bullet power-up every certain number of points ~Adam
	public int mPowerUpScore = 300;
	int mPowerUpInterval = 300;
	[SerializeField] private GameObject mTripleBulletEmblem;
	//For spawning a shield power-up every certain number of points ~Adam
	public int mShieldScore = 600;
	int mShieldInterval = 300;
	[SerializeField] private GameObject mShieldEmblem;

	//For the UI of showing a meter depicting tim until next powerup
	[SerializeField] private Image mPowerUpMeter;
	[SerializeField] private GameObject mPowerUpMeterBack;
	public Text mPowerUpMeterScoreDisplay;

	//Time that the player is invincible after getting hit ~Adam
	public float mPlayerSafeTime = 0f;

	public GameObject mPlayerAvatar;
	public GameObject mPlayerDeathEffect;

	//For when we have two players ~Adam
	public GameObject mPlayer2Avatar;
	public GameObject mPlayer2DeathEffect;

	//For better GUI elements ~Adam
	[SerializeField] private GUIStyle mScoreManStyle;
	[SerializeField] private GUIStyle mHighScoreStyle;

	//List of level names ~Adam
	[SerializeField] private string[] mLevelNames;

	//For making the background scrolling persist between levels ~Adam
	public Vector3 mBackgroundPosition;
	public float mBackgroundOffset;

	//For using the new UI system so we can use an image for a font ~Adam
	public Text mLevelInfoText;
	public Text mHighScoreText;
	
    public Canvas mHighscoreCanvas;


	//For differentiating player 1 and player 2's scores ~Adam
	public int mP1Score = 0;
	public int mP2Score = 0;


	//For showing what power up is going to spawn next ~Adam
	[SerializeField] private Image mNextPowerUpImage;
	[SerializeField] private Sprite mShieldEmblemSprite;
	[SerializeField] private Sprite mTripleEmblemSprite;

	public bool mInCoOpMode = false;
	public int mP1Lives = 100;
	public int mP2Lives = 0;

	//For keepign the high score up to date via Unity's PlayerPrefs ~Adam
	void StoreHighscore(int newHighscore)
	{
		int oldHighscore = PlayerPrefs.GetInt("highscore", 0);    
		if(newHighscore > oldHighscore)
			PlayerPrefs.SetInt("highscore", newHighscore);
	}



	void Start () 
	{
        if (Application.isMobilePlatform)
        {
            mLivesRemaining = 25;
            mMaxLives = 25;
        }


		//Delete self if there's already a score manager to prevent duplicates (this only seems to delete the new ones, which is what we want)
        foreach (var canv in gameObject.GetComponentsInChildren<Canvas>())
        {
            if (canv.transform.name == "ScoreCanvas")
            {
                mHighscoreCanvas = canv;
            }
        }
		if(mPlayer2Avatar == null && mPlayerAvatar.GetComponent<PlayerOneShipController>().mPlayerTwo.gameObject != null && mPlayerAvatar.GetComponent<PlayerOneShipController>().mPlayerTwo.gameObject.activeInHierarchy)
		{
			mPlayer2Avatar = mPlayerAvatar.GetComponent<PlayerOneShipController>().mPlayerTwo.gameObject;
		}
	}

	//Pesist between level loads/reloads ~adam
	void Awake()
	{


		DontDestroyOnLoad (transform.gameObject);


		//Figure out how old this ScoreManager is ~Adam
		if(mOriginalLevel == 0)
		{
			mOriginalLevel = Application.loadedLevel;
		}
		ScoreManager[] otherScoreManagers = FindObjectsOfType<ScoreManager>();

		//Delete self if there's an older ScoreManager ~Adam
		foreach(ScoreManager otherScoreManager in otherScoreManagers)
		{
			if (otherScoreManager != null && otherScoreManager.mOriginalLevel < mOriginalLevel)
			{
				Destroy(this.gameObject);
			}
		}
		mPlayerAvatar = GameObject.FindGameObjectWithTag("Player").gameObject;

		if (mPlayerAvatar != null) {

			if (mPlayerAvatar.GetComponent<PlayerOneShipController> ().mPlayerTwo.gameObject != null && mPlayerAvatar.GetComponent<PlayerOneShipController> ().mPlayerTwo.gameObject.activeInHierarchy) {
				mPlayer2Avatar = mPlayerAvatar.GetComponent<PlayerOneShipController> ().mPlayerTwo.gameObject;
			}
		}
	}

	// Update is called once per frame
	void Update () 
	{

		mCurrentLevel = Application.loadedLevel; //Wasn't affected in either Awake() or Start()




		mPlayerSafeTime-=Time.deltaTime;



		//For showing the meter that says how close the player is to a power up ~Adam
		if(mPowerUpMeterScoreDisplay != null && mPowerUpMeter != null && mPowerUpMeterBack != null)
		{

			if(mScore < 0)
			{

				mPowerUpMeter.rectTransform.localScale = new Vector3(0f, 1f, 1f);
				mPowerUpMeterScoreDisplay.text = "Loser. Try Shooting.";
			}
			else
			{

				mPowerUpMeterScoreDisplay.text = "Total Score: " + mScore;

				if(mPowerUpScore < mShieldScore)
				{
					float barAdjust = 1f*(mPowerUpInterval-(mPowerUpScore-mScore))/mPowerUpInterval;

					if(mPowerUpScore == 400f)
					{
						barAdjust = 1f*((mScore))/mPowerUpScore;
					}

					if(barAdjust < 0f)
					{
						barAdjust = 0f;
					}
					mPowerUpMeter.rectTransform.localScale = new Vector3(barAdjust, 1f,1f); 
				}
				else
				{
					float barAdjust = 1f*(mShieldInterval-(mShieldScore-mScore))/mShieldInterval;
					if(barAdjust < 0f)
					{
						barAdjust = 0f;
					}
					mPowerUpMeter.rectTransform.localScale = new Vector3(barAdjust, 1f,1f); 
				}
			}
		}
		//Spawn a triple bullet power up every 500 kills (assuming 1 point per kill) ~Adam
		if(mScore >= mPowerUpScore)
		{
			float spawnXPos = Random.Range(-16f,16f);
			float spawnyPos = Random.Range(-17f,23f);
			Instantiate(mTripleBulletEmblem, new Vector3(spawnXPos, spawnyPos, -2f), Quaternion.identity);
			mPowerUpMeterBack.GetComponent<Animator>().Play("PowerPointMeterFlash_Anim");
			mPowerUpScore += (mPowerUpInterval+mShieldInterval);
		}
		//Spawn a shield power up every 300 kills (assuming 1 point per kill) ~Adam
		if(mScore >= mShieldScore)
		{
			float spawnXPos = Random.Range(-16f,16f);
			float spawnyPos = Random.Range(-17f,23f);
			Instantiate(mShieldEmblem, new Vector3(spawnXPos, spawnyPos, -2f), Quaternion.identity);
			mPowerUpMeterBack.GetComponent<Animator>().Play("PowerPointMeterFlash_Anim");
			mShieldScore += (mShieldInterval+mPowerUpInterval);
		}

		//Make sure we have a reference to the player's ship ~Adam
		if (mPlayerAvatar == null && GameObject.FindGameObjectWithTag("Player") != null)
		{
			mPlayerAvatar = GameObject.FindGameObjectWithTag("Player").gameObject;
			
		}

		//Play particle effect and overlay over the player while invincible ~Adam
		if(mPlayerAvatar != null && mPlayerAvatar.GetComponent<PlayerShipController>() != null)
		{
			if(mPlayerSafeTime > 0)
			{
				mPlayerAvatar.GetComponent<PlayerShipController>().mMainShipHitSprite.SetActive(true);
				mPlayerAvatar.GetComponent<PlayerShipController>().mDamageParticles.SetActive(true);
				if(mPlayerAvatar.GetComponent<PlayerShipController>().mShipRecovered)
				{
					mPlayerAvatar.GetComponent<PlayerShipController>().mSecondShipHitSprite.SetActive(true);
				}
			}
			else
			{
				mPlayerAvatar.GetComponent<PlayerShipController>().mDamageParticles.SetActive(false);
				mPlayerAvatar.GetComponent<PlayerShipController>().mMainShipHitSprite.SetActive(false);
				mPlayerAvatar.GetComponent<PlayerShipController>().mSecondShipHitSprite.SetActive(false);
			}
		}
		if(mPlayer2Avatar != null)
		{
			if(mPlayerSafeTime > 0)
			{
				mPlayer2Avatar.GetComponent<PlayerShipController>().mMainShipHitSprite.SetActive(true);
				mPlayer2Avatar.GetComponent<PlayerShipController>().mDamageParticles.SetActive(true);
				if(mPlayer2Avatar.GetComponent<PlayerShipController>().mShipRecovered)
				{
					mPlayer2Avatar.GetComponent<PlayerShipController>().mSecondShipHitSprite.SetActive(true);
				}
			}
			else
			{
				mPlayer2Avatar.GetComponent<PlayerShipController>().mDamageParticles.SetActive(false);
				mPlayer2Avatar.GetComponent<PlayerShipController>().mMainShipHitSprite.SetActive(false);
				mPlayer2Avatar.GetComponent<PlayerShipController>().mSecondShipHitSprite.SetActive(false);
			}
		}

		//Say what shows up in the UI box for the level name ~Adam
		switch(Application.loadedLevelName)
		{
		case "Level26_Boss":
			mLevelInfoText.text = "Game Over";
			break;
		case "Credits":
			mLevelInfoText.text = "Thank you for playing!";
			break;
		case "EndGame":
			mLevelInfoText.text = "Game Over";
			break;
		default:
			if(mLevelNames.Length > Application.loadedLevel && mLevelNames[Application.loadedLevel].Contains("Boss"))
			{
				mLevelInfoText.text = mLevelNames[Application.loadedLevel];
			}
			else if (mLevelNames.Length > Application.loadedLevel)
			{
				mLevelInfoText.text = "Level "+ (Application.loadedLevel-Application.loadedLevel/6) + ":\n" + mLevelNames[Application.loadedLevel];
			}
			break;
		}


		//Display High Score ~Adam
		mHighScoreText.text = "High Score:\n" + PlayerPrefs.GetInt("highscore", 0);

		StoreHighscore (mScore);

		//Show what power up is spawning next ~Adam
		if(mNextPowerUpImage != null && mShieldEmblemSprite != null && mTripleEmblemSprite != null)
		{
			if(mShieldScore < mPowerUpScore)
			{
				mNextPowerUpImage.sprite = mShieldEmblemSprite;
			}
			else
			{
				mNextPowerUpImage.sprite = mTripleEmblemSprite;
			}
		}

		//If we're out of lives, wait a short bit for the player explosion to play, then clean up the objects that normally persist between levels ~Adam
		//Then go to the EndGame scene and delete this game object ~Adam
		if(mLivesRemaining <= 0 && mPlayerSafeTime <= 0 && (mPlayer2Avatar == null || !mPlayer2Avatar.activeInHierarchy) && (mPlayerAvatar == null || !mPlayerAvatar.activeInHierarchy))
		{

			if(FindObjectOfType<LevelKillCounter>() != null)
			{
				Destroy(FindObjectOfType<LevelKillCounter>().gameObject);
			}
			mLevelInfoText.text = "\nGame Over";
			if(GameObject.Find("PowerMeterCanvas") != null)
			{
				GameObject.Find("PowerMeterCanvas").SetActive (false);
			}
			if(mPlayerAvatar != null)
			{
				Destroy(mPlayerAvatar.gameObject);
			}
			if(mPlayer2Avatar != null)
			{
				Destroy(mPlayer2Avatar.gameObject);
			}
			Application.LoadLevel("EndGame");
			this.enabled = false;
			//Destroy(this.gameObject);
			
		}

		//Let dead players borrow lives to repsawn in Co-Op mode ~Adam
		//Only allow the repsawning in Co-Op mode while a player isn't invincible and there are still lives leftover ~Adam
		if(mInCoOpMode && mPlayerSafeTime <= 0f && mLivesRemaining > 1)
		{
			//For player 1 coming back ~Adam
			if(!mPlayerAvatar.activeInHierarchy && mPlayer2Avatar.activeInHierarchy)
			{
				if( ( (InputManager.ActiveDevice.Action1.WasPressed || InputManager.ActiveDevice.Action4.WasPressed) 
				    && InputManager.ActiveDevice != mPlayer2Avatar.GetComponent<PlayerShipController>().mPlayerInputDevice) 
				   || Input.GetButtonDown("FireGun") )
				{
					if(mP2Lives >= 10)
					{
						mP2Lives -= 5;
						mP1Lives += 5;
						if(AchievementManager.instance != null)
						{
                        	AchievementManager.instance.PostAchievement("Mortgage");
						}
					}
					else if(mP2Lives > 5)
					{
						mP2Lives -= 3;
                        mP1Lives += 3;
						if(AchievementManager.instance != null)
						{
							AchievementManager.instance.PostAchievement("Mortgage");
						}
					}
					else
					{
						mP2Lives -= 1;
                        mP1Lives += 1;
						if(AchievementManager.instance != null)
						{
							AchievementManager.instance.PostAchievement("Mortgage");
						}
					}
					mPlayerAvatar.transform.position = mPlayer2Avatar.transform.position;
					mPlayerAvatar.SetActive(true);
					mPlayerAvatar.GetComponent<PlayerShipController>().Respawn ();
				}
			}

			//For player 2 coming back ~Adam
			if(mPlayerAvatar.activeInHierarchy && !mPlayer2Avatar.activeInHierarchy)
			{
				if(( (InputManager.ActiveDevice.Action1.WasPressed || InputManager.ActiveDevice.Action4.WasPressed) 
				    && InputManager.ActiveDevice != mPlayerAvatar.GetComponent<PlayerShipController>().mPlayerInputDevice) 
				   || Input.GetButtonDown("FireGunP2") )
				{
					if(mP1Lives >= 10)
					{
						mP1Lives -= 5;
                        mP2Lives += 5;
						if(AchievementManager.instance != null)
						{
							AchievementManager.instance.PostAchievement("Mortgage");
						}
					}
					else if(mP1Lives > 5)
					{
						mP1Lives -= 3;
                        mP2Lives += 3;
						if(AchievementManager.instance != null)
						{
							AchievementManager.instance.PostAchievement("Mortgage");
						}
					}
					else
					{
						mP1Lives -= 1;
                        mP2Lives += 1;
						if(AchievementManager.instance != null)
						{
							AchievementManager.instance.PostAchievement("Mortgage");
						}
					}
					mPlayer2Avatar.transform.position = mPlayerAvatar.transform.position;
					mPlayer2Avatar.SetActive(true);
					mPlayer2Avatar.GetComponent<PlayerShipController>().Respawn ();
				}
			}

		}
	}//END of Update()



	//Used for adding/subtracting points
	public void AdjustScore(int points, bool mPlayer1Kill)
	{
		if(mScore < 0)
		{
			mP1Score = 0;
			mP2Score = 0;
			mScore = 0;
		}
		mScore += points;
		if(mPlayer1Kill)
		{
			mP1Score += points;
		}
		else
		{
			mP2Score += points;
		}
	}

	public void HalfScore()
	{
		mScore /= 2;
	}

	public void DoubleScore()
	{
		mScore *= 2;
	}

	//Handle damage to Player 1 ~Adam
	public void LoseALife()
	{
		if(mPlayerSafeTime<=0f)
		{



			//Lose a life if the player isn't shielded ~Adam
			if(!mPlayerAvatar.GetComponent<PlayerShipController>().mShielded)
			{
				Camera.main.GetComponent<CameraShaker> ().RumbleController(.1f, .2f);

				if(mP1Lives == 1)
				{
					GameObject playerDeathParticles;
					playerDeathParticles = Instantiate(mPlayerDeathEffect, mPlayerAvatar.transform.position, Quaternion.identity) as GameObject;
				}
				if(mPlayerAvatar.GetComponent<PlayerShipController>().mShipRecovered)
				{
					mPlayerAvatar.GetComponent<PlayerShipController>().mShipRecovered = false;
					mPlayerAvatar.GetComponent<PlayerShipController>().StartSpin();
					Camera.main.GetComponent<CameraShaker>().ShakeCameraDeath();


				}
				else
				{
					mScore -= 10;
					mP1Score -= 10;
					if(mP1Score < 0)
					{
						mP2Score += mP1Score;
						mP1Score = 0;
					}
					if(mScore <-1)
					{
						mScore = -1;
						mP1Score = 0;
						mP2Score = 0;
					}
					mLivesRemaining--;
					mP1Lives--;
					mPlayerAvatar.GetComponent<PlayerShipController>().StartSpin();
					mPlayerAvatar.GetComponent<PlayerShipController>().TakeStatDamage();
					Camera.main.GetComponent<CameraShaker>().ShakeCameraDeath();


				}
			}
			else
			{
				//mScore -= 10;
				mPlayerAvatar.GetComponent<PlayerShipController>().StartSpin();
				Camera.main.GetComponent<CameraShaker>().ShakeCameraDeath();
				if(AchievementManager.instance != null)
				{
					AchievementManager.instance.IDontCare.IncreseValue();
				}
			}

			//If that wasn't the last life, go invulnerable, otherwise go back to the title screen
			if(mP1Lives <= 0)
			{

				Camera.main.GetComponent<CameraShaker> ().RumbleController(.6f, 3.15f);

				mPlayerAvatar.gameObject.SetActive (false);

				mPlayerSafeTime = 3f;

			}
			else
			{
				mPlayerSafeTime = 2f;
			}

		}
	}//END of LoseALife()

	//Handle damage to Player 2 ~Adam
	public void LosePlayerTwoLife()
	{
		if(mPlayerSafeTime<=0f)
		{

			//Lose a life if the player isn't shielded ~Adam
			if(!mPlayer2Avatar.GetComponent<PlayerShipController>().mShielded)
			{
				Camera.main.GetComponent<CameraShaker> ().RumbleController(.1f, .2f);

				if(mP2Lives == 1)
				{
					GameObject playerDeathParticles;
					playerDeathParticles = Instantiate(mPlayer2DeathEffect, mPlayer2Avatar.transform.position, Quaternion.identity) as GameObject;
				}
				if(mPlayer2Avatar.GetComponent<PlayerShipController>().mShipRecovered)
				{
					mPlayer2Avatar.GetComponent<PlayerShipController>().mShipRecovered = false;
					mPlayer2Avatar.GetComponent<PlayerShipController>().StartSpin();
					Camera.main.GetComponent<CameraShaker>().ShakeCameraDeath();
				}
				else
				{
					mScore -= 10;
					mP2Score -= 10;
					if(mP2Score < 0)
					{
						mP1Score += mP2Score;
						mP2Score = 0;
					}
					if(mScore <-1)
					{
						mScore = -1;
						mP1Score = 0;
						mP2Score = 0;
					}
					mLivesRemaining--;
					mP2Lives--;
					mPlayer2Avatar.GetComponent<PlayerShipController>().StartSpin();
					mPlayer2Avatar.GetComponent<PlayerShipController>().TakeStatDamage();
					Camera.main.GetComponent<CameraShaker>().ShakeCameraDeath();
				}
			}
			else
			{
				mPlayer2Avatar.GetComponent<PlayerShipController>().StartSpin();
				Camera.main.GetComponent<CameraShaker>().ShakeCameraDeath();

			
			}
			
			//If that wasn't the last life, go invulnerable, otherwise go back to the title screen ~Adam
			if(mP2Lives <= 0)
			{

				Camera.main.GetComponent<CameraShaker> ().RumbleController(3f, 2f);

				mPlayer2Avatar.gameObject.SetActive (false);

				mPlayerSafeTime = 3f;
				
			}
			else
			{
				mPlayerSafeTime = 2f;
			}
			
		}
	}//END of LosePlayerTwoLife()

	//This gets called when a player gets hit and then checks to see which player's score and lives to modify ~Adam
	public void HitAPlayer(GameObject playerHit)
	{
		if(playerHit == mPlayerAvatar)
		{
			LoseALife ();
		}
		else if(playerHit == mPlayer2Avatar)
		{
			LosePlayerTwoLife ();
		}
	}

	//Redistribute lives between Player 1 and Player 2 for Co-Op mode ~Adam
	public void StartCoOpMode()
	{
		mInCoOpMode = true;
		mMaxLives = 50;
		mP1Lives = 50;
		mP2Lives = 50;
		mLivesRemaining = 100;
	}

}
