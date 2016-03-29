using UnityEngine;
using System.Collections;

//Turns components on when a boss is under a health threshold ~Adam

public class BossComponentActivator : MonoBehaviour 
{
	public BossCentral mBossCentral;
	public MonoBehaviour mActivatedComponent;
	public int mSwitchThreshold = 100;
	

	
	// Update is called once per frame
	void Update () 
	{
		if(mBossCentral != null && mActivatedComponent != null)
		{
			if(mBossCentral.mCurrentHealth <= mSwitchThreshold)
			{
				mActivatedComponent.enabled = true;
				this.enabled = false;
			}
		}
	}//END of Update()
}
