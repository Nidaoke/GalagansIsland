using UnityEngine;
using System.Collections;

//This is the script that controls the formations that the enemies fly in.  ~Adam
//It gets attached to an otherwise empty game object with a number of child game objects that all have the SwarmGridSlot script attached to them ~Adam
//The child SwarmGridSlots can be freely moved around in the Unity Editor environment so that anyone can design an enemy formation and then save it as a prefab without needing to know how the code behind it works. ~Adam

public class SwarmGrid : MonoBehaviour 
{
	//An array of all the GridSlot game object that this is going to have as children ~Adam
	public SwarmGridSlot[] mGridSlots;
	
	//The spot that the grid's hub moves around ~Adam
	[SerializeField] private bool mOverrideFocusPoint = false;
	[SerializeField] private Vector3 mFocusPoint = new Vector3	(0f,0f,-2f);

	//How fast the swarm grid moves ~Adam
	[SerializeField] private float mGridMoveSpeed = 3f;

	//Variables for moving the swarm up and down ~Adam
	[SerializeField] private bool mMoveVeritcal = false;
	[SerializeField] private float mVerticalMoveDist = 5f;
	[SerializeField] private bool mMovingUp = false;

	//Variables for moving the swarm left and right ~Adam
	[SerializeField] private bool mMoveHorizontal = false;
	[SerializeField] private float mHorizontaMoveDist = 5f;
	[SerializeField] private bool mMovingRight = false;

	//The direction the grid is going to be moving for circluar movement ~Adam
	[SerializeField] private bool mMoveCirclular = true;
	[SerializeField] private Vector3 mGridMoveDirection = new Vector3(5, 1, 0);

	//For switching between formation shapes ~Adam
	[SerializeField] private bool mUseMultipleFormations = false;
	[SerializeField] private float mFormationSwitchTimeDefault = 10f;
	[SerializeField] private float mFormationSwitchTime;
	[SerializeField] private bool mDelayFirstFormationSwitch = false;
	[SerializeField] private bool mLimitedFormationSwitches = false;
	[SerializeField] private int mMaxFormationSwitches = 100;
	[SerializeField] private int mFormationSwitchCount = 0;
	[SerializeField] private int mFormationUsed = -1;
	[SerializeField] private SwarmGrid[] mAlternateFormations;

	// Use this for initialization
	void Start () 
	{
		if (!mOverrideFocusPoint)
		{
			mFocusPoint = transform.position;
		}
		if(!mDelayFirstFormationSwitch)
		{
			mFormationSwitchTime = mFormationSwitchTimeDefault;
		}
	}//END of Start()
	
	// Update is called once per frame
	void Update () 
	{
		//Moving Up and Down ~Adam
		if(mMoveVeritcal)
		{
			if (transform.position.y >= mFocusPoint.y+mVerticalMoveDist)
			{
				mMovingUp = false;
			}
			else if (transform.position.y <= mFocusPoint.y-mVerticalMoveDist)
			{
				mMovingUp = true;
			}
			if(mMovingUp)
			{
				transform.position += Vector3.up*mGridMoveSpeed* Time.deltaTime;
			}
			else
			{
				transform.position += Vector3.up*mGridMoveSpeed*-1f* Time.deltaTime;
			}
		}

		//Moving Left and Right ~Adam
		if(mMoveHorizontal)
		{
			if (transform.position.x >= mFocusPoint.x+mHorizontaMoveDist)
			{
				mMovingRight = false;
			}
			else if (transform.position.x <= mFocusPoint.x-mHorizontaMoveDist)
			{
				mMovingRight = true;
			}
			if(mMovingRight)
			{
				transform.position += Vector3.right*mGridMoveSpeed* Time.deltaTime;
			}
			else
			{
				transform.position += Vector3.right*mGridMoveSpeed*-1f* Time.deltaTime;
			}
		}

		//Moving around in a circle ~Adam
		if(mMoveCirclular)
		{
			mGridMoveDirection += mFocusPoint - transform.position;
			mGridMoveDirection.Normalize();
			mGridMoveDirection *= mGridMoveSpeed;
			
			transform.position += (mGridMoveDirection * Time.deltaTime);
		}

		//Swap between formation shapes on a timer by referencing inactive SwarmGrids in the scene ~Adam
		//These other SwarmGrids are set in-editor rather than at runtime ~Adam
		if(mUseMultipleFormations && mFormationUsed >= 0)
		{
			if(!mLimitedFormationSwitches || mMaxFormationSwitches > mFormationSwitchCount)
			{
				mFormationSwitchTime -= Time.deltaTime;
				if (mFormationSwitchTime <= 0)
				{
					mFormationUsed++;
					mFormationSwitchCount++;

					if(mFormationUsed >= mAlternateFormations.Length)
					{
						mFormationUsed = 0;
					}
					mFormationSwitchTime = mFormationSwitchTimeDefault;
				}
				if(mAlternateFormations.Length > 0 && mAlternateFormations[mFormationUsed] !=null)
				{
					ChangeFormation(mFormationUsed);
				}
			}
		}

	}//END of Update()


	//Finding the next empty position in the grid to assign it to a newly spawned enemy ~Adam
	public GameObject GetGridPosition(EnemyShipAI searchingShip)
	{
		//Set to -1 to show that we haven't found a GridSlot yet ~Adam
		int gridPosition = -1;

		//Loop through mGridSlots until you find an empty one ~Adam
		for (int i = 0; i < mGridSlots.Length; i++)
		{
			//if we find an open GridSlot, set is occupied status to true and make that the gridPosition that we return ~Adam
			if (mGridSlots[i] != null && !mGridSlots[i].mOccupied)
			{
				gridPosition = i;
				mGridSlots[i].SetOccupier(searchingShip);
				break;
			}
		}

		//Return a gridPosition if there was a slot open ~Adam
		if (gridPosition >= 0)
		{
			return mGridSlots[gridPosition].gameObject;
		}
		//Return null if there are no empty slots ~Adam
		else
		{
			return null;
		}

	}//END of GetGridPosition()

	//For checking if the swarm grid is full to avoid spawning excess enemies ~Adam
	public bool CheckIfSwarmFull()
	{
		bool swarmFull = true;
		//Loop through mGridSlots until you find an empty one ~Adam
		for (int i = 0; i < mGridSlots.Length; i++)
		{
			//if we find an open GridSlot, set swarmFull to false because the grid isn't full ~Adam
			if (mGridSlots[i] != null && !mGridSlots[i].mOccupied)
			{
				swarmFull = false;
				break;
			}
		}
		return swarmFull;
	}

	//For Changing the shape of the Swarm's formation ~Adam
	//Works best when the reference formation has the samw number of SwarmGridSlots as this formation ~Adam
	public void ChangeFormation(int formationNumber)
	{
		for (int i = 0; i < mAlternateFormations[formationNumber].mGridSlots.Length; i++)
		{
			if (mGridSlots[i] != null && mAlternateFormations[formationNumber].mGridSlots[i] != null)
			{
				mGridSlots[i].mFormationPosition = mAlternateFormations[formationNumber].mGridSlots[i].transform;
			}
		}

	}//END of ChangeFormation()


	//For accessing what formation number is currently being used ~Adam
	public int GetFormationNumber()
	{
		return mFormationUsed;
	}//END of GetFormationNumber()

	//Check if this swarm has no enemies assigned to it.  This way we can trigger events on particular swarms in the scene being cleared out rather than just checking if ALL enemies are gone ~Adam
	public bool CheckIfSwarmEmpty()
	{
		bool swarmEmpty = true;
		foreach (SwarmGridSlot slot in mGridSlots)
		{
			if(slot.mOccupied)
			{
				swarmEmpty = false;
			}
		}
		return swarmEmpty;
	}//END of CheckIfSwarmEmpty()
}
