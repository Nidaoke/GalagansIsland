using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlobBossCentral : BossCentral 
{
	//For ramming the player with the death weapon ~Adam
	float mDefaultSpeed;
	public float mRammingSpeed;
	public float mRamInterval;
	public float mRamTimer = 0f;
	public bool mRamming = false;
	public Vector3 mRamPoint = Vector3.zero;

	public bool spewEnemies;
	public float spewTimer;
	private float spewTimerMax;

	public GameObject vomit;

	bool mShooting = false;
	int mShotsFired = 0;
	public int mShots = 25;

	public GameObject mSpewTarget;
	public GameObject spewBullet;



	//For knocking out teeth ~Adam
	public Animator mAnimator;
	public List<RuntimeAnimatorController> mAnimationStages = new List<RuntimeAnimatorController>();
	public List<int> mHealthStages = new List<int>();
	public GameObject mDamageEffect;
	public Transform mDamageEffectPoint;

	// Use this for initialization
	protected override void Start () 
	{
		base.Start();

		mSpewTarget = GameObject.FindGameObjectWithTag ("Player");

		vomit.SetActive (false);

		spewTimerMax = spewTimer;

		mDefaultSpeed = mMoveSpeed;
	}//END of Start()
	
	// Update is called once per frame
	protected override void Update () 
	{
		base.Update ();

		if(!mDying)
		{
			if(mSpewTarget == null)
			{
				mSpewTarget = GameObject.FindGameObjectWithTag ("Player");
			}

			if (spewEnemies) {

				if(spewTimer < .5f){

					vomit.SetActive(true);
				}

				if (spewTimer > 0) {
					
					spewTimer -= Time.deltaTime;
				} else {

					vomit.SetActive(true);
					
					spewTimer = .5f;
					//Instantiate(spewBullet, transform.position, Quaternion.identity);
					mShooting = true;

					if(mShooting)
					{
						
						mShotsFired++;
						Instantiate(spewBullet, transform.position, Quaternion.identity);

						if(mShotsFired >= mShots)
						{
							vomit.SetActive(false);

							spewTimer = 10f;
							mShooting = false;
							mShotsFired = 0;
						}
					}
				}
				
			}
		}
		//Change number of sprite based on health ~Adam
		if(mHealthStages.Count >0 && mAnimationStages.Count > 0 && mCurrentHealth <= mHealthStages[0])
		{
			//Make a boom where the damage was ~Adam
			if(mCurrentHealth == mHealthStages[0] && mDamageEffect != null && mDamageEffectPoint != null)
			{
				Instantiate (mDamageEffect, mDamageEffectPoint.position, Quaternion.identity);

			}

			mAnimator.runtimeAnimatorController = mAnimationStages[0];
			mHealthStages.Remove (mHealthStages[0]);
			mAnimationStages.Remove(mAnimationStages[0]);


		}


		//Toggle Ramming when under the death weapon threshhold ~Adam
		if(!mDying && mFightStarted && mCurrentHealth <= mDeathWeaponThreshhold)
		{
			mRamTimer -= Time.deltaTime;

			if(mRamTimer <= 0f)
			{
				mRamming = true;
				mMoveSpeed = mRammingSpeed;
				//Turn off the shooting while ramming ~Adam
				foreach(GameObject weapon in mWeapons)
				{
					weapon.SetActive (false);
				}
				mDeathWeapon.SetActive (true);

				if(mRamTimer <= mRamInterval *-0.5f)
				{
					mRamming = false;
					mRamTimer = mRamInterval;
				}
			}
			else
			{
				mMoveSpeed = mDefaultSpeed;
				mRamPoint = mTargetedPlayer.transform.position;
				//Turn the shooting back on ~Adam
				foreach(GameObject weapon in mWeapons)
				{
					weapon.SetActive (true);
				}
				if(mRamTimer >0f && mRamTimer <1f)
				{
					mDeathWeapon.SetActive (true);
				}
				else
				{
					mDeathWeapon.SetActive (false);
				}
			}
		}
	}//END of Update()
	
	protected override void BossEntry()
	{
		base.BossEntry ();
	}//END of BossEntry()
	
	protected override void BossMovement()
	{
		base.BossMovement ();
		if(mRamming)
		{
			mMoveTarget = mRamPoint;
		}
		if(Vector3.Distance (transform.position, mRamPoint) < 2f)
		{
			mRamming = false;
			mRamTimer = mRamInterval;
		}
	}//END of BossMovement()
	
	protected override void BossDeath()
	{
		base.BossDeath ();
	}
}