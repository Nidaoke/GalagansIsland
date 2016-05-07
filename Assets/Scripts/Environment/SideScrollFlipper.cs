using UnityEngine;
using System.Collections;

//This script flips the level sideways to turn the game into a side-scroller, and then flips it back after a certain amount of time ~Adam
//Pair with a StartupDelay script to make it not immediately rotate and to ensure there's a ScoreManager and player for it to find ~Adam

public class SideScrollFlipper : MonoBehaviour 
{
	[SerializeField] private Transform mEnvironmentHolder;
	[SerializeField] private ScoreManager mScoreMan;
	[SerializeField] private PlayerShipController mPlayer1;
	[SerializeField] private PlayerShipController mPlayer2;
	[SerializeField] private float mSideScrolltime = 120f;

	[SerializeField] private BKG mBackgroundScroller;
	[SerializeField] private BKG_SideScrolling mBackgroundSideScroller;

	[SerializeField] private LevelKillCounter mKillCounter;


	float mPlayerDropSpeed = 0.68f;

	bool mSideScrolling = true;

	bool mReadyToFlipBack = false;

	[SerializeField] private GameObject mFinalSideScrollSegment;

	// Use this for initialization
	void Start () 
	{
		mScoreMan = FindObjectOfType<ScoreManager>();
		if(mScoreMan.mPlayerAvatar != null)
		{
			mPlayer1 = mScoreMan.mPlayerAvatar.GetComponent<PlayerShipController>();
			mPlayerDropSpeed = mPlayer1.GetMaxDropSpeed();
		}
		if(mScoreMan.mPlayer2Avatar != null)
		{
			mPlayer2 = mScoreMan.mPlayer2Avatar.GetComponent<PlayerShipController>();
		}
		mBackgroundScroller.enabled = false;
		mBackgroundSideScroller.enabled = true;

		StartCoroutine(WaitToFlipBack());
	}//END of Start()
	
	// Update is called once per frame
	void Update () 
	{
		//Flipping sideways ~Adam
		if(mSideScrolling)
		{
			FlipToSide(Camera.main.transform, 90f,false);

			if(mEnvironmentHolder != null && mEnvironmentHolder.rotation.eulerAngles.z != -90f)
			{
				FlipToSide(mEnvironmentHolder, 270,true);
			}
			if(mPlayer1 != null && mPlayer1.mSpinTimer <=0f)
			{
				FlipToSide(mPlayer1.transform, 90f, false);
				FlipToSide(mPlayer1.mMainShip.transform, 0f,true);
				mPlayer1.SetMaxDropSpeed(0f);
				FlipToSide(mPlayer1.mLaserFist.transform, 0f,true);
				mPlayer1.mLaserFist.transform.localPosition = new Vector3(-4f,0f,-1f);
				FlipToSide(mPlayer1.mBigBlast.transform, 0f,true);

				if (mPlayer1.mMoveDir.x > 0.2f && !(mPlayer1.secondShipOnHip&&mPlayer1.mShipRecovered) ) 
				{
					foreach (ParticleSystem shipTrail in mPlayer1.mMainShip.GetComponentsInChildren<ParticleSystem>()) 
					{
						if (shipTrail.gameObject != mPlayer1.mDamageParticles) 
						{
							shipTrail.enableEmission = true;
						}
					}
				}
				
				if(mPlayer1.secondShipOnHip)
				{
					mPlayer1.mSecondShip.transform.localRotation = Quaternion.Euler(Vector3.forward*90);
					//FlipToSide(mPlayer1.mSecondShip.transform, 180f,true);
					if(mPlayer1.mShipRecovered)
					{
						foreach (ParticleSystem shipTrail in mPlayer1.mSecondShip.GetComponentsInChildren<ParticleSystem>()) 
						{
							if (shipTrail.gameObject != mPlayer1.mDamageParticles) 
							{
								shipTrail.enableEmission = false;
							}
						}
					}
				}
				else
				{
					mPlayer1.mSecondShip.transform.localRotation = Quaternion.Euler(Vector3.forward*270);
					//FlipToSide(mPlayer1.mSecondShip.transform, 0f,true);
					if(mPlayer1.mMoveDir.x > 0.2f && mPlayer1.mShipRecovered)
					{
						foreach (ParticleSystem shipTrail in mPlayer1.mSecondShip.GetComponentsInChildren<ParticleSystem>()) 
						{
							if (shipTrail.gameObject != mPlayer1.mDamageParticles) 
							{
								shipTrail.enableEmission = true;
							}
						}
					}
				}

			}
			if(mPlayer2 != null && mPlayer2.mSpinTimer <=0f)
			{
				FlipToSide(mPlayer2.transform, 90f, false);
				FlipToSide(mPlayer2.mMainShip.transform, 0f,true);
				mPlayer2.SetMaxDropSpeed(0f);
				FlipToSide(mPlayer2.mLaserFist.transform, 0f,true);
				mPlayer2.mLaserFist.transform.localPosition = new Vector3(-4f,0f,-1f);
				FlipToSide(mPlayer2.mBigBlast.transform, 0f,true);
			}

			//Going back upright ~Adam
			if ( mReadyToFlipBack && (mFinalSideScrollSegment == null || mFinalSideScrollSegment.activeInHierarchy == false))
			{
				Destroy(mFinalSideScrollSegment);
				StartCoroutine(FlipBack());
			}

		}

		else
		{
			FlipToSide(Camera.main.transform, 0f,true);

			if(mEnvironmentHolder != null && mEnvironmentHolder.rotation.eulerAngles.z != -90f)
			{
				FlipToSide(mEnvironmentHolder, 0f,false);
			}
			if(mPlayer1 != null && mPlayer1.mSpinTimer <=0f)
			{
				FlipToSide(mPlayer1.transform, 0f, true);
				FlipToSide(mPlayer1.mMainShip.transform, 0f,false);
				FlipToSide(mPlayer1.mLaserFist.transform, 0f,false);
				mPlayer1.mLaserFist.transform.localPosition = new Vector3(0f,-4f,-1f);
				FlipToSide(mPlayer1.mBigBlast.transform, 0f,false);


				if(mPlayer1.secondShipOnHip)
				{
					FlipToSide(mPlayer1.mSecondShip.transform, 0f,false);
				}
				else
				{
					FlipToSide(mPlayer1.mSecondShip.transform, 180f,false);
				}

			}
			if(mPlayer2 != null && mPlayer2.mSpinTimer <=0f)
			{
				FlipToSide(mPlayer2.transform, 0f,false);
				FlipToSide(mPlayer2.mLaserFist.transform, 0f,false);
				mPlayer2.mLaserFist.transform.localPosition = new Vector3(0f,-4f,-1f);
				FlipToSide(mPlayer2.mBigBlast.transform, 0f,false);
			}
		}
	}//END of Update()

	//Make the game start side scrolling ~Adam
	void FlipToSide(Transform flippedTransform, float mDesiredAngle, bool clockwise)
	{
		if( Mathf.Abs(mDesiredAngle - flippedTransform.rotation.eulerAngles.z) < 2f)
		{
			flippedTransform.rotation = Quaternion.Euler(Vector3.forward*mDesiredAngle);
		}
		else
		{
			if(clockwise)
			{
				flippedTransform.Rotate(Vector3.back);
			}
			else
			{
				flippedTransform.Rotate(Vector3.forward);
			}
		}
	}//END of FlipToSide()

	IEnumerator WaitToFlipBack()
	{
		yield return new WaitForSeconds(mSideScrolltime);
		mReadyToFlipBack = true;
	}

	//After reaching the end of the side scrolling, switch back to normal ~Adam
	IEnumerator FlipBack()
	{
		//yield return new WaitForSeconds(mSideScrolltime);
		mSideScrolling = false;

		mBackgroundScroller.enabled = true;
		mBackgroundSideScroller.enabled = false;

		if(mPlayer1 != null)
		{
			mPlayer1.SetMaxDropSpeed(mPlayerDropSpeed);
		}
		if(mPlayer2 != null)
		{
			mPlayer2.SetMaxDropSpeed(mPlayerDropSpeed);
		}

		yield return new WaitForSeconds(5f);
		//End the level ~Adam
		mKillCounter.mKillCount = mKillCounter.mRequiredKills+50;

	}//END of WaitToFlipBack()
}
