using UnityEngine;
using System.Collections;

//Use this for making bosses shoot/instantiate projectiles.  This script is meant to be inherited from ~Adam

public class BossShooter : MonoBehaviour 
{
	public BossCentral mBossCentral;

	public GameObject mBuildUp;
	
	public GameObject mTarget;
	
	public GameObject mBullet;

	public float mTimer = 10f;
	public float mFireRate = 0.5f;
	public float mTimerTemp = 10f;
	

	protected int mShotsFired = 0;
	public int mShots = 1;

	// Use this for initialization
	protected virtual void Start () 
	{
		mTarget = mBossCentral.mTargetedPlayer.gameObject;


	}//END of Start()
	
	// Update is called once per frame
	protected virtual void Update () 
	{
		mTarget = mBossCentral.mTargetedPlayer.gameObject;


		//Fire the eye beam on button press ~Adam
		mTimerTemp -= Time.deltaTime;
		
		if (mTimerTemp < 1 && mBuildUp != null) 
		{
			
			mBuildUp.SetActive (true);
		} 
		else if(mBuildUp != null)
		{
			
			mBuildUp.SetActive(false);
		}
		
		if (mTimerTemp <= 0) 
		{

			mTimerTemp = mFireRate;
			mShotsFired ++;
			Instantiate(mBullet, transform.position, Quaternion.identity);
			if(mShotsFired >= mShots)
			{
				mTimerTemp = mTimer;
				mShotsFired = 0;
			}
		}
		

		

	}//END of Update()
}
