using UnityEngine;
using System.Collections;

public class HornetStingerBeam : MonoBehaviour 
{
	[SerializeField] private Animator mBossAnimator;
	[SerializeField] private GameObject mStingerBullet;
	bool mHasFired = false;
	[SerializeField] private GameObject mBuildup;
	[SerializeField] private GameObject mBeam;
	[SerializeField] private float mTimer = 10f;
	[SerializeField] private float mInterval = 10f;
	[SerializeField] private float mDuration = 1f;

	[SerializeField] private BossRotator mRotator;
	float mDefaultRotSpeed;
	[SerializeField] private float mAttackRotSpeed = 20f;
	[SerializeField] private bool mUseBeam = false;
	// Use this for initialization
	void Start () 
	{
		if(mRotator != null)
		{
			mDefaultRotSpeed = mRotator.mRotateSpeed;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		mTimer -= Time.deltaTime;

		//Turn on the buildup ~Adam
		if(mTimer <= mDuration +1f && mBuildup != null)
		{
			mBuildup.SetActive (true);
		}
		//Fire the laser ~Adam
		if(mTimer <= mDuration && mBeam != null)
		{
			if(mUseBeam && mBeam != null)
			{
				mBeam.SetActive (true);
			}
			if(mRotator != null)
			{
				mRotator.mRotateSpeed = mAttackRotSpeed;
			}
			if(!mHasFired && mStingerBullet != null)
			{
				Instantiate(mStingerBullet, transform.position, Quaternion.identity);
				mHasFired = true;
				mBossAnimator.Play("StingerRecover");
			}
		}
		//Turn off ~Adam
		if(mTimer <= 0f)
		{
			if(mBuildup != null)
			{
				mBuildup.SetActive (false);
			}
			if(mBeam != null)
			{
				mBeam.SetActive (false);
			}
			if(mRotator != null)
			{
				mRotator.mRotateSpeed = mDefaultRotSpeed;
			}
			mTimer = mInterval;
			mHasFired = false;
		}
	}
}
