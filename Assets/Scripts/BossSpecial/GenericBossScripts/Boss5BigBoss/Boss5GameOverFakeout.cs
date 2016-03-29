using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Boss5GameOverFakeout : MonoBehaviour 
{
	[SerializeField] private AudioClip mBossMusic;
	[SerializeField] private GameObject mBoss;
	[SerializeField] private GameObject mGetReadyObject;
	[SerializeField] private GameObject mPlayerRevealParticle;
	ScoreManager mScoreManager;
	bool mBossUp = false;
	bool mEnemiesCleared = false;
	float mLevelTimer = 0f;
	[SerializeField] private float mPlayerEntranceDelay = 5f; 
	
	[SerializeField] private float mBossEntranceDelay = 20f; 
	float mBossEntranceTimer = 0f;
	
	bool mHidingPlayer = true;

	[SerializeField] private GameObject mScreenFader;

	
	//For muting the usual sounds during the boss fight to emphasize the rap music and boss effect ~Ada
	public GameObject mMutedPlayerBullet;
	public GameObject mMutedPlayerDeath;
	
	// Use this for initialization
	void Start () 
	{
		mScoreManager = FindObjectOfType<ScoreManager>() as ScoreManager;

		mScreenFader.GetComponent<Renderer>().material.color = new Color(0,0,0,0);

	}//END of Start()
	
	// Update is called once per frame
	void Update () 
	{
		//Increase the timer
		mLevelTimer += Time.deltaTime;
		
		
		
		
		//Make sure we always have a ScoreManager ~Adam
		if(mScoreManager == null)
		{
			mScoreManager = FindObjectOfType<ScoreManager>() as ScoreManager;
		}

		

		
		//Start the boss fight if enough time has passed or the skull swarm is gone `Adam
		if(!mEnemiesCleared)
		{
			EnemyShipAI remainingEnemey = FindObjectOfType<EnemyShipAI>();
			
			if( (remainingEnemey == null && mLevelTimer > 5f) || (mLevelTimer > 25f+mPlayerEntranceDelay) )
			{
				ClearEnemies();
			}
		}
		else if(!mBossUp)
		{
			mBossEntranceTimer += Time.deltaTime;
			
			if(mBossEntranceTimer >= mBossEntranceDelay)
			{
				StartBoss();
			}
		}
		
		//Hide the player ship and UI at the start ~Adam
		if(mHidingPlayer)
		{
			//Hiding UI ~Adam
			if(GameObject.Find("Game_HUD").GetComponent<Canvas>() != null){

				GameObject.Find("Game_HUD").GetComponent<Canvas>().enabled = false;
			}
		
			FindObjectOfType<PauseManager>().enabled = false;
			
			//Hiding the player ~Adam
			if(FindObjectOfType<PlayerOneShipController>() != null)
			{
				GameObject playerShip = FindObjectOfType<PlayerOneShipController>().gameObject;
				
				playerShip.GetComponent<PlayerShipController>().enabled = false;
				playerShip.GetComponent<Collider>().enabled = false;
				
				foreach(ParticleSystem shipParticles in playerShip.GetComponentsInChildren<ParticleSystem>() )
				{
					shipParticles.enableEmission = false;
				}
				foreach(SpriteRenderer shipSprite in playerShip.GetComponentsInChildren<SpriteRenderer>() )
				{
					shipSprite.enabled = false;
				}
			}
			
			//Hiding player 2 ~Adam
			if(FindObjectOfType<PlayerTwoShipController>() != null)
			{
				GameObject player2Ship = FindObjectOfType<PlayerTwoShipController>().gameObject;
				
				player2Ship.GetComponent<PlayerTwoShipController>().enabled = false;
				player2Ship.GetComponent<Collider>().enabled = false;
				
				foreach(ParticleSystem shipParticles in player2Ship.GetComponentsInChildren<ParticleSystem>() )
				{
					shipParticles.enableEmission = false;
				}
				foreach(SpriteRenderer shipSprite in player2Ship.GetComponentsInChildren<SpriteRenderer>() )
				{
					shipSprite.enabled = false;
				}
			}
			
			if(mLevelTimer > mPlayerEntranceDelay)
			{
				RevealPlayer();
			}
			
			
		}
		
	}//END of Update()
	
	void ClearEnemies()
	{
		//Kill any remaining enemies ~Adam
		EnemyShipAI[] skullShips = FindObjectsOfType<EnemyShipAI>();
		
		foreach(EnemyShipAI startShip in skullShips)
		{
			startShip.EnemyShipDie();
		}
		
		//Change BGM ~Adam
		Camera.main.GetComponentInChildren<AudioSource>().clip = mBossMusic;
		Camera.main.GetComponentInChildren<AudioSource>().Play();
		
//		//Mute the usual player sound effects ~Adam
//		FindObjectOfType<PlayerShipController>().mBulletPrefab = mMutedPlayerBullet;
//		FindObjectOfType<PlayerShipController>().GetComponent<AudioSource>().enabled = false;
//		mScoreManager.mPlayerDeathEffect = mMutedPlayerDeath;
		
		mEnemiesCleared = true;
	}//EndOfClearEnemies
	
	void StartBoss()
	{
		

		
		//Start the boss behavior ~Adam
		mBoss.SetActive(true);
		mGetReadyObject.SetActive (true);
		mBossUp = true;
		
		
		//GetComponent<CreditsSpawner>().mRollingCredits = true;
	}//END of StartBoss()
	
	void RevealPlayer()
	{
		mHidingPlayer = false;
		//Revealing the UI ~Adam
		FindObjectOfType<PauseManager>().enabled = true;
		GameObject.Find("Game_HUD").GetComponent<Canvas>().enabled = true;
		
		//Revealing the Player 1 ~Adam
		if(FindObjectOfType<PlayerOneShipController>() != null)
		{
			GameObject playerShip = FindObjectOfType<PlayerOneShipController>().gameObject;
			playerShip.GetComponent<PlayerShipController>().enabled = true;
			playerShip.GetComponent<Collider>().enabled = true;
			
			foreach(ParticleSystem shipParticles in playerShip.GetComponentsInChildren<ParticleSystem>() )
			{
				shipParticles.enableEmission = true;
			}
			foreach(SpriteRenderer shipSprite in playerShip.GetComponentsInChildren<SpriteRenderer>() )
			{
				shipSprite.enabled = true;
			}
			//Put a particle effect on the player ship as it appears ~Adam
			Instantiate(mPlayerRevealParticle, playerShip.transform.position, Quaternion.identity);
		}
		//Revealing the Player 2 ~Adam
		if(FindObjectOfType<PlayerTwoShipController>() != null)
		{
			GameObject playerTwoShip = FindObjectOfType<PlayerTwoShipController>().gameObject;
			playerTwoShip.GetComponent<PlayerTwoShipController>().enabled = true;
			playerTwoShip.GetComponent<Collider>().enabled = true;
			
			foreach(ParticleSystem shipParticles in playerTwoShip.GetComponentsInChildren<ParticleSystem>() )
			{
				shipParticles.enableEmission = true;
			}
			foreach(SpriteRenderer shipSprite in playerTwoShip.GetComponentsInChildren<SpriteRenderer>() )
			{
				shipSprite.enabled = true;
			}
			//Put a particle effect on the player ship as it appears ~Adam
			Instantiate(mPlayerRevealParticle, playerTwoShip.transform.position, Quaternion.identity);
		}
	}//END of RevealPlayer()
}
