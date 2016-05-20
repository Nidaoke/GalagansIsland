using UnityEngine;
using System.Collections;

public class SwarmGridSlot : MonoBehaviour 
{
	public bool mOccupied;

	public Transform mFormationPosition;

	EnemyShipAI mOccupier;

	[SerializeField] private float mPatternShootAngle = 0f;
	
	// Update is called once per frame
	void Update () 
	{
		if (mFormationPosition != null)
		{
			if(Vector3.Distance(transform.localPosition, mFormationPosition.localPosition) < 1f)
			{
				transform.localPosition = mFormationPosition.localPosition;

			}
			else
			{
				transform.localPosition = Vector3.Lerp(transform.localPosition, mFormationPosition.localPosition, Time.deltaTime);
			}

		}

	}

	public void SetOccupier(EnemyShipAI newShip)
	{
		mOccupier = newShip;
		mOccupied = true;
	}

	public void ClearOccupation()
	{
		mOccupier = null;
		mOccupied = false;
	}

	public void PatternShoot()
	{
		if(mOccupied && mOccupier != null)
		{
			//Have the mOccupier shoot in a fixed direction ~Adam
			if(mOccupier.mCurrentAIState == EnemyShipAI.AIState.Swarming)
			{
				mOccupier.ShootPatternBullet(mPatternShootAngle);
			}
		}
	}
}
