using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Use this as the main controller/central body for the boss.  This script is meant to be inherited from ~Adam
public class BossCentral : MonoBehaviour 
{
	public ScoreManager mScoreMan;
	public PlayerShipController mPlayer1;
	public PlayerShipController mPlayer2;

	public PlayerShipController mTargetedPlayer;

	public List<BossWeakPoint> mWeakPoints = new List<BossWeakPoint>();
	public List<GameObject> mWeapons = new List<GameObject>();
	public GameObject mDeathWeapon;
	public int mDeathWeaponThreshhold;

	public float mCurrentHealth = 0f;
	public float mTotalHealth = 0f;
	



	//For Dying and spawning the next boss ~Adam
	public bool mDying = false;
	public float mDeathTimer = 5f;
	public GameObject mDeathEffect;

	public Vector3 mMoveTarget = new Vector3(0f,0f,-2f);
	public float mMoveSpeed = 15f;
	
	public float[] mBounds;
	public float mEntryTime = 5f;
	public bool mFightStarted = false;

	public int mScoreValue = 500;

	// Use this for initialization
	protected virtual void Start () 
	{
		mScoreMan = FindObjectOfType<ScoreManager>();
		if(mScoreMan.mPlayerAvatar != null)
		{
			mPlayer1 = mScoreMan.mPlayerAvatar.GetComponent<PlayerShipController>();
			mTargetedPlayer = mPlayer1;
		}
		if(mScoreMan.mPlayer2Avatar != null)
		{
			mPlayer2 = mScoreMan.mPlayer2Avatar.GetComponent<PlayerShipController>();
		}

	}//END of Start()
	
	// Update is called once per frame
	protected virtual void Update () 
	{
		if(mScoreMan == null)
		{
			mScoreMan = FindObjectOfType<ScoreManager>();
			if(mScoreMan.mPlayerAvatar != null)
			{
				mPlayer1 = mScoreMan.mPlayerAvatar.GetComponent<PlayerShipController>();
				mTargetedPlayer = mPlayer1;
			}
			if(mScoreMan.mPlayer2Avatar != null)
			{
				mPlayer2 = mScoreMan.mPlayer2Avatar.GetComponent<PlayerShipController>();
			}
		}

		//Check on and remove null WeakPoints and weapons~Adam
		mWeakPoints.Remove(null);
		mWeapons.Remove(null);



		if(!mDying)
		{
			//Bring the boss on-screen ~Adam
			if(!mFightStarted)
			{
				BossEntry();
			}
			else
			{
				//Make sure weak points die in the right order ~Adam
				if(mWeakPoints.Count > 0)
				{
					mWeakPoints[0].mActiveWeakPoint = true;
				}
				foreach(BossWeakPoint weakPoint in mWeakPoints)
				{
					if(!weakPoint.mActiveWeakPoint)
					{
						weakPoint.enabled = false;
						weakPoint.GetComponent<Collider>().enabled = false;
					}
					else
					{
						weakPoint.enabled = true;
						weakPoint.GetComponent<Collider>().enabled = true;
					}
				}

				//Move the boss around ~Adam
				BossMovement();


				//Target the player with the higher score ~Adam
				if(mScoreMan.mP1Score >= mScoreMan.mP2Score && mPlayer1.gameObject.activeInHierarchy)
				{
					mTargetedPlayer = mPlayer1;

				}
				else if(mPlayer2.gameObject.activeInHierarchy)
				{
					mTargetedPlayer = mPlayer2;
				}

				//Activate death weapon if there is one ~Adam
				if(mDeathWeapon != null && mCurrentHealth <= mDeathWeaponThreshhold)
				{
					mDeathWeapon.SetActive (true);
				}

				//Check if the boss is dead yet, but don't let the boss auto-die before the fight starts ~Adam
				if(mCurrentHealth <= 0)
				{
					mDying = true;
				}


			}
		}
		else
		{
			BossDeath();
		}
	}//END of Update()

	protected virtual void BossEntry()
	{
		//Move to the center of the screen ~Adam
		transform.position = Vector3.Lerp(transform.position, mMoveTarget, mMoveSpeed*0.0005f * Time.timeScale);
		mEntryTime -= Time.deltaTime;
		//Start doing regular behavior, make the weak points vulnerable, and enable the weapons ~Adam
		if(mEntryTime <= 0f)
		{
			mFightStarted = true;
			foreach(BossWeakPoint weakPoint in mWeakPoints)
			{
				if(weakPoint.GetComponent<Collider>() != null)
				{
					weakPoint.GetComponent<Collider>().isTrigger = true;
				}
			}
			foreach(GameObject weapon in mWeapons)
			{
				weapon.SetActive (true);
			}
			mDeathWeapon.SetActive (false);
		}
	}//END of BossEntry()

	protected virtual void BossMovement()
	{
		transform.position = Vector3.Lerp(transform.position, mMoveTarget, mMoveSpeed*0.001f * Time.timeScale);

		if(Vector3.Distance (transform.position, mMoveTarget) < 7f && mBounds.Length >= 4)
		{
			mMoveTarget = new Vector3(Random.Range (mBounds[0],mBounds[1]), Random.Range (mBounds[2],mBounds[3]),-2f);
		}
	}//END of BossMovement()

	protected virtual void BossDeath()
	{
		if(mDeathTimer <= 0f)
		{
			//Let the Kill Counter know to go to the next level
			LevelKillCounter killCounter = FindObjectOfType<LevelKillCounter>();
			killCounter.mKillCount = killCounter.mRequiredKills+1;

			//Award points for death ~Adam
			if(mScoreMan.mPlayerAvatar != null && mScoreMan.mPlayerAvatar.activeInHierarchy && mScoreMan.mPlayer2Avatar != null && mScoreMan.mPlayer2Avatar.activeInHierarchy)
			{
				mScoreMan.AdjustScore (mScoreValue/2, true);
				mScoreMan.AdjustScore (mScoreValue/2, false);
			}
			else if((mScoreMan.mPlayerAvatar == null || !mScoreMan.mPlayerAvatar.activeInHierarchy) && mScoreMan.mPlayer2Avatar != null && mScoreMan.mPlayer2Avatar.activeInHierarchy)
			{
				mScoreMan.AdjustScore (mScoreValue, false);
			}
			else
			{
				mScoreMan.AdjustScore (mScoreValue, true);
			}

			Destroy(this.gameObject);
		}
		else
		{
			mDeathTimer -= Time.deltaTime;
			if(mDeathEffect != null)
			{
				mDeathEffect.SetActive (true);
			}
			//Turn off any remaining weapons and weakpoints ~Adam
			foreach(BossWeakPoint weakPoint in mWeakPoints)
			{
				weakPoint.gameObject.SetActive (false);
			}
			foreach(GameObject weapon in mWeapons)
			{
				weapon.SetActive (false);
			}
			mDeathWeapon.SetActive (false);
		}
	}
}
