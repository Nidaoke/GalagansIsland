using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class CoOpShipPanelUI : MonoBehaviour 
{
	public bool mP2UI = false;

	ScoreManager mScoreMan;
	
	[SerializeField] private PlayerOneShipController mP1Ship;
	[SerializeField] private PlayerTwoShipController mP2Ship;

	[SerializeField] private float mHealthValue = 0f;
	[SerializeField] private float mShieldValue = 0f;
	[SerializeField] private float mOverheatValue = 0f;
	[SerializeField] private float mTripleTimerValue = 0f;

	public Image mHealthBar;
	//public Image mShieldBar;
	public Image mShieldCircleBar;
	public Color mShieldFillColor;
	public Color mShieldPulseColor;
	float mShieldPulseTime;
	public Text mShieldedLabel;

	public Image mOverheatBar;
	public Image[] mTripleTimerBar;
	public Image mFireRateBar;
	public Image mSpeedBar;



	public Text mScoreText;

	#region Leftover from when we used to display and flash ship parts
	//UI Ship pieces ~Adam
//	public GameObject mShipHull;
//	public GameObject mShipLeftWing;
//	public GameObject mShipRightWing;
//	public GameObject mShipLeftClaw;
//	public GameObject mShipRightClaw;
//	public GameObject mShipLeftGun;
//	public GameObject mShipRightGun;
	#endregion

	//For playing the overheat whistle noise
	public bool mCanPlaySteamNoise = true;

	[SerializeField] private Image mFireCheckGlow;
	[SerializeField] private Image mSpeedCheckGlow;
	[SerializeField] private Text mCheckpointCounter;

	//Sound effects for checkpoint placement
	[SerializeField] private AudioSource mCheckPointAudioSource;
	[SerializeField] private AudioClip mCheckpointSoundCharge;
	[SerializeField] private AudioClip mCheckpointSoundSuccess;
	[SerializeField] private AudioClip mCheckpointSoundFailure;

	// Use this for initialization
	void Start () 
	{
		//Find the score manager and the player ships ~Adam
		if(FindObjectOfType<ScoreManager>() != null)
		{
			mScoreMan = FindObjectOfType<ScoreManager>();
		}
		if(FindObjectOfType<PlayerOneShipController>() != null)
		{
			mP1Ship = FindObjectOfType<PlayerOneShipController>();
		}
		if(FindObjectOfType<PlayerTwoShipController>() != null)
		{
			mP2Ship = FindObjectOfType<PlayerTwoShipController>();
		}
		UpdateCheckpointCount();
	}
		

	// Update is called once per frame
	void Update () 
	{
		if(mScoreMan != null)
		{
//			mHealthValue = mScoreMan.mLivesRemaining/(mScoreMan.mMaxLives+.00001f);

			if(!mP2UI)
			{
				mHealthValue = mScoreMan.mP1Lives/(mScoreMan.mMaxLives+.00001f);
			}
			else
			{
				mHealthValue = mScoreMan.mP2Lives/(mScoreMan.mMaxLives+.00001f);
			}

			#region Leftover from when we used to display and flash ship parts
			//Flash the ship parts ~Adam
//			if(mScoreMan.mPlayerSafeTime >0f)
//			{
//				mShipHull.GetComponent<Animator>().SetInteger("UIFlashState", 1);
//				mShipLeftWing.GetComponent<Animator>().SetInteger("UIFlashState", 1);
//				mShipRightWing.GetComponent<Animator>().SetInteger("UIFlashState", 1);
//				mShipLeftClaw.GetComponent<Animator>().SetInteger("UIFlashState", 1);
//				mShipRightClaw.GetComponent<Animator>().SetInteger("UIFlashState", 1);
//			}
//			else
//			{
//				mShipHull.GetComponent<Animator>().SetInteger("UIFlashState", 0);
//				mShipLeftWing.GetComponent<Animator>().SetInteger("UIFlashState", 0);
//				mShipRightWing.GetComponent<Animator>().SetInteger("UIFlashState", 0);
//				mShipLeftClaw.GetComponent<Animator>().SetInteger("UIFlashState", 0);
//				mShipRightClaw.GetComponent<Animator>().SetInteger("UIFlashState", 0);
//			}
			#endregion
		}

		#region Leftover from when we used to display and flash ship parts
//		if(mHealthValue<0.8f)
//		{
//			mShipRightClaw.GetComponent<Animator>().SetInteger("UIFlashState", 2);
//		}
//		if(mHealthValue<0.6f)
//		{
//			mShipLeftClaw.GetComponent<Animator>().SetInteger("UIFlashState", 2);
//		}
//		if(mHealthValue<0.4f)
//		{
//			mShipRightWing.GetComponent<Animator>().SetInteger("UIFlashState", 2);
//		}
//		if(mHealthValue<0.2f)
//		{
//			mShipLeftWing.GetComponent<Animator>().SetInteger("UIFlashState", 2);
//		}
		#endregion
		if(mHealthValue < 0f)
		{
			mHealthValue = 0f;
		}


		if(!mP2UI && mP1Ship != null)
		{
			//Adjust the meters ~Adam
			mOverheatValue = mP1Ship.heatLevel/mP1Ship.maxHeatLevel;
			if(mOverheatValue < 0f)
			{
				mOverheatValue = 0f;
			}

			mShieldValue = mP1Ship.mShieldTimer/25f;
			if(mShieldValue < 0f)
			{
				mShieldValue = 0f;
			}

			mTripleTimerValue = mP1Ship.mThreeBulletTimer/30f;
			if(mTripleTimerValue < 0f)
			{
				mTripleTimerValue = 0f;
			}

			mFireRateBar.fillAmount = (mP1Ship.mFireUpgrade-0.6f)/0.4f;
			mSpeedBar.fillAmount = (mP1Ship.mMoveUpgrade-0.6f)/0.4f;

			#region Leftover from when we used to display and flash ship parts
			//Adjust Claw/Wing graphics based on damage/upgrade status ~Adam
//			if(mP1Ship.mFireUpgrade < 0.8f)
//			{
//				mShipRightClaw.GetComponent<Animator>().SetInteger("UIFlashState", 2);
//			}
//			if(mP1Ship.mFireUpgrade < 0.65f)
//			{
//				mShipLeftClaw.GetComponent<Animator>().SetInteger("UIFlashState", 2);
//			}
//			if(mP1Ship.mMoveUpgrade < 0.8f)
//			{
//				mShipRightWing.GetComponent<Animator>().SetInteger("UIFlashState", 2);
//			}
//			if(mP1Ship.mMoveUpgrade < 0.65f)
//			{
//				mShipLeftWing.GetComponent<Animator>().SetInteger("UIFlashState", 2);
//			}
//
//			//Flash the ship gun parts ~Adam
//			if(mScoreMan != null)
//			{
//				if(mP1Ship.mThreeBullet)
//				{
//					if(mScoreMan.mPlayerSafeTime >0f)
//					{
//						mShipLeftGun.GetComponent<Animator>().SetInteger("UIFlashState", 1);
//						mShipRightGun.GetComponent<Animator>().SetInteger("UIFlashState", 1);
//					}
//					else
//					{
//						mShipLeftGun.GetComponent<Animator>().SetInteger("UIFlashState", 0);
//						mShipRightGun.GetComponent<Animator>().SetInteger("UIFlashState", 0);
//					}
//				}
//				else
//				{
//					mShipLeftGun.GetComponent<Animator>().SetInteger("UIFlashState", 2);
//					mShipRightGun.GetComponent<Animator>().SetInteger("UIFlashState", 2);
//				}
//			}
			#endregion
			//Show this player's individual score ~Adam
			mScoreText.text = "P1 Score: " + mScoreMan.mP1Score;
		}
		else if(mP2UI && mP2Ship != null)
		{
			//Adjust the meters ~Adam
			mOverheatValue = mP2Ship.heatLevel/mP2Ship.maxHeatLevel;
			if(mOverheatValue < 0f)
			{
				mOverheatValue = 0f;
			}
			if(mOverheatValue > 1f)
			{
				mOverheatValue = 1f;
			}

			mShieldValue = mP2Ship.mShieldTimer/25f;
			if(mShieldValue < 0f)
			{
				mShieldValue = 0f;
			}
			if(mShieldValue > 1f)
			{
				mShieldValue = 1f;
			}

			mTripleTimerValue = mP2Ship.mThreeBulletTimer/30f;
			if(mTripleTimerValue < 0f)
			{
				mTripleTimerValue = 0f;
			}
			else if(mTripleTimerValue > 1f)
			{
				mTripleTimerValue = 1f;
			}

			mFireRateBar.fillAmount = (mP2Ship.mFireUpgrade-0.6f)/0.4f;
			mSpeedBar.fillAmount = (mP2Ship.mMoveUpgrade-0.6f)/0.4f;

			#region Leftover from when we used to display and flash ship parts ~Adam
//			//Adjust Claw/Wing graphics based on damage/upgrade status ~Adam
//			if(mP2Ship.mFireUpgrade < 0.8f)
//			{
//				mShipRightClaw.GetComponent<Animator>().SetInteger("UIFlashState", 2);
//			}
//			if(mP2Ship.mFireUpgrade < 0.65f)
//			{
//				mShipLeftClaw.GetComponent<Animator>().SetInteger("UIFlashState", 2);
//			}
//			if(mP2Ship.mMoveUpgrade < 0.8f)
//			{
//				mShipRightWing.GetComponent<Animator>().SetInteger("UIFlashState", 2);
//			}
//			if(mP2Ship.mMoveUpgrade < 0.65f)
//			{
//				mShipLeftWing.GetComponent<Animator>().SetInteger("UIFlashState", 2);
//			}
//
//			//Flash the ship gun parts ~Adam
//			if(mScoreMan != null)
//			{
//				if(mP2Ship.mThreeBullet)
//				{
//					if(mScoreMan.mPlayerSafeTime >0f)
//					{
//						mShipLeftGun.GetComponent<Animator>().SetInteger("UIFlashState", 1);
//						mShipRightGun.GetComponent<Animator>().SetInteger("UIFlashState", 1);
//					}
//					else
//					{
//						mShipLeftGun.GetComponent<Animator>().SetInteger("UIFlashState", 0);
//						mShipRightGun.GetComponent<Animator>().SetInteger("UIFlashState", 0);
//					}
//				}
//				else
//				{
//					mShipLeftGun.GetComponent<Animator>().SetInteger("UIFlashState", 2);
//					mShipRightGun.GetComponent<Animator>().SetInteger("UIFlashState", 2);
//				}
//			}
			#endregion
			//Show this player's individual score ~Adam
			mScoreText.text = "P2 Score: " + mScoreMan.mP2Score;
		}

		//Control the overheat whistle noise
		if( (mOverheatValue  < 0.9f && GetComponent<AudioSource>().isPlaying) || mOverheatValue < 0.05f)
		{
			mCanPlaySteamNoise = true;
		}
		else if (mOverheatValue > 0.9f && mCanPlaySteamNoise)
		{
			GetComponent<AudioSource>().Play();
			mCanPlaySteamNoise = false;
		}


		//Find ships if they're null
		else if (!mP2UI && mP1Ship == null)
		{
			if(FindObjectOfType<PlayerOneShipController>() != null)
			{
				mP1Ship = FindObjectOfType<PlayerOneShipController>();
			}
		}
		else if (mP2UI && mP2Ship == null)
		{
			if(FindObjectOfType<PlayerTwoShipController>() != null)
			{
				mP2Ship = FindObjectOfType<PlayerTwoShipController>();
			}
		}

		//Set the bar sizes ~Adam
		//mHealthBar.rectTransform.localScale = new Vector3(mHealthValue, 1f,1f);
		mHealthBar.fillAmount = mHealthValue;
		//mShieldBar.rectTransform.localScale = new Vector3(mShieldValue, 1f,1f);
		mShieldCircleBar.fillAmount = mShieldValue;
		if(mShieldValue > 0f)
		{
			mShieldedLabel.gameObject.SetActive (true);
			mShieldPulseTime += Time.deltaTime/Time.timeScale;
			if(mShieldPulseTime < 0.5f)
			{
				mShieldCircleBar.color = Color.Lerp(mShieldCircleBar.color, mShieldFillColor, 0.1f);
			}
			else if(mShieldPulseTime < 1f)
			{
				if(mShieldValue < 5f/30f)
				{
					mShieldCircleBar.color = Color.Lerp(mShieldCircleBar.color, Color.red, 0.1f);
				}
				else
				{
					mShieldCircleBar.color = Color.Lerp(mShieldCircleBar.color, mShieldPulseColor, 0.1f);
				}
			}
			else
			{
				mShieldPulseTime = 0f;
			}
		}
		else
		{
			mShieldedLabel.gameObject.SetActive (false);
		}
		//mOverheatBar.rectTransform.localScale = new Vector3(mOverheatValue, 1f,1f);
		mOverheatBar.fillAmount = mOverheatValue;
		//mTripleTimerBar.rectTransform.localScale = new Vector3(mTripleTimerValue, 1f,1f);
		if(mTripleTimerValue > 0.75f)
		{
			mTripleTimerBar[0].fillAmount = (mTripleTimerValue-0.75f)/0.25f;
			mTripleTimerBar[1].fillAmount = (mTripleTimerValue-0.75f)/0.25f;
			mTripleTimerBar[2].fillAmount = 1f;
			mTripleTimerBar[3].fillAmount = 1f;
			mTripleTimerBar[4].fillAmount = 1f;
			mTripleTimerBar[5].fillAmount = 1f;
			mTripleTimerBar[6].fillAmount = 1f;
			mTripleTimerBar[7].fillAmount = 1f;
		}
		else if(mTripleTimerValue > 0.5f)
		{
			mTripleTimerBar[0].fillAmount = 0f;
			mTripleTimerBar[1].fillAmount = 0f;
			mTripleTimerBar[2].fillAmount = (mTripleTimerValue-0.5f)/0.25f;
			mTripleTimerBar[3].fillAmount = (mTripleTimerValue-0.5f)/0.25f;
			mTripleTimerBar[4].fillAmount = 1f;
			mTripleTimerBar[5].fillAmount = 1f;
			mTripleTimerBar[6].fillAmount = 1f;
			mTripleTimerBar[7].fillAmount = 1f;
		}
		else if(mTripleTimerValue > 0.25f)
		{
			mTripleTimerBar[0].fillAmount = 0f;
			mTripleTimerBar[1].fillAmount = 0f;
			mTripleTimerBar[2].fillAmount = 0f;
			mTripleTimerBar[3].fillAmount = 0f;
			mTripleTimerBar[4].fillAmount = (mTripleTimerValue-0.25f)/0.25f;
			mTripleTimerBar[5].fillAmount = (mTripleTimerValue-0.25f)/0.25f;
			mTripleTimerBar[6].fillAmount = 1f;
			mTripleTimerBar[7].fillAmount = 1f;
		}
		else if(mTripleTimerValue > 0f)
		{
			mTripleTimerBar[0].fillAmount = 0f;
			mTripleTimerBar[1].fillAmount = 0f;
			mTripleTimerBar[2].fillAmount = 0f;
			mTripleTimerBar[3].fillAmount = 0f;
			mTripleTimerBar[4].fillAmount = 0f;
			mTripleTimerBar[5].fillAmount = 0f;
			mTripleTimerBar[6].fillAmount = (mTripleTimerValue)/0.25f;
			mTripleTimerBar[7].fillAmount = (mTripleTimerValue)/0.25f;
		}
		else
		{
			mTripleTimerBar[0].fillAmount = 0f;
			mTripleTimerBar[1].fillAmount = 0f;
			mTripleTimerBar[2].fillAmount = 0f;
			mTripleTimerBar[3].fillAmount = 0f;
			mTripleTimerBar[4].fillAmount = 0f;
			mTripleTimerBar[5].fillAmount = 0f;
			mTripleTimerBar[6].fillAmount = 0f;
			mTripleTimerBar[7].fillAmount = 0f;
		}
	}

	public void DoCheckpointGlow(bool fireCheck)
	{
		if(fireCheck && mFireCheckGlow != null)
		{
			mFireCheckGlow.color = Color.Lerp(mFireCheckGlow.color, Color.white,Time.deltaTime/3f);
		}
		else if (mSpeedCheckGlow != null)
		{
			mSpeedCheckGlow.color = Color.Lerp(mSpeedCheckGlow.color, Color.white,Time.deltaTime/3f);
		}
	}

	public void CheckpointGlowOff()
	{
		if(mFireCheckGlow != null && mSpeedCheckGlow != null)
		{
			mFireCheckGlow.color = Color.clear;
			mSpeedCheckGlow.color = Color.clear;
		}
	}

	public void UpdateCheckpointCount()
	{

		if(mP1Ship != null && mCheckpointCounter != null)
		{
			mCheckpointCounter.text = "CHECKPOINTS\nREMAINING: " + mP1Ship.mCheckPointsRemaining;
			if(mP1Ship.mCheckPointsRemaining < 0)
			{
				mCheckpointCounter.text = "CHECKPOINTS\nREMAINING: 0";
			}
		}

	}

	public void CheckPointAudioStop()
	{
		mCheckPointAudioSource.Stop();
	}
	public void CheckPointAudioCharge()
	{
		if(!mCheckPointAudioSource.isPlaying)
		{
			mCheckPointAudioSource.PlayOneShot(mCheckpointSoundCharge);
			//mCheckPointAudioSource.Play();
		}
	}
	public void CheckPointAudioSuccess()
	{
		StartCoroutine(CheckPointPlacedMessage());
		mCheckPointAudioSource.PlayOneShot(mCheckpointSoundSuccess);
	}
	public void CheckPointAudioFailure()
	{
		mCheckPointAudioSource.PlayOneShot(mCheckpointSoundFailure);
	}

	IEnumerator CheckPointPlacedMessage()
	{
		mCheckpointCounter.text = "CHECKPOINT\nPLACED!";

		yield return new WaitForSeconds(3f);
		UpdateCheckpointCount();

	}
}
