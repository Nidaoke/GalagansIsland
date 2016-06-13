using UnityEngine;
using System.Collections;

//This script is attached to the bullet prefabs that get instantiated by the EnemyShipAI ~Adam
//It allows for multiple, editable projectile prefabs with unique behaviors and properties that all use a single base script ~Adam

public class EnemyBulletController : MonoBehaviour 
{
	
	public GameObject mPlayer = null;
	public GameObject mPlayerClone = null; //For co-op mode ~Adam
	public float mBulletSpeed = 20.0f;
	protected float mSelfDestructTimer = 5.0f;
	private ScoreManager mScoreController;
	public bool mShootable;
	public bool mAimAtPlayer = false;
	public bool mFixedFireDir = false;
	public bool mMoveTowardsPlayer = false;
	public Vector3 mFireDir;

	public Vector2 bulletForce;

	public GameObject bulletExplosion;

	public bool mDestroyedByBombs = true;

	//For handling slow-mo ~Adam
	SlowTimeController mSlowTimeController = null; 
	bool mIsSlow = false;
	Vector3 mStandardVelocity = Vector3.zero;

	public void Start()
	{
		mScoreController = FindObjectOfType<ScoreManager>();
		if(mScoreController.mPlayerAvatar != null)
		{
			mPlayer = mScoreController.mPlayerAvatar;
		}
		else
		{
			mPlayer = FindObjectOfType<PlayerShipController>().gameObject;
		}
		#region Keep a reference to Player 2 in Co-op Mode ~Adam
		if(mScoreController.mPlayer2Avatar != null)
		{
			mPlayerClone = mScoreController.mPlayer2Avatar;
		}
		#endregion
		//Vector2 bulletForce;

		//For slow-mo bullet dodging ~Adam
		if(FindObjectOfType<SlowTimeController>() != null)
		{
			mSlowTimeController = FindObjectOfType<SlowTimeController>();
		}

		//Used for firing in a particular pattern (i.e. rotational pattern on prototype boss horns)~Adam
		if (mFixedFireDir) 
		{
			bulletForce = mFireDir * mBulletSpeed;
		}
		//Used for aiming at the player when first instantiated ~Adam
		else if (mAimAtPlayer) 
		{
			Vector3 directionToPlayer = Vector3.down;
			#region Determine which player is closer in Co-Op mode ~Adam
			//Fire at the player 2 ship if it is both present and closer -Adam
			if (mPlayerClone != null && mPlayerClone.activeInHierarchy && (Vector3.Distance (transform.position, mPlayerClone.transform.position) <= Vector3.Distance (transform.position, mPlayer.transform.position) || !mPlayer.activeInHierarchy)) 
			{
				directionToPlayer = mPlayerClone.transform.position - transform.position;
				bulletForce = Vector3.Normalize (directionToPlayer) * mBulletSpeed;
				transform.LookAt (mPlayerClone.transform.position);
				transform.rotation = Quaternion.Euler (new Vector3 (90f, 0f, 0f) + transform.rotation.eulerAngles);
			}
			#endregion
			else 
			{
				//fire at the player ~Adam
				directionToPlayer = mPlayer.transform.position - transform.position;
				bulletForce = Vector3.Normalize (directionToPlayer) * mBulletSpeed;
				transform.LookAt (mPlayer.transform.position);
				transform.rotation = Quaternion.Euler (new Vector3 (90f, 0f, 0f) + transform.rotation.eulerAngles);
			}
		} 
		//For constantly tracking/chasing/homing in on the player after the bullet has been fired ~Adam
		else if (mMoveTowardsPlayer) 
		{
			Vector3 directionToPlayer = Vector3.down;
			#region Determine which player is closer in Co-Op mode ~Adam
			//Fire at the player 2 ship if it is both present and closer -Adam
			if (mPlayerClone != null && mPlayerClone.activeInHierarchy && (Vector3.Distance (transform.position, mPlayerClone.transform.position) <= Vector3.Distance (transform.position, mPlayer.transform.position) || !mPlayer.activeInHierarchy)) 
			{
				directionToPlayer = mPlayerClone.transform.position - transform.position;
				bulletForce = Vector3.Normalize (directionToPlayer) * mBulletSpeed;
				transform.LookAt (mPlayerClone.transform.position);
				transform.rotation = Quaternion.Euler (new Vector3 (90f, 0f, 0f) + transform.rotation.eulerAngles);
			}
			#endregion
			else 
			{
				//fire at the player ~Adam
				directionToPlayer = mPlayer.transform.position - transform.position;
				bulletForce = Vector3.Normalize (directionToPlayer) * mBulletSpeed;
				transform.LookAt (mPlayer.transform.position);
				transform.rotation = Quaternion.Euler (new Vector3 (90f, 0f, 0f) + transform.rotation.eulerAngles);
			}
		}
		//Just fire up and down ~Adam
		else
		{
			#region Determine which player is closer in Co-Op mode ~Adam
			if(mPlayerClone != null && mPlayerClone.activeInHierarchy && (Vector3.Distance(transform.position,mPlayerClone.transform.position) <= Vector3.Distance(transform.position,mPlayer.transform.position) || !mPlayer.activeInHierarchy))
			{
				//Fire at player 2 ~Adam
				if (mPlayerClone.transform.position.y > transform.position.y)
				{
					bulletForce = new Vector2(0.0f,mBulletSpeed);
					transform.RotateAround(transform.position,Vector3.forward,180f);
				}
				else
				{
					bulletForce = new Vector2(0.0f,mBulletSpeed * -1.0f);
				}
			}
			#endregion
			else
			{
				//Fire up/down at player 1 ~Adam
				if (mPlayer.transform.position.y > transform.position.y)
				{
					bulletForce = new Vector2(0.0f,mBulletSpeed);
					transform.RotateAround(transform.position,Vector3.forward,180f);
				}
				else
				{
					bulletForce = new Vector2(0.0f,mBulletSpeed * -1.0f);
				}

			}
		}

	
		GetComponent<Rigidbody> ().velocity = bulletForce;



		
	}//END of Start()
	
	public virtual void Update()
	{
		mSelfDestructTimer -= Time.deltaTime;

		#region For slow-mo bullet dodging ~Adam
		if(mSlowTimeController == null && FindObjectOfType<SlowTimeController>() != null)
		{
			mSlowTimeController = FindObjectOfType<SlowTimeController>();
		}
		#endregion

		//Actively chase the player if set to do so ~Adam
		if (mMoveTowardsPlayer) 
		{
			
			Vector3 directionToPlayer = Vector3.down;
			#region Capable of switching which target its chasing if in co-op mode ~Adam
			//Chase the player 2 ship if it is both present and closer -Adam
			if (mPlayerClone != null && mPlayerClone.activeInHierarchy && (Vector3.Distance (transform.position, mPlayerClone.transform.position) <= Vector3.Distance (transform.position, mPlayer.transform.position) || !mPlayer.activeInHierarchy)) 
			{
				directionToPlayer = mPlayerClone.transform.position - transform.position;
				bulletForce = Vector3.Normalize (directionToPlayer) * mBulletSpeed;
				transform.LookAt (mPlayerClone.transform.position);
				transform.rotation = Quaternion.Euler (new Vector3 (90f, 0f, 0f) + transform.rotation.eulerAngles);
			}
			#endregion
			else 
			{
				//Chase player 1
				if(mPlayer != null && mPlayer.activeInHierarchy)
				{
					directionToPlayer = mPlayer.transform.position - transform.position;
					bulletForce = Vector3.Normalize (directionToPlayer) * mBulletSpeed;
					transform.LookAt (mPlayer.transform.position);
					transform.rotation = Quaternion.Euler (new Vector3 (90f, 0f, 0f) + transform.rotation.eulerAngles);
				}
			}
			GetComponent<Rigidbody> ().velocity = bulletForce;

		}

		#region Slow down for slow-mo dodge ~Adam
		if(mSlowTimeController != null)
		{
			if(mSlowTimeController.mSlowTimeActive && !mIsSlow)
			{
				mStandardVelocity = GetComponent<Rigidbody>().velocity;
				GetComponent<Rigidbody> ().velocity *= mSlowTimeController.mSlowTimeScale;
				mIsSlow = true;
			}
			else if(!mSlowTimeController.mSlowTimeActive && mIsSlow)
			{
				GetComponent<Rigidbody>().velocity = mStandardVelocity;
				mIsSlow = false;
			}

		}
		#endregion

		#region Self-destruct after a certain amount of time
		if(mSelfDestructTimer<0.0f)
		{
			if(mMoveTowardsPlayer)
			{
				if(bulletExplosion != null)
				{
					
					Instantiate(bulletExplosion, transform.position, Quaternion.identity);
				}
			}

			Destroy(gameObject);
		}
		#endregion

		//Detect distance to player and slow down time if close but not quite hitting ~Adam
		if (Vector3.Distance(this.transform.position, mPlayer.transform.position) <= 2.85f && mPlayer.activeInHierarchy)
		{
			if(mSlowTimeController!= null)
			{
				mSlowTimeController.SlowDownTime(0.4f,1f);
			}
		}

		#region Detect distance to player and kill the player and destroy self if close enough to "touch" ~Adam
		//We use this instead of actual collision detection with the player do to issues that the Unity Engine has (or at least had when we started this project) with processing collisions with moving "isTrigger" colliders ~Adam
		if (Vector3.Distance(this.transform.position, mPlayer.transform.position) <= 1.5f && mPlayer.activeInHierarchy)
		{

			Debug.Log("The player was shot");
			if(mScoreController != null && mScoreController.enabled == true)
			{
				mScoreController.LoseALife();
				Destroy(gameObject);
			}
		}
		#endregion

		#region If double ship upgraded is activated, extend the slow time down ~ Jonathan
		//Also kill the ship FINALLY!!! ~ Jonathan
		if (mPlayer.GetComponent<PlayerShipController> ().mShipRecovered && mPlayer.activeInHierarchy) 
		{

			if(Vector3.Distance(this.transform.position, mPlayer.GetComponent<PlayerShipController> ().mSecondShip.transform.position) <= 2.85f){

				if(mSlowTimeController!= null)
				{
					mSlowTimeController.SlowDownTime(0.4f,1f);
				}
			}

			if(Vector3.Distance(this.transform.position, mPlayer.GetComponent<PlayerShipController> ().mSecondShip.transform.position) <= 1.5f){

				Debug.Log("The second ship was shot");
				mScoreController.LoseALife();
				Destroy(gameObject);
			}
		}
		#endregion

		#region Dection for player 2 ship on co-op mode ~Adam
		//Detect distance to player2 and slow down time if close but not quite hitting ~Adam
		if (mPlayerClone != null && Vector3.Distance(this.transform.position, mPlayerClone.transform.position) <= 2.5f && mPlayerClone.activeInHierarchy)
		{
			if(FindObjectOfType<SlowTimeController>()!= null)
			{
				FindObjectOfType<SlowTimeController>().SlowDownTime(0.4f,1f);
			}
		}

		//Detect distance to player clone and kill the clone and destroy self if close enough to "touch" ~Adam
		if(mPlayerClone != null && mPlayerClone.activeInHierarchy)
		{
			if (Vector3.Distance(this.transform.position, mPlayerClone.transform.position) <= 1.5f)
			{
				Debug.Log("The clone was shot");
				mScoreController.LosePlayerTwoLife();
				Destroy(gameObject);
			}
		}
		#endregion
	}//END of Update()

	void OnTriggerEnter (Collider other)
	{
		//Some types of enemy bullets can be destroyed by player bullets ~Adam
		if(other.tag == "Player Bullet")
		{
			if(mShootable)
			{
				//Some shootable bullets can make a lingering explosion effect that can hurt the player ~Adam
				if(bulletExplosion != null)
				{
					Instantiate(bulletExplosion, transform.position, Quaternion.identity);
				}

				Destroy(other.gameObject);
				Destroy(this.gameObject);
			}
		}

	}//END of OnTriggerEnter()



	void OnCollisionEnter (Collision other)
	{
		//For having certain types of enemy bullets kill other enemies as they chase the player ~Adam
		if (other.gameObject.tag == "Enemy" && mMoveTowardsPlayer && (other.gameObject.GetComponent<EnemyShipAI>()!= null && other.gameObject.GetComponent<EnemyShipAI>().mGrabber == false)) 
		{
			if (mPlayerClone != null && Vector3.Distance (transform.position, mPlayerClone.transform.position) <= Vector3.Distance (transform.position, mPlayer.transform.position)) 
			{
				other.gameObject.GetComponent<EnemyShipAI>().mKillerNumber = 2;
			}
			else
			{
				other.gameObject.GetComponent<EnemyShipAI>().mKillerNumber = 1;
			}


			other.gameObject.GetComponent<EnemyShipAI>().EnemyShipDie ();
		}
	}//END of OnCollisionEnter()



}