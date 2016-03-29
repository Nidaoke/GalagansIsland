using UnityEngine;
using System.Collections;

//Changes the type of bullet a BossShooter uses when the BossCentral component is below a certain health threshold ~Adam

public class BossBulletChanger : MonoBehaviour 
{
	public BossCentral mBossCentral;
	public BossShooter mShooter;
	public GameObject mReplacementBullet;
	public int mSwitchThreshold = 100;


	
	// Update is called once per frame
	void Update () 
	{
		if(mBossCentral != null && mShooter != null && mReplacementBullet != null)
		{
			if(mBossCentral.mCurrentHealth <= mSwitchThreshold)
			{
				mShooter.mBullet = mReplacementBullet;
			}
		}
	}//END of Update()
}
