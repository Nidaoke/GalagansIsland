using UnityEngine;
using System.Collections;

public class BossBulletSpreader : BossShooter 
{

	[SerializeField] protected bool mSecondaryShooter;
	[SerializeField] protected BossShooter mReferenceShooter;
	[SerializeField] protected float mSpreadAngle = 30f;
	protected override void Start ()
	{
		base.Start();
		if(mSecondaryShooter && mReferenceShooter != null)
		{
			mTimerTemp = mReferenceShooter.mTimerTemp;
		}
	}
	// Update is called once per frame
	protected override void Update () 
	{

		mTarget = mBossCentral.mTargetedPlayer.gameObject;
		
		

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
			GameObject bulletOne;
			GameObject bulletTwo;
			bulletOne = Instantiate(mBullet, transform.position, Quaternion.identity) as GameObject;
			bulletOne.GetComponent<EnemyBulletController>().mFireDir = Quaternion.Euler(0f,0f,mSpreadAngle) * Vector3.Normalize(mBossCentral.mTargetedPlayer.transform.position - transform.position);
			bulletOne.transform.LookAt (bulletOne.transform.position + bulletOne.GetComponent<EnemyBulletController>().mFireDir);
			bulletOne.transform.rotation = Quaternion.Euler (new Vector3 (90f, 0f, 0f) + bulletOne.transform.rotation.eulerAngles);


			bulletTwo = Instantiate(mBullet, transform.position, Quaternion.identity) as GameObject;
			bulletTwo.GetComponent<EnemyBulletController>().mFireDir = Quaternion.Euler(0f,0f,-1f*mSpreadAngle) * Vector3.Normalize(mBossCentral.mTargetedPlayer.transform.position - transform.position);
			bulletTwo.transform.LookAt (bulletTwo.transform.position + bulletTwo.GetComponent<EnemyBulletController>().mFireDir);
			bulletTwo.transform.rotation = Quaternion.Euler (new Vector3 (90f, 0f, 0f) + bulletTwo.transform.rotation.eulerAngles);

			if(mShotsFired >= mShots)
			{
				mTimerTemp = mTimer;
				mShotsFired = 0;
			}
		}
		
		
		
		
	}//END of Update()
}
