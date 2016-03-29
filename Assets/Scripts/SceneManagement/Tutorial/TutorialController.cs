using UnityEngine;
using System.Collections;
using UnityEngine.UI;


//Script that handles the tutorial ~Adam

public class TutorialController : MonoBehaviour 
{
	//For handling what step of the tutorial is running ~Adam
	[SerializeField] private int mTutorialStep =1;
	private float mTutStepTimer = 0f;

	//Baseline stuff that's always in the first scene that we want to delete at the end ~Adam
	[SerializeField] private PlayerShipController mPlayer1;
	[SerializeField] private PlayerShipController mPlayer2;
	[SerializeField] private LevelKillCounter mKillCounter;
	[SerializeField] private ScoreManager mScoreMan;
	[SerializeField] private GameObject mHUD;
	[SerializeField] private GameObject mHUDMobile;
	[SerializeField] private GameObject mIcicleStorm;

	//For the movement and hover part of the tutorial ~Adam
	[SerializeField] private TutorialTargetBox[] mMovementBoxes;
	[SerializeField] private TutorialTargetBox mHoverBox;

	//For spawning enemies ~Adam
	[SerializeField] private GameObject[] mFirstSwarmSpawners;
	[SerializeField] private GameObject[] mSecondSwarmSpawners;

	//For damage and repair ~Adam
	[SerializeField] private GameObject mRepairStation;
	[SerializeField] private GetReady mGetReady;


	//For UI messages and tutorial text ~Adam
	[SerializeField] private GameObject mMainTutorialBox;
	[SerializeField] private Text mMainTutorialText;
	[SerializeField] private GameObject mOverheatTut;
	[SerializeField] private GameObject mScoreMeterTut;
	[SerializeField] private GameObject mMainMeterTut;

	[SerializeField] private BKG mBackPlane;

	//For the Damage and Repair tutorial ~Adam
	public int mRepairDoorEntered = 0;
	int mPreRepairLives = 100;
	float mPreRepairFire = 1f;
	float mPreRepairSpeed = 1f;
	bool mShipRepairMessageShown = false;


	//For double ship tutorial ~Adam
	[SerializeField] private GameObject mFirstThief;
	[SerializeField] private GameObject mThiefSpawner;

	// Use this for initialization
	void Start () 
	{
		PlayerPrefs.SetInt ("CheckPointedLevel", 0);
	}
	
	// Update is called once per frame
	void Update () 
	{
		mTutStepTimer -= Time.deltaTime;

		switch (mTutorialStep)
		{
			#region Startup Messages ~Adam
		case 1:
			mMainTutorialText.text = "Booting ship controls...";
			if(mTutStepTimer <=0f)
			{
				mTutStepTimer = 3f;
				mTutorialStep++;
			}
			break;
		case 2:
			mMainTutorialText.text = "Initiating\nRuntime\nCalibrations.";
			if(mTutStepTimer <=0f)
			{
				mTutStepTimer = 5f;
				mTutorialStep++;
			}
			break;
		case 3:
			mMainTutorialText.text = "Welcome Pilot!";
			if(mTutStepTimer <=0f)
			{
				mTutStepTimer = 3f;
				foreach(TutorialTargetBox marker in mMovementBoxes)
				{
					marker.gameObject.SetActive (true);
				}
				mTutorialStep++;
			}
			break;
			#endregion

			#region Movement tutorial ~Adam
		case 4:
			mMainTutorialText.text = "Forward Thrust Check:\n\nPlease maneuver to the marked locations.";
			int markerCount = 0;
			foreach(TutorialTargetBox marker in mMovementBoxes)
			{
				if (marker.mCleared == true)
				{
					markerCount++;
				}
			}
			if(markerCount == mMovementBoxes.Length)
			{
				mTutStepTimer = 3f;

				foreach(TutorialTargetBox marker in mMovementBoxes)
				{
					marker.gameObject.SetActive (false);
				}
				mTutorialStep++;
			}
			break;
		case 5:
			mMainTutorialText.text = "Forward Thrust Check:\n\nSuccessful.";
			if(mTutStepTimer <=0f)
			{
				mTutStepTimer = 3f;
				mHoverBox.gameObject.SetActive (true);
				mTutorialStep++;
			}
			break;
			#endregion

			#region Hover Tutorial ~Adam
		case 6:
			mMainTutorialText.text = "Lateral Thrust Check:\n\nPlease hold the Hover button to maintain position at the marked location.";
			if(mHoverBox.mCleared == true)
			{
				mTutStepTimer = 3f;
				mHoverBox.gameObject.SetActive (false);
				mTutorialStep++;
			}
			break;
		case 7:
			mMainTutorialText.text = "Lateral Thrust Check:\n\nSuccessful.";
			if(mTutStepTimer <=0f)
			{
				mTutStepTimer = 6f;
				foreach(GameObject spawner in mFirstSwarmSpawners)
				{
					spawner.SetActive (true);
				}
				mTutorialStep++;
			}
			break;
			#endregion

			#region Shooting Tutorial part 1 ~Adam
		case 8:
			mMainTutorialText.text = "Warning!\n\nHostile targets inbound from multiple directions.";
			if(mTutStepTimer <=0f)
			{
				mTutStepTimer = 5f;
				mTutorialStep++;
			}
			break;
		case 9:
			mMainTutorialText.text = "Hold the Fire button to dispatch targets.";
			if(mTutStepTimer <=0f)
			{
				mMainTutorialBox.SetActive (false);
			}
			if(mKillCounter.mKillCount >= 20)
			{
				mTutStepTimer = 2f;
				foreach(GameObject spawner in mFirstSwarmSpawners)
				{
					spawner.SetActive (false);
				}
				mMainTutorialBox.SetActive (true);
				mTutorialStep++;
			}
			break;
		case 10:
			mMainTutorialText.text = "Weapons Check:\n\n";
			if(mTutStepTimer <=0f)
			{
				mTutStepTimer = 3f;
				mTutorialStep++;
			}
			break;
		case 11:
			mMainTutorialText.text = "Weapons Check:\n\nSuccessful.";
			if(mTutStepTimer <=0f)
			{
				mTutStepTimer = 6f;
				mMainTutorialBox.SetActive (false);
				mTutorialStep++;
			}
			break;
			#endregion

			#region Score Meter tutorial ~Adam
		case 12:
			mScoreMeterTut.SetActive (true);//Talk about the Score Meter ~Adam
			if(mTutStepTimer <=0f)
			{
				mTutStepTimer = 3f;
				mMainTutorialBox.SetActive (true);
				mScoreMeterTut.SetActive (false);

				mFirstThief.SetActive (true);
				mThiefSpawner.SetActive (true);

				mTutorialStep++;
			}
			break;
			#endregion

			#region Double Ship Tutorial ~Adam
			//13: spawn brain ship.  Move to next step when it copies the player. ~Adam
		case 13:
			mMainTutorialText.text = "Warning!\n\nCloner Detected.";
			if(mTutStepTimer <=0f)
			{
				mMainTutorialBox.SetActive (false);
			}
			if(mPlayer1.mShipStolen)
			{
				mTutStepTimer = 4f;
				mMainTutorialBox.SetActive (true);
				mTutorialStep++;
			}
			break;
			//14: Move to next step when the player gets a side ship ~Adam
		case 14:
			mMainTutorialText.text = "Shoot your double to take control of it.";
			if(mTutStepTimer <=0f)
			{
				mMainTutorialBox.SetActive (false);
			}
			if(mPlayer1.mShipRecovered)
			{
				mTutStepTimer = 4f;
				mMainTutorialBox.SetActive (true);
				mTutorialStep++;
			}
			break;
			//15: Move to next step when the player swaps the ship position ~Adam
		case 15:
			mMainTutorialText.text = "Press the Flip button to change the position of your second ship.";

			if(!mPlayer1.secondShipOnHip)
			{
				mTutStepTimer = 4f;
				mMainTutorialBox.SetActive (true);

				mThiefSpawner.SetActive (false);
				
				foreach(GameObject spawner in mSecondSwarmSpawners)
				{
					spawner.SetActive (true);
				}

				mTutorialStep++;
			}
			break;
			#endregion

			#region More Shooting and Overheat Tutorial ~Adam
		case 16:
			mMainTutorialText.text = "Warning!\n\nMore hostile targets inbound.";
			if(mTutStepTimer <=0f)
			{
				mTutStepTimer = 6f;
				mMainTutorialBox.SetActive (false);
				mOverheatTut.SetActive (true);

				mTutorialStep++;
			}
			break;
		case 17://Talk about the Overheat Meter ~Adam

			if(mTutStepTimer <=0f)
			{
				mTutStepTimer = 3f;
				mMainTutorialBox.SetActive (false);
				mOverheatTut.SetActive (false);
			}
			if(mKillCounter.mKillCount >= 100 && !mKillCounter.mRemainingEnemy)
			{
				mTutStepTimer = 5f;
				mMainTutorialBox.SetActive (true);
				foreach(GameObject spawner in mSecondSwarmSpawners)
				{
					spawner.SetActive (false);
				}
				mTutorialStep++;
			}
			break;
		case 18:
			mMainTutorialText.text = "Hostiles dispersed.";
			if(mTutStepTimer <=0f)
			{
				mTutStepTimer = 6f;
				mTutorialStep++;
			}
			break;
			#endregion

			#region Talk about setting Checkpoints ~Adam
		case 19:
			mMainTutorialText.text = "Hold one of the Set Checkpoint Buttons to spend movement or gun speed to make a Checkpoint.";
			if(mPlayer1.GetComponent<PlayerOneShipController>().mCheckPointsRemaining < 3)
			{
				mTutStepTimer = 6f;
				mMainTutorialBox.SetActive (false);
				mMainMeterTut.SetActive (true);
				mTutorialStep++;
			}
			break;
			#endregion

			#region Damage and Repair Tutorial ~Adam
		case 20://Talk about the Health and Damage meters ~Adam

			if(mTutStepTimer <=0f)
			{
				mTutStepTimer = 3f;
				mMainTutorialBox.SetActive (true);
				mMainMeterTut.SetActive (false);
				mRepairStation.SetActive(true);
				mTutorialStep++;
			}
			break;
		case 21:
			mMainTutorialText.text = "Repair station ahead.\n\nChoose repair area to prioritize.";
			if(mTutStepTimer <=0f)
			{
				mTutStepTimer = 3f;
				mMainTutorialBox.SetActive (false);

				//Set up Pre-Repair values ~Adam
				mPreRepairLives = mScoreMan.mP1Lives;
				mPreRepairFire = mPlayer1.mFireUpgrade;
				mPreRepairSpeed = mPlayer1.mMoveUpgrade;
				
				mTutorialStep++;
			}
			break;
		case 22:
			mGetReady.gameObject.SetActive (false);

			if(mRepairStation != null && mRepairStation.GetComponent<RepairStation>().mServicedP1 == true)
			{
				if(mPreRepairLives < mScoreMan.mP1Lives)
				{
					mRepairDoorEntered = 1;
				}
				else if(mPreRepairFire < mPlayer1.mFireUpgrade)
				{
					mRepairDoorEntered = 2;
				}
				else if(mPreRepairSpeed < mPlayer1.mMoveUpgrade)
				{
					mRepairDoorEntered = 3;
				}
			}

			switch (mRepairDoorEntered)
			{
				case 0:
					if(mRepairStation == null)
					{
						mMainTutorialText.text = "Repairs foregone.\n\nWell, you seem confident.";
						if(!mShipRepairMessageShown)
						{
							mMainTutorialBox.SetActive (true);
							mTutStepTimer = 3f;
							mShipRepairMessageShown = true;
							}

					}
					break;
				case 1:
					mMainTutorialText.text = "Ship Hull Repaired!";
					if(!mShipRepairMessageShown)
					{
						mMainTutorialBox.SetActive (true);
						mTutStepTimer = 3f;
						mShipRepairMessageShown = true;
					}

					break;
				case 2:
					mMainTutorialText.text = "Rate of Fire Repaired!";
					if(!mShipRepairMessageShown)
					{
						mMainTutorialBox.SetActive (true);
						mTutStepTimer = 3f;
						mShipRepairMessageShown = true;
					}

					break;
				case 3:
					mMainTutorialText.text = "Movement Speed Repaired!";
					if(!mShipRepairMessageShown)
					{
						mMainTutorialBox.SetActive (true);
						mTutStepTimer = 3f;
						mShipRepairMessageShown = true;
					}

					break;
				default:
					break;
			}

			if(mTutStepTimer <0)
			{
				mMainTutorialBox.SetActive (false);

			}

			if(mRepairStation == null)
			{
				Debug.Log ("Repair station offscreen");
				if(mTutStepTimer <= -1f)
				{
					Debug.Log ("reseting Step Timer");
					mTutStepTimer = 4f;
				}

				if(mTutStepTimer <= 0f)
				{
					Debug.Log ("Going to Step 19");
					mTutStepTimer = 5f;
					mMainTutorialBox.SetActive (true);

					mTutorialStep++;
				}
			}
			mGetReady.gameObject.SetActive (false);
			break;
			#endregion

			#region End of tutorial ~Adam
		case 23:
			mMainTutorialText.text = "Runtime Calibrations Complete!";
			if(mTutStepTimer <= 0f)
			{
				mTutStepTimer = 5f;
				mTutorialStep++;
			}
			break;
		case 24:
			mMainTutorialText.text = "Entering Hostile Territory...";
			mIcicleStorm.SetActive(true);
			if(mTutStepTimer <= 0f)
			{
				mTutStepTimer = 3f;
				mTutorialStep++;
			}
			break;
		case 25:
			mMainTutorialText.text = "Get Ready!";
			if(mTutStepTimer <= 0f)
			{
				//Do things to transition to Level 1 ~Adam
				Destroy(mPlayer1.gameObject);
				Destroy(mPlayer2.gameObject);
				Destroy(mKillCounter.gameObject);
				Destroy(mScoreMan.gameObject);
				Destroy(mHUD);
				Destroy(mHUDMobile);
				mBackPlane.mFadeAway = true;
				PlayerPrefs.SetInt("PlayedTutorial", 1);

				Application.LoadLevel (1);
			}
			break;
			#endregion

		default:
			break;
		}
	}
}
