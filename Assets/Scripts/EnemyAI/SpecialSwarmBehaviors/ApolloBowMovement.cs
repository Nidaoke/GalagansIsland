using UnityEngine;
using System.Collections;

public class ApolloBowMovement : MonoBehaviour 
{
	//For moving around ~Adam
	public Vector3 mMoveTarget = new Vector3(0f,0f,-2f);
	public float mMoveSpeed = 15f;
	
	public float[] mBounds;

	//For rotating ~Adam
	public ScoreManager mScoreMan;
	public GameObject mPlayer1;
	public GameObject mPlayer2;
	public Vector3 mTargetRotation = Vector3.zero;
	public Vector3 newDir;
	public float mRotateSpeed = 10f;
	Vector3 mTargetPlayerPosition;
	
	//References to the swarm grids ~Adam
	public SwarmGrid mBowHaft;
	public SwarmGrid mBowString;

	public float mActionTimer = -10f;
	public float mArrowTime = 3f;
	public float mDrawTime = 5f;
	public float mFireTime = 15f;



	//For Firing Arrows ~Adam
	public GameObject mArrowPortal;
	public EnemyShipSpawner mArrowSpawner;
	public ApolloArrowMovement mArrowMover;

	//For the shield killer attached to the arrow ~Adam
	public GameObject mShieldKiller;

	// Use this for initialization
	void Start () 
	{
		mScoreMan = FindObjectOfType<ScoreManager>();

		if(mScoreMan !=null)
		{
			if(mScoreMan.mPlayerAvatar != null)
			{
				mPlayer1 = mScoreMan.mPlayerAvatar;
			}
			if(mScoreMan.mPlayer2Avatar != null)
			{
				mPlayer2 = mScoreMan.mPlayer2Avatar;
			}
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(mScoreMan == null)
		{
			mScoreMan = FindObjectOfType<ScoreManager>();

			if(mScoreMan !=null)
			{
				if(mScoreMan.mPlayerAvatar != null)
				{
					mPlayer1 = mScoreMan.mPlayerAvatar;
				}
				if(mScoreMan.mPlayer2Avatar != null)
				{
					mPlayer2 = mScoreMan.mPlayer2Avatar;
				}
			}
		}

		//Update the timer ~Adam
		mActionTimer += Time.deltaTime;

		//For doing the first draw early ~Adam
		if(mActionTimer > -0.5f && mActionTimer < 0f)
		{
			mActionTimer = mArrowTime;
			if(mShieldKiller != null && !mShieldKiller.activeInHierarchy)
			{
				mShieldKiller.SetActive (true);
			}
		}

		if(mActionTimer >= 0f)
		{
			//Rotate to aim at the player ~Adam
			BowRotation();

			//Move around the play field ~Adam
			BowMovement();


			if(mActionTimer < mArrowTime)
			{
				if(mArrowPortal != null && mArrowSpawner != null)
				{
					mArrowPortal.SetActive (false);
					mArrowSpawner.mSpawnCounter = 0;
					mArrowSpawner.mSpawning = true;
				}
			}
			else
			{
				if(mArrowPortal != null)
				{
					mArrowPortal.SetActive (true);
				}
				if(mArrowMover != null)
				{
					mArrowMover.mShooting = false;
				}
			}
			if(mActionTimer < mDrawTime)
			{
				if(mBowHaft != null && mBowString != null)
				{
					mBowHaft.ChangeFormation (0);
					mBowString.ChangeFormation (0);
				}
			}
			else
			{
				if(mBowHaft != null && mBowString != null)
				{
					mBowHaft.ChangeFormation (1);
					mBowString.ChangeFormation (1);
				}
			}
			if(mActionTimer > mFireTime)
			{
				if(mArrowMover != null)
				{
					mArrowMover.mShooting = true;
				}
				mActionTimer = 0f;
			}
		}

	}

	void BowRotation()
	{
		if(mScoreMan != null && (mPlayer1 != null || mPlayer2 != null) )
		{
			if(mPlayer1 != null && mScoreMan.mP1Score >= mScoreMan.mP2Score)
			{
				mTargetPlayerPosition = mPlayer1.transform.position;
				//mTargetRotation += Vector3.up*3f;
			}
			else if(mPlayer2 != null)
			{
				mTargetPlayerPosition = mPlayer2.transform.position;
				//mTargetRotation += Vector3.up*3f;
			}
			mTargetRotation = new Vector3(mTargetPlayerPosition.x-transform.position.x,mTargetPlayerPosition.y-transform.position.y,0f);
			
			Vector3.Normalize (mTargetRotation);
			mTargetRotation = new Vector3(0f, 0f, Vector3.Angle(mTargetRotation, Vector3.down));
			if(mTargetPlayerPosition.x < transform.position.x)
			{
				mTargetRotation *= -1f;
			}
			
			transform.rotation =Quaternion.Lerp (transform.rotation, Quaternion.Euler (mTargetRotation), 0.001f*mRotateSpeed * Time.timeScale);
		}
	}//END of BowRotation()

	void BowMovement()
	{
		transform.position = Vector3.Lerp(transform.position, mMoveTarget, mMoveSpeed*0.001f * Time.timeScale);
		
		if(Vector3.Distance (transform.position, mMoveTarget) < 7f && mBounds.Length >= 4)
		{
			mMoveTarget = new Vector3(Random.Range (mBounds[0],mBounds[1]), Random.Range (mBounds[2],mBounds[3]),-2f);
		}
	}//END of BowMovement()
}
