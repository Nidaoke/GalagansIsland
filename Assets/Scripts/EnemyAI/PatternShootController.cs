using UnityEngine;
using System.Collections;

public class PatternShootController : MonoBehaviour 
{
	[SerializeField] private SwarmGrid mSwarmGrid;
	[SerializeField] private float mFireTime = 10f;
	[SerializeField] private bool mIterativeFire = false;
	[SerializeField] private float mFireIterationTime = 0.1f;
	[SerializeField] private LevelKillCounter mKillCounter;
	[SerializeField] private bool mImmediateFirstFire = false;
	// Use this for initialization
	void Start () 
	{
		mKillCounter=FindObjectOfType<LevelKillCounter>();
		StartCoroutine(ShootCylce());

	}


	IEnumerator ShootCylce()
	{
		if(!mImmediateFirstFire)
		{
			yield return new WaitForSeconds(mFireTime);
		}
		else
		{
			mImmediateFirstFire = false;
		}
		StartCoroutine(ShootPattern());

		if(!mKillCounter.mLevelComplete)
		{
			StartCoroutine(ShootCylce());
		}
	}

	IEnumerator ShootPattern()
	{
		foreach (SwarmGridSlot slot in mSwarmGrid.mGridSlots)
		{
			slot.PatternShoot();
			if(mIterativeFire)
			{
				yield return new WaitForSeconds(mFireIterationTime);
			}
		}
	}
}
