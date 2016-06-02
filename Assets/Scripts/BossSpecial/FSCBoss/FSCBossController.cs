using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FSCBossController : MonoBehaviour 
{
	
	[SerializeField] private FSCBossTeleporter mBossTeleporter;
	enum PortalBossBehaviorState { StartupSpawn, Firing, MinionAttacking, Spawning };
	[SerializeField] private PortalBossBehaviorState mBehaviorState;

	public int mBossHP = 300;

	//The particle effect fiels where we'll be centering the enemy spawners ~Adam
	[SerializeField] private GameObject mHornSpawnPartLeft;
	[SerializeField] private GameObject mHornSpawnPartRight;
	//The enemy spawners for the minions ~Adam
	[SerializeField] private EnemyShipSpawner mHornSpawnerLeft;
	[SerializeField] private EnemyShipSpawner mHornSpawnerRight;
	

	//The main particle beam laser ~Adam
	[SerializeField] private GameObject mMouthLaser;
	[SerializeField] private GameObject mMouthLaserHolder;

	[SerializeField] private SpriteRenderer mEyeGraphic;

	GameObject mPlayerShip;
	ScoreManager mScoreManager;
	[SerializeField] private GameObject mDeathExplosion;
	[SerializeField] private GameObject mFinalExplosion;

	[SerializeField] private Animator mAnimator;
	bool mFiring = false;
	float mEndGameTimer = 0f;

	LevelKillCounter mKillCounter;
	public Vector3 mLaserTargetRotation = Vector3.zero;
	public float mLaserRotateSpeed = 10f;



	Vector3 mTargetPlayerPosition;

	[SerializeField] private float mStartupSpawnTime = 5f;
	[SerializeField] private float mFireTime = 5f;
	[SerializeField] private float mMinionAttackTime = 5f;
	[SerializeField] private float mSpawnTime = 5f;

	// Use this for initialization
	void Start () 
	{
		mPlayerShip = FindObjectOfType<PlayerShipController> ().gameObject;
		mScoreManager = FindObjectOfType<ScoreManager>() as ScoreManager;
		mEyeGraphic.color = Color.clear;
		mKillCounter = FindObjectOfType<LevelKillCounter>();

		StartCoroutine(StartupSpawnTimer());

	}//END of Start()
	
	// Update is called once per frame
	void Update () 
	{
		//Make sure we always have a ScoreManager ~Adam
		if(mScoreManager == null)
		{
			mScoreManager = FindObjectOfType<ScoreManager>() as ScoreManager;
		}
		else
		{
			if(mScoreManager.mP1Score >= mScoreManager.mP2Score)
			{
				mPlayerShip = mScoreManager.mPlayerAvatar;
			}
			else if (mScoreManager.mPlayer2Avatar != null)
			{
				mPlayerShip = mScoreManager.mPlayer2Avatar;
			}
		}
		//Make sure we always have a Player Ship ~Adam
		if (mPlayerShip == null) 
		{
			mPlayerShip = FindObjectOfType<PlayerShipController> ().gameObject;
		}
		else
		{
			//If the boss is still alive, do stuff ~Adam
			if(mBossHP >0)
			{
				mAnimator.SetBool("Firing", mFiring);
				GetComponent<Collider>().isTrigger = !mFiring;

				mEyeGraphic.color = Color.Lerp(mEyeGraphic.color, Color.clear, 0.01f);
				GetComponent<SpriteRenderer>().color = Color.Lerp(GetComponent<SpriteRenderer>().color, Color.white, 0.01f);

				// 15-20 hitable, spawning, minions circling, go back to 0 when 20 is hit ~Adam
				if (mBehaviorState == PortalBossBehaviorState.Spawning)
				{
					#region Spawning minions ~Adam
					SpawnMinions();
					#endregion
					//Make all the minions constantly circle ~Adam
					MinionCircle();
				
				}
				// 10-15 hittable, minions attacking ~Adam
				else if (mBehaviorState == PortalBossBehaviorState.MinionAttacking)
				{
					//Spawn minions ~Adam
					SpawnMinions();
					//Make all the minions constantly attack ~Adam
					foreach(EnemyShipAI minion in FindObjectsOfType<EnemyShipAI>())
					{
						minion.mCurrentAIState = EnemyShipAI.AIState.Attacking;
						minion.gameObject.transform.up = transform.up;
					}

					//Stop firing the laser ~Adam
					mFiring = false;
					mMouthLaser.GetComponent<ParticleSystem>().Stop();
				}
				// 0-10 Firing, invincible minions circling
				else if (mBehaviorState == PortalBossBehaviorState.Firing)
				{
					#region Stop the spawning of minions ~Adam

					StopMinionSpawning();
					#endregion

					//Make all the minions constantly circle ~Adam
					MinionCircle ();

					//Fire the Laser ~Adam
					mFiring = true;
					GetComponent<Collider>().isTrigger = false;
					//mMouthLaser.transform.LookAt(mPlayerShip.transform.position);
					LaserSpin();

					if(!mMouthLaser.GetComponent<ParticleSystem>().isPlaying)
					{
						mMouthLaser.GetComponent<ParticleSystem>().Play();
					}

				}
				// <0 Spawnning, invincible ~Adam
				else if (mBehaviorState == PortalBossBehaviorState.StartupSpawn)
				{
					#region Spawning minions ~Adam
					SpawnMinions();
					#endregion
					//Close the eye and become invincible
					mFiring = true;
					//Make all the minions constantly circle ~Adam
					MinionCircle ();
					//Stop firing the laser ~Adam
					mMouthLaser.GetComponent<ParticleSystem>().Stop();
				}
			}
			//Take care of the boss dying ~Adam
			else
			{
				#region Stop the spawning of minions ~Adam
				//StopMinionSpawning();
				
				#endregion
				mHornSpawnerLeft.gameObject.SetActive(false);
				mHornSpawnerRight.gameObject.SetActive(false);
				//Kill the remaining minions ~Adam
				if(FindObjectsOfType<EnemyShipAI>().Length >0)
				{
					foreach(EnemyShipAI minion in FindObjectsOfType<EnemyShipAI>())
					{
						minion.EnemyShipDie();
					}
				}
				//Stop firing the laser ~Adam
				mFiring = false;
				mMouthLaser.GetComponent<ParticleSystem>().Stop();





				mEndGameTimer += Time.deltaTime;

				Instantiate(mDeathExplosion, transform.position+(new Vector3(Random.Range(-5f,5f),Random.Range(-5f,5f),-0.5f)), Quaternion.identity);
				
				if(mEndGameTimer >= 2.8f)
				{
					Instantiate(mDeathExplosion, transform.position+(new Vector3(0f,0f,-0.5f)), Quaternion.identity);

				}
				if(mEndGameTimer >= 6f)
				{
					mKillCounter.mKillCount = mKillCounter.mRequiredKills+1;
					mScoreManager.AdjustScore(200, true);
					Destroy(this.gameObject);

				}
			}
		}

	}//END of Update()


	void OnTriggerEnter(Collider other)
	{
		//Get hit by bullets
		if(other.gameObject.GetComponent<PlayerBulletController>() != null)
		{
			
			if(!other.gameObject.GetComponent<PlayerBulletController>().mSideBullet && !mFiring)
			{
				Destroy (other.gameObject);
				mBossHP--;
				mEyeGraphic.color = Color.Lerp(mEyeGraphic.color, Color.white, 0.9f);
				GetComponent<SpriteRenderer>().color = Color.Lerp(GetComponent<SpriteRenderer>().color, Color.red, 0.9f);

			}
			
		}
		//Kill Player when touched
		if(other.gameObject.GetComponent<PlayerOneShipController>() != null)
		{
			mScoreManager.LoseALife();
		}
		else if(other.gameObject.GetComponent<PlayerTwoShipController>() != null)
		{
			mScoreManager.LosePlayerTwoLife();
		}
	}//END of OnCollisionEnter()

	//For stoping the spawning of minions
	void StopMinionSpawning()
	{
		foreach(ParticleSystem portalPart in mHornSpawnPartLeft.GetComponentsInChildren<ParticleSystem>())
		{
			if(portalPart.isPlaying)
			{
				portalPart.Stop();
			}
		}
		foreach(ParticleSystem portalPart in mHornSpawnPartRight.GetComponentsInChildren<ParticleSystem>())
		{
			if(portalPart.isPlaying)
			{
				portalPart.Stop();
			}
		}
		mHornSpawnerLeft.enabled = false;
		mHornSpawnerRight.enabled = false;
	}//END of StopMinionSpawning()

	//For Spawning Minions()
	void SpawnMinions()
	{
		foreach(ParticleSystem portalPart in mHornSpawnPartLeft.GetComponentsInChildren<ParticleSystem>())
		{
			if(!portalPart.isPlaying)
			{
				portalPart.Play();
			}
		}
		foreach(ParticleSystem portalPart in mHornSpawnPartRight.GetComponentsInChildren<ParticleSystem>())
		{
			if(!portalPart.isPlaying)
			{
				portalPart.Play();
			}
		}
		mHornSpawnerLeft.enabled = true;
		mHornSpawnerRight.enabled = true;
		if(FindObjectsOfType<EnemyShipAI>().Length < 200)
		{
			mHornSpawnerLeft.mSpawnCounter = 0;
			mHornSpawnerRight.mSpawnCounter = 0;
		}
	}//END of SpawnMinions()

	//For forcing minions to circle
	void MinionCircle()
	{
		foreach(EnemyShipAI minion in FindObjectsOfType<EnemyShipAI>())
		{
			if(minion.mCurrentAIState == EnemyShipAI.AIState.Attacking || minion.mCurrentAIState == EnemyShipAI.AIState.Swarming)
			{
				minion.mCurrentAIState = EnemyShipAI.AIState.ApproachingSwarm;
			}
			if(minion.mCurrentAIState == EnemyShipAI.AIState.ApproachingSwarm && Vector3.Distance(minion.transform.position, minion.mLoopPoint.transform.position) <1.1f)
			{
				minion.transform.up = transform.up;
				minion.mVel = new Vector2(0, -1);
				minion.mCurrentAIState = EnemyShipAI.AIState.FlightLooping;
			}
			minion.mHasLooped = false;
		}
	}



	void LaserSpin()
	{
		//Spin on its own towards the player ~Adam
		if(mPlayerShip != null)
		{


			if(mScoreManager != null)
			{
				if(mScoreManager.mP1Score >= mScoreManager.mP2Score-2)
				{
					mTargetPlayerPosition = mScoreManager.mPlayerAvatar.transform.position;
				}
				else
				{
					mTargetPlayerPosition = mScoreManager.mPlayer2Avatar.transform.position;
				}
				mLaserTargetRotation = new Vector3(mTargetPlayerPosition.x-transform.position.x,mTargetPlayerPosition.y-transform.position.y,0f);
			}

			else
			{
				mTargetPlayerPosition = mPlayerShip.transform.position;
				mLaserTargetRotation = new Vector3(mTargetPlayerPosition.x-transform.position.x,mTargetPlayerPosition.y-transform.position.y,0f);

			}

			Vector3.Normalize (mLaserTargetRotation);
			mLaserTargetRotation = new Vector3(0f, 0f, Vector3.Angle(mLaserTargetRotation, Vector3.down));
			if(mTargetPlayerPosition.x < transform.position.x)
			{
				mLaserTargetRotation *= -1f;
			}

			mMouthLaserHolder.transform.rotation =Quaternion.Lerp (mMouthLaserHolder.transform.rotation, Quaternion.Euler (mLaserTargetRotation), 0.001f*mLaserRotateSpeed * Time.timeScale);
		}
		//Just spin without regards to the player
		else
		{

			mLaserTargetRotation = transform.rotation.eulerAngles + new Vector3(0f,0f,mLaserRotateSpeed);

			mMouthLaserHolder.transform.rotation =Quaternion.Lerp (transform.rotation, Quaternion.Euler (mLaserTargetRotation), 0.5f * Time.timeScale);


		}
	}


	IEnumerator StartupSpawnTimer()
	{
		yield return new WaitForSeconds(mStartupSpawnTime);
		mBehaviorState = PortalBossBehaviorState.MinionAttacking;
		StartCoroutine(MinionAttackTimer());
	}

	IEnumerator MinionAttackTimer()
	{
		yield return new WaitForSeconds(mMinionAttackTime);
		mBehaviorState = PortalBossBehaviorState.Spawning;
		StartCoroutine(MinionSpawnTimer());
	}

	IEnumerator MinionSpawnTimer()
	{
		yield return new WaitForSeconds(mSpawnTime);
		if(!mKillCounter.mLevelComplete && mBossHP >0)
		{
			mBossTeleporter.StartTeleport();
		}

	}

	IEnumerator MouthLaserFireTimer()
	{
		mBehaviorState = PortalBossBehaviorState.Firing;
		yield return new WaitForSeconds(mFireTime);
		mBehaviorState = PortalBossBehaviorState.MinionAttacking;
		StartCoroutine(MinionAttackTimer());
	}

	public void StartMouthLaser()
	{
		StartCoroutine(MouthLaserFireTimer());
	}
}

