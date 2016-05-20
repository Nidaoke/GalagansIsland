using UnityEngine;
using System.Collections;

public class CoffinController : MonoBehaviour 
{
	[SerializeField] private SwarmGrid mCoffinCover;
	[SerializeField] private SwarmGrid mCoffinBat;
	[SerializeField] private SwarmGrid mHeartSwarm;
	[SerializeField] private GameObject mHeartSpawner;
	[SerializeField] private ShootableGenerator mFieldGenerator;
	[SerializeField] private ShieldKiller mShieldKiller;
	[SerializeField] private LevelKillCounter mKillCounter;
	[SerializeField] private EnemyShipSpawner[] mCoffinSpawners;

	
	// Update is called once per frame
	void Update () 
	{
		//If the force fields are down, open the coffin and stop the shield killer beams~Adam
		if(mFieldGenerator.GetHitPoints()[0] <= 0)
		{
			
			mCoffinCover.ChangeFormation(1);
			mCoffinBat.ChangeFormation(1);
			mHeartSpawner.SetActive(true);
			mShieldKiller.SetTimer(0.05f);
			foreach(EnemyShipSpawner spawner in mCoffinSpawners)
			{
				spawner.mSpawning = false;
			}
		}
		else
		{
			mCoffinCover.ChangeFormation(0);
			mCoffinBat.ChangeFormation(0);
			if( (mKillCounter.mKillCount < mKillCounter.mRequiredKills) || ( (mKillCounter.mKillCount >= mKillCounter.mRequiredKills) && !mHeartSwarm.CheckIfSwarmEmpty()) )
			{
				foreach(EnemyShipSpawner spawner in mCoffinSpawners)
				{
					spawner.mSpawning = true;
				}
			}
		}
	
	}//END of Update()

}
