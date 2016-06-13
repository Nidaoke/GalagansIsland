using UnityEngine;
using System.Collections;

//This script is for the game objects that only render in the editor view that enemy AI references to figure out where it should go to fly in formation ~Adam


public class SwarmGridSlot : MonoBehaviour 
{
	public bool mOccupied;

	//This is set and altered during runtime by the parent SwarmGrid game object if the SwarmGrid is capable of swapping between multiple formation shapes ~Adam
	[HideInInspector] public Transform mFormationPosition;

	//Keeping track of which enemy ship is assigned to this grid slot ~Adam
	EnemyShipAI mOccupier;

	//For having the occupying enemy shoot in a specified direction (used in the second set of level, not currently available in the public release)  ~Adam
		//This value gets set in-editor ~Adam
	[SerializeField] private float mPatternShootAngle = 0f; 
	
	// Update is called once per frame
	void Update () 
	{
		//For swapping between formation shapes ~Adam
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

	//Called when a new enemy is spawned and assigned to this spot in the swarm ~Adam
	public void SetOccupier(EnemyShipAI newShip)
	{
		mOccupier = newShip;
		mOccupied = true;
	}//END of SetOccupier

	//Gets called when the enemy occupying this grid slot dies, freeing it up for a newly spawned enemy to use ~Adam
	public void ClearOccupation()
	{
		mOccupier = null;
		mOccupied = false;
	}//END of ClearOccupation

	//For having the occupying enemy shoot in a specified direction (used in the second set of level, not currently available in the public release)  ~Adam
	//By having each slot in a swarm have a slightly different angle, the spread of the bullets can have a cohesive pattern ~Adam
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
	}//END of PatternShoot()
}
