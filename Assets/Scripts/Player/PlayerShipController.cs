using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using InControl;
using Assets.Scripts.Achievements;
using XInputDotNetPure; // Required in C#

//This is the main script for controlling the player character. ~Adam
//It makes use of the open source version of the InControl Unity plugin for taking game pade input.  This plugin may be found at: "https://github.com/pbhogan/InControl" ~Adam


public class PlayerShipController : MonoBehaviour 
{
	public bool isHovering;

	public bool isOnBottomY; //Just to check if on the bottom of the screen. If this takes up too much space, just remove this and where the floatiness is, just replace with the barrier statements. ~Jonathan

	public bool secondShipOnHip = true;

	//For multiplayer co-op ~Adam
	public PlayerOneShipController mPlayerOne;
	public PlayerTwoShipController mPlayerTwo;

	//For multiplaye co-op ~Adam
	public InputDevice mPlayerInputDevice;
	public string mPlayerInputMeta = "";

	public GameObject mDamageParticles;//Particles that play when the player gets hit ~Adam
	protected bool playerIndexSet = false;
	public PlayerIndex playerIndex;

	public bool cheats = false;
	//For if animating the ship ~Adam
	[SerializeField] protected Animator mMainShipAnimator;
	[SerializeField] protected Animator mSecondShipAnimator;
	
	public float bulletShootSpeed = .3f;
	
	//For firing bullets on a set time interval ~Adam
	protected float mBulletFireTime = 0f;
	//The prefab we're using for the player bullets ~Adam
	public GameObject mBulletPrefab = null;
	
	public float mBaseMovementSpeed = 13f;
	public float mMovementSpeed = 0f;
	
	public Vector3 mMoveDir = new Vector3(0f,-1f,0f);
	

	//Variables for editing drop speeds ~Adam
	[SerializeField] protected float mMaxDropSpeed = 0.58f;
	[SerializeField] protected float mDropSpeed = 0.1f;
	[SerializeField] protected float mDropAccelRate = 0.1f;
	[SerializeField] protected float mDropDeccelRate = 0.1f;
	
	//Used for the duplicating of the ship via Grabber Enemies ~Adam
	public bool mShipStolen = false;
	public bool mShipRecovered = false;
	
	//The game objects that are our ship sprites ~Adam
	public GameObject mMainShip;
	public GameObject mSecondShip;
	//Where our bullets spawn from ~Adam
	//indexes:
		//0: Main ship, main bullet
		//1: Second ship, main bullet
		//2: Main ship, left bullet
		//3: Main ship, right bullet
		//4: Second ship, left bullet
		//5: Second ship, right bullet
	[SerializeField] protected Transform[] mBulletSpawns;
	
	
	//For Overheating
	public bool overHeatProcess = true;
	public bool isOverheated = false;
	public float heatLevel = 0f;
	public float mBaseHeatMax = 45f;
	public float maxHeatLevel;

	//For when the player has 3 bullets  ~Adam
	public bool mThreeBullet = true;
	public float mThreeBulletTimer = 0f;
	public GameObject mSideBullet;

	//For when the ship has a shieldPowerUp ~Adam
	public bool mShielded = false;
	[SerializeField] protected  SpriteRenderer mMainShipShieldSprite;
	[SerializeField] protected  SpriteRenderer mSecondShipShieldSprite;
	public float mShieldTimer = 0f;

	
	//For deleting duplicate ships when we change levels ~Adam
	public int mShipCreationLevel;
	public bool mToggleFireOn = true;
	
	//For tracking where the ship was last frame so we can see how much/in what direction its moving ~Adam
	//public Vector3 mLastFramePosition;
	//public Vector3 mLastFrameDifference = Vector3.zero;
	//movprotected float mLastNonZeroHorizontalDifference;
	protected bool mDriftDown = true;
	protected float mInputVertical = 0f;
	protected float mInputHorizontal = 0f;

	//For spinning the ship around when the player gets hit ~Adam
	protected float mSpinning = 0f;
	[HideInInspector] public  float mSpinTimer = 0f;
	protected float mSpinTimerDefault = 0.5f;
	
	//For Super Screen-Wiper powerup ~Adam
	public bool mHaveLaserFist = false;
	public GameObject mLaserFist;
	public bool mHaveBigBlast = false;
	public GameObject mBigBlast;
	
	
	
	//For making the ship flash when hit ~Adam
	public GameObject mMainShipHitSprite;
	public GameObject mSecondShipHitSprite;


	//For altering movement and firing speed with damage and repair ~Adam
	public float mFireUpgrade = 1f;
	public float mMoveUpgrade = 1f;
	[SerializeField] protected ScoreManager mScoreMan;


	//For flipping the ship in co-op mode ~Adam
	public float mFlipTimer = 1f;
	public bool mFlipped = false;

    bool achievementFriendOfMine;

	//For the PauseManager being a component of the ScoreManager rather than of the ship ~Adam
	[SerializeField] protected PauseManager mPauseMan;


	//For toggling the ability to hover externally ~Adam
	protected bool mHoverDisabled;

	// Use this for initialization
	protected virtual void Start () 
	{
		//Make sure we always have a reference to the score manager and set the current life percentage ~Adam
		if(FindObjectOfType<ScoreManager>() != null)
		{
			mScoreMan = FindObjectOfType<ScoreManager>();
			mPauseMan = mScoreMan.gameObject.GetComponent<PauseManager>();
		}

		//Adjust speed and scale for mobile ~Adam
		if (Application.isMobilePlatform)
		{
			mBaseMovementSpeed = 15.0f;
			transform.localScale = new Vector3(1.5f,1.5f,1.5f);
		}
		
		mShipCreationLevel = Application.loadedLevel;
		
		PlayerShipController[] otherPlayerShips = FindObjectsOfType<PlayerShipController>();
		//Debug.Log(otherPlayerShip.name);
		foreach(PlayerShipController othership in otherPlayerShips)
		{
			if(othership.mShipCreationLevel < this.mShipCreationLevel)
			{
				Debug.Log("Found another ship so destroying self.");
				Destroy(this.gameObject);
			}
		}
		

	}//END of Start()
	
	
	//Persist between level loads/reloads ~Adam
	protected virtual void Awake()
	{
		DontDestroyOnLoad (transform.gameObject);
		InputManager.Setup();


	}//END of Awake()
	
	
	// Update is called once per frame
	protected virtual void Update () 
	{
		//Make sure we always have a reference to the score manager and keep the current life percentage up-to-date ~Adam
		if(mScoreMan == null)
		{
			if(FindObjectOfType<ScoreManager>() != null)
			{
				mScoreMan = FindObjectOfType<ScoreManager>();
				mPauseMan = mScoreMan.gameObject.GetComponent<PauseManager>();
			}
		}

		#region ACHIEVEMENTS by Mateusz
		if(AchievementManager.instance != null)
		{
	        if (mShipRecovered)
	        {
	            if (achievementFriendOfMine == false)
	            {
	                achievementFriendOfMine = true;
	                AchievementManager.instance.FriendOMine.StartTimer();
	            }
	        }
	        else
	        {
	            if (achievementFriendOfMine == true)
	            {
	                achievementFriendOfMine = false;
	                AchievementManager.instance.FriendOMine.StopTimer();
	            }
	        }
		}
		#endregion



		//for managing input devices on co-op mode ~Adam
		ManageInputDevice();


		GetComponent<AudioSource>().volume = 0.18f*(30f-16f)/30f;

		//Level skip for development and testing purposes ~Jonathan
		//cheats should always be set to "false" in the published build
		if (cheats) 
		{
			if(Input.GetKeyDown(KeyCode.Alpha1))
			{
				Application.LoadLevel(Application.loadedLevel + 1);
				mShipStolen = false;
				EnableHover();
			}
			
			if(Input.GetKeyDown(KeyCode.Alpha2))
			{
				Application.LoadLevel(Application.loadedLevel - 1);
				mShipStolen = false;
				EnableHover();
			}
		}
		
		//Spin the ships when hit ~Ada,
		if(mSpinning != 0f)
		{
			SpinControl();
		}
		

		//Toggle shield sprites ~Adam
		if(mShielded)
		{
			mMainShipShieldSprite.GetComponent<Animator>().SetInteger ("ShieldState", 1);
			mSecondShipShieldSprite.GetComponent<Animator>().SetInteger ("ShieldState", 1);
			
			mMainShipShieldSprite.enabled = true;
			mMainShipShieldSprite.GetComponent<Light>().enabled = true;
			if(mShipRecovered)
			{
				mSecondShipShieldSprite.enabled = true;
				mSecondShipShieldSprite.GetComponent<Light>().enabled = true;
			}
			else
			{
				mSecondShipShieldSprite.enabled = false;
			}
			//Decrease Shield time ~Adam
			mShieldTimer -= Time.deltaTime;
			if(mShieldTimer <= 0f)
			{
				mShielded = false;
			}
			if(mShieldTimer < 2f)
			{
				mMainShipShieldSprite.GetComponent<Animator>().SetInteger ("ShieldState", 0);
				mSecondShipShieldSprite.GetComponent<Animator>().SetInteger ("ShieldState", 0);
				
			}
			if(mShieldTimer < 5f)
			{
				mMainShipShieldSprite.GetComponent<Renderer>().material.color = Color.Lerp (mMainShipShieldSprite.GetComponent<Renderer>().material.color, Color.red,0.1f);
				mSecondShipShieldSprite.GetComponent<Renderer>().material.color = Color.Lerp (mSecondShipShieldSprite.GetComponent<Renderer>().material.color, Color.red,0.1f);
			}
			else
			{
				mMainShipShieldSprite.GetComponent<Renderer>().material.color = Color.white;
				mSecondShipShieldSprite.GetComponent<Renderer>().material.color = Color.white;
			}
			
		}
		else
		{
			mMainShipShieldSprite.enabled = false;
			mMainShipShieldSprite.GetComponent<Light>().enabled = false;
			mSecondShipShieldSprite.enabled = false;
			mSecondShipShieldSprite.GetComponent<Light>().enabled = false;
			
		}
		
		//We used to increase movement speed as we progress through levels. ~Adam
		//The code from that setup is being preserved via commenting in the event that our game design changes back to use that at a later day ~Adam
		if(Time.timeScale > 0f)
		{
			//mMovementSpeed = ( mBaseMovementSpeed + (6f/25f*(Application.loadedLevel)) ) /Time.timeScale;
			
			//mMovementSpeed = ( mBaseMovementSpeed + (0.24f +5.76f*(Application.loadedLevel-1)/(Application.levelCount-4) ))*(mMoveUpgrade) /Time.timeScale;

			//Make the movement speed constant ~Adam
			mMovementSpeed = ( mBaseMovementSpeed + (0.24f +5.76f*(18)/(25) ))*(mMoveUpgrade) /Time.timeScale;

			//Placing a min and max on move speed based on the min/max before adding in damage and repair ~Adam
			if(mMovementSpeed < mBaseMovementSpeed/2f)
			{
				mMovementSpeed = mBaseMovementSpeed/2f;
			}
			if(mMovementSpeed > 19.24f)
			{
				mMovementSpeed = ( 13f + (0.24f +5.76f*(25f)/(24f) ));
			}
		}
		//Avoid dividing by 0 and don't move if the time scale is less than/equal to zero ~Adam
		else
		{
		//	mMovementSpeed = ( mBaseMovementSpeed + (6f/25f*(Application.loadedLevel)) );
			mMovementSpeed = 0f;
		}

		//Make the player drift toward the bottom of the screen
		DriftDown();

		//Default keyboard/gamepad stick input ~Adam
		TakeDirectionalInput();
		//END of Movement Control Input


		//Toggle bullet firing ~Adam
		TakeFiringInput();




		//Recolor the ship sprite when overheating ~Adam
		if (isOverheated) 
		{
			mToggleFireOn = false;
			mMainShip.GetComponent<Renderer>().material.color = Color.Lerp(mMainShip.GetComponent<Renderer>().material.color,Color.red,0.05f);
			mSecondShip.GetComponent<Renderer>().material.color = Color.Lerp(mSecondShip.GetComponent<Renderer>().material.color,Color.red,0.05f);
		}
		else if (heatLevel/maxHeatLevel > 0.9f) 
		{
			mMainShip.GetComponent<Renderer>().material.color = Color.Lerp(mMainShip.GetComponent<Renderer>().material.color,Color.yellow,0.1f);
			mSecondShip.GetComponent<Renderer>().material.color = Color.Lerp(mSecondShip.GetComponent<Renderer>().material.color,Color.yellow,0.1f);
		}
		
		else
		{
			mMainShip.GetComponent<Renderer>().material.color = Color.Lerp(mMainShip.GetComponent<Renderer>().material.color,Color.white,0.1f);
			mSecondShip.GetComponent<Renderer>().material.color = Color.Lerp(mSecondShip.GetComponent<Renderer>().material.color,Color.white,0.1f);
		}
		
		//firing bullets while the fire button is held
		if (mToggleFireOn) 
		{
			//Increase the overheat level while firing ~Adam
			if(!isOverheated)
			{
				if(heatLevel < maxHeatLevel)
				{
					if(Time.timeScale != 0f)
					{
						heatLevel += Time.deltaTime/Time.timeScale;
					}
				}
				//Lock the gun down when the heat levels get too high ~Adam
				if(heatLevel >= maxHeatLevel)
				{
					
					heatLevel = maxHeatLevel;
					isOverheated = true;
					if(AchievementManager.instance != null)
					{
						AchievementManager.instance.numberOfOverheats++;
					}

				}
				
				//Firing Bullets
				if (Time.time > mBulletFireTime) 
				{
					
					// Make the bullet object
					GameObject newBullet = Instantiate (mBulletPrefab, mBulletSpawns[0].position, mMainShip.transform.rotation * Quaternion.Euler (0f,0f,Random.Range(-3.0f,3.0f))) as GameObject;
					SetBulletNumber (newBullet.GetComponent<PlayerBulletController>());

					//Fire extra bullets if we have the spread-fire powerup ~Adam
					if (mThreeBullet) 
					{

						if(!mShipRecovered)
						{
							GameObject tripBullet1 = Instantiate (mSideBullet, mBulletSpawns[2].position, mMainShip.transform.rotation * Quaternion.Euler (0f, 0f, 8f) * Quaternion.Euler (0f,0f,Random.Range(-4.0f,4.0f))) as GameObject;
							SetBulletNumber (tripBullet1.GetComponent<PlayerBulletController>());
						}
						//Adjust triple-bullet firing when you have the double/side ship ~Adam
						else if(mShipRecovered)
						{
							GameObject tripBullet1 = Instantiate (mSideBullet, mBulletSpawns[2].position, mMainShip.transform.rotation * Quaternion.Euler (0f, 0f, 5f) * Quaternion.Euler (0f,0f,Random.Range(-8.0f,3.0f))) as GameObject;
							SetBulletNumber (tripBullet1.GetComponent<PlayerBulletController>());
						}
						GameObject tripBullet2 = Instantiate (mSideBullet, mBulletSpawns[3].position, mMainShip.transform.rotation * Quaternion.Euler (0f, 0f, -8f) * Quaternion.Euler (0f,0f,Random.Range(-4.0f,4.0f))) as GameObject;
						SetBulletNumber (tripBullet2.GetComponent<PlayerBulletController>());

					}
					//Play bullet-firing sound effect ~Adam
					GetComponent<AudioSource> ().Play ();
			
					//Do side ship bullets if the player has double ships from destroying a hostile copy of themselves ~Ada
					if (mShipRecovered)
					{


						GameObject secondBullet;
						secondBullet = Instantiate (mBulletPrefab, mBulletSpawns[1].position, mSecondShip.transform.rotation * Quaternion.Euler (0f,0f,Random.Range(-3.0f,3.0f))) as GameObject;
						secondBullet.name = "SECONDBULLET";
						SetBulletNumber (secondBullet.GetComponent<PlayerBulletController>());
						if (mThreeBullet) 
						{
							if(secondShipOnHip)
							{

								GameObject tripBullet3 = Instantiate (mSideBullet, mBulletSpawns[5].position, mSecondShip.transform.rotation * Quaternion.Euler (0f, 0f, 8f) * Quaternion.Euler (0f,0f,Random.Range(-4.0f,4.0f))) as GameObject;
								SetBulletNumber (tripBullet3.GetComponent<PlayerBulletController>());

								GameObject tripBullet4 = Instantiate (mSideBullet, mBulletSpawns[4].position, mSecondShip.transform.rotation * Quaternion.Euler (0f, 0f, -5f) * Quaternion.Euler (0f,0f,Random.Range(-3.0f,8.0f))) as GameObject;
								SetBulletNumber (tripBullet4.GetComponent<PlayerBulletController>());
							}
							else
							{

								GameObject tripBullet3 = Instantiate (mSideBullet, mBulletSpawns[4].position, mSecondShip.transform.rotation * Quaternion.Euler (0f, 0f, 8f) * Quaternion.Euler (0f,0f,Random.Range(-4.0f,4.0f))) as GameObject;
								SetBulletNumber (tripBullet3.GetComponent<PlayerBulletController>());

								GameObject tripBullet4 = Instantiate (mSideBullet, mBulletSpawns[5].position, mSecondShip.transform.rotation * Quaternion.Euler (0f, 0f, -5f) * Quaternion.Euler (0f,0f,Random.Range(-3.0f,8.0f))) as GameObject;
								SetBulletNumber (tripBullet4.GetComponent<PlayerBulletController>());
							}
						}
					}
					//Reset the timer to fire bullets. ~Adam
					//Originally, the later the level, the smaller the time between shots ~Adam
					//It now instead is based on the mFireUpgrade variable which fluctuates as the ship is damaged and gets repaired ~Adam
					if(mSpinning == 0)
					{
						//Scale firing rate based on what level we're on ~Adam
						if(Application.loadedLevelName != "Credits")
						{
							float bulletFireMod = 0.04f;
							if(mFireUpgrade <1f)
							{
//								bulletFireMod = ( (0.01f +.24f*(Application.loadedLevel-1)/(Application.levelCount-4)) *(mFireUpgrade*mFireUpgrade) );
								//Make fire rate constant ~Adam
								bulletFireMod = ( (0.01f +.24f*(17)/(25)) *(mFireUpgrade*mFireUpgrade) );
							}
							else
							{
//								bulletFireMod = ( (0.01f +.24f*(Application.loadedLevel-1)/(Application.levelCount-4)) *(mFireUpgrade) );
								//Make fire rate constant ~Adam
								bulletFireMod = ( (0.01f +.24f*(17)/(25)) *(mFireUpgrade*mFireUpgrade) );
							}
							//Add in a min/max bullet firing time based on paramaters from before when we added in damage/repair ~Adam
							//Fastest possible firing ~Adam
							if(bulletFireMod > 0.26f)
							{
								bulletFireMod = 0.26f;
							}
							//Slowest Possible firing ~Adam
							if(bulletFireMod < -0.1f)
							{
								bulletFireMod = -0.1f;
							}

							mBulletFireTime = Time.time + bulletShootSpeed - bulletFireMod;

						}
						else
						{
							mBulletFireTime = Time.time + (bulletShootSpeed - ( (0.01f +.24f*(17)/(25)) ) );
						}
					}
					//Fire faster while spinning ~Adam
					else
					{
						mBulletFireTime = Time.time + (bulletShootSpeed - 0.25f)/3f;
					}
				}
			}
		}
		//When not firing, lower the overheat amount ~Adam
		else if(heatLevel > 0 &&Time.timeScale != 0)
		{
			heatLevel -= Time.deltaTime * maxHeatLevel/5f/Time.timeScale;
		}

		//Keep from overheating during credits ~Adam
		if(Application.loadedLevelName == "Credits")
		{
			heatLevel = 0;
		}

		if (heatLevel <= 0f) 
		{
			
			isOverheated = false;
			if (Application.isMobilePlatform) //Start shooting when weapons are Cool. Lol, weapons are always cool.
			{
				mToggleFireOn = true;
			}
		}

		//Thruster control for hovering ~Adam
		TakeThrusterInput();

		//Move the ship by the mMoveDir vector if not paused
		if(Time.timeScale != 0)
		{
			if (Application.isMobilePlatform)
			{
				if(mMoveDir.y < 0f && !(mInputVertical == 0.0f && !Input.GetMouseButton(0)))
				{
					mMoveDir = Vector3.Lerp(mMoveDir, mMoveDir+ new Vector3(0f,-mDropSpeed,0f), 0.08f);
				}
				transform.Translate(mMoveDir);
				
				if (mInputVertical == 0.0f && !Input.GetMouseButton(0))
				{
					mDriftDown = true;
					mMoveDir = Vector3.Lerp(mMoveDir, new Vector3(mMoveDir.x,-mDropSpeed,mMoveDir.z), 0.9f);
					mMoveDir = Vector3.Lerp(mMoveDir, new Vector3(0f,-mDropSpeed,0f), 0.03f);
					
				}
			}
			else
			{
				if(mMoveDir.y < 0f && !(mInputVertical == 0.0f))
				{
					mMoveDir = Vector3.Lerp(mMoveDir, mMoveDir+ new Vector3(0f,-mDropSpeed,0f), 0.08f);
				}
				transform.Translate(mMoveDir);
				
				if (mInputVertical == 0.0f)
				{
					mDriftDown = true;
					mMoveDir = Vector3.Lerp(mMoveDir, new Vector3(mMoveDir.x,-mDropSpeed,mMoveDir.z), 0.9f);
					mMoveDir = Vector3.Lerp(mMoveDir, new Vector3(0f,-mDropSpeed,0f), 0.03f);
					
				}
				if (mInputHorizontal == 0.0f)
				{

					if(isHovering || isOnBottomY){

						mMoveDir = Vector3.Lerp(mMoveDir, new Vector3(0f,mMoveDir.y,0f), 0.4f);
					}else{

						mMoveDir = Vector3.Lerp(mMoveDir, new Vector3(0f,mMoveDir.y,0f), 0.1f);
					}
				}
			}
		}

		//Set ship animations ~Adam
		mMainShipAnimator.speed = 16/(Application.levelCount-3)*5f +1f;//Application.loadedLevel/5f+1f;
		mSecondShipAnimator.speed = 16/(Application.levelCount-3)*5f +1f;//Application.loadedLevel/5f+1f;
		if(mToggleFireOn)
		{
			mMainShipAnimator.SetBool("IsFiring", true);
			mSecondShipAnimator.SetBool("IsFiring", true);
		}
		else
		{
			mMainShipAnimator.SetBool("IsFiring", false);
			mSecondShipAnimator.SetBool("IsFiring", false);
		}
		



		//Control whether or not to render the second ship on the side ~Adam
		if (mShipRecovered)
		{
			mSecondShip.GetComponent<SpriteRenderer>().enabled = true;
			foreach (ParticleSystem shipTrail in mSecondShip.GetComponentsInChildren<ParticleSystem>())
			{
				if(!(mMoveDir.y < 0f && mDriftDown))
				{
					if(shipTrail.gameObject != mDamageParticles)
					{
						if(secondShipOnHip)
						{
							shipTrail.enableEmission = true;
						}
						else
						{
							shipTrail.enableEmission = false;
						}
					}
				}
			}
		}
	
		else
		{
			mSecondShip.GetComponent<SpriteRenderer>().enabled = false;
			foreach (ParticleSystem shipTrail in mSecondShip.GetComponentsInChildren<ParticleSystem>())
			{
				if(shipTrail.gameObject != mDamageParticles)
				{
					shipTrail.enableEmission = false;
				}
			}
		}

		//Flip the side ship between facing up and facing down
		if (Input.GetKeyDown (KeyCode.V) || mPlayerInputDevice.LeftTrigger.WasPressed) 
		{
			

			if (mShipRecovered) 
			{
				secondShipOnHip = !secondShipOnHip;

				if(secondShipOnHip)
				{
					
					mSecondShip.transform.localPosition = new Vector3(-3.5f, -0.1f,-0.53f);
					mSecondShip.transform.localScale = new Vector3(8, 8, 8);
					mSecondShip.transform.rotation = Quaternion.identity;			
				}
				else
				{
					
					mSecondShip.transform.localPosition = new Vector3(0, -3.3f,-0.53f);
					mSecondShip.transform.localScale = new Vector3(-8, 8, 8);
					mSecondShip.transform.rotation = Quaternion.Euler(0,0,180);
				}
			}
		}


		mFlipTimer -= Time.deltaTime;

	}//END of Update()
	
	protected virtual void LateUpdate () 
	{
		//Keep ship within screen bounds ~Adam
		if (transform.position.x < -23.5 && mShipRecovered && secondShipOnHip)// && Application.isMobilePlatform) (from when we were doing twin-stick
		{
			transform.position = new Vector3(-23.5f, transform.position.y, transform.position.z);
		}																							//Second ship is in new position now ~ Jonathan
		else if(transform.position.x < -27f)
		{
			transform.position = new Vector3(-27f, transform.position.y, transform.position.z);
		}
		if (transform.position.x > 27f)
		{
			transform.position = new Vector3(27f, transform.position.y, transform.position.z);
		}
		if (transform.position.y < -29.5f && mShipRecovered && !secondShipOnHip) { //Original is -33, but there is a new second ship position now ~ Jonathan
			transform.position = new Vector3 (transform.position.x, -29.5f, transform.position.z);
			isOnBottomY = true;
		} else if (transform.position.y < -33f) {
			isOnBottomY = true;
			transform.position = new Vector3 (transform.position.x, -33f, transform.position.z);
		} else {

			isOnBottomY = false;
		}
		if (transform.position.y > 23f)
		{
			transform.position = new Vector3(transform.position.x, 23, transform.position.z);
		}
		
		if(mThreeBulletTimer <= 0f)
		{
			mThreeBullet = false;
		}
	}//END of LateUpdate()


	//For flipping ships upside down in co-op mode ~Adam
	public virtual void OnCollisionEnter(Collision collision)
	{
		if(collision.gameObject.GetComponent<PlayerBulletController>() != null)
		{
			if(collision.gameObject.GetComponent<PlayerBulletController>().mPlayerBulletNumber != 1 )
			{
				Destroy (collision.gameObject);
				if(mFlipTimer <= 0f && mPlayerTwo.mSpinTimer <= 0f)
				{
					mFlipTimer = 1f;
					mFlipped = !mFlipped;
					if(!mFlipped)
					{
						mMainShip.transform.localRotation = Quaternion.Euler (new Vector3(0,0,180));
					}
					else
					{
						mMainShip.transform.localRotation = Quaternion.Euler (new Vector3(0,0,0));
					}
				}
			}
		}
	}

	//Toggle firing on and off ~Adam
	public virtual void ToggleFire()
	{
		mToggleFireOn = !mToggleFireOn;
	}//END of ToggleFire()



	protected virtual void OnLevelWasLoaded()
	{
		Input.ResetInputAxes();

		mPlayerInputDevice = null;
	}//END of OnLevelWasLoaded()
	

	#region For spinning the ship around when hit ~Adam
	public virtual void StartSpin()
	{
		mSpinTimer = mSpinTimerDefault;
		mSpinning = Random.Range(-1,1);
		if (mSpinning == 0f)
		{
			mSpinning += 0.1f;
		}


	}//END of StartSpin()
	

	
	public virtual void SpinShip(float spinDir)
	{
		if(spinDir > 0f)
		{
			mMainShip.transform.Rotate(Vector3.forward*Time.deltaTime*720f);
			mSecondShip.transform.Rotate(Vector3.forward*Time.deltaTime*-720f);
		}
		else if (spinDir < 0f)
		{
			mMainShip.transform.Rotate(Vector3.forward*Time.deltaTime*-720f);
			mSecondShip.transform.Rotate(Vector3.forward*Time.deltaTime*720f);
		}
		
	}//END of SpinShip()
	#endregion

	//For getting hit by boss beams ~Adam
	protected virtual void OnParticleCollision(GameObject other)
	{
		Debug.Log("The player was shot by a particle");
		if(mScoreMan != null)
		{
			mScoreMan.LoseALife();
		}
	}//END of OnParticleCollision

	//For taking weapon/movement damage ~Adam
	public virtual void TakeStatDamage()
	{
		mMoveUpgrade -= 0.005f;
		mFireUpgrade -= 0.005f;

		if(mMoveUpgrade < 0.6f)
		{
			mMoveUpgrade = 0.6f;
		}

		if(mFireUpgrade < 0.6f)
		{
			mFireUpgrade = 0.6f;
		}
	}//END of TakeStatDamage()



	#region breaking down parts of the Update() function for parts that will be different between Player1 and Player2
	//managing input devices on co-op mode ~Adam
	protected virtual void ManageInputDevice()
	{
		if (mPlayerInputDevice == null) 
		{
			//	Debug.Log("Setting player one controller");
			if(mPlayerInputMeta == "")
			{
				mPlayerInputDevice = InputManager.ActiveDevice;
				mPlayerInputMeta = mPlayerInputDevice.Meta;
			}
			else if (mPlayerInputMeta == InputManager.ActiveDevice.Meta)
			{
				mPlayerInputDevice = InputManager.ActiveDevice;
				mPlayerInputMeta = mPlayerInputDevice.Meta;
			}
		}	
	}//END of ManageInputDevice()

	//For Spinning the ship ~Adam
	protected virtual void SpinControl()
	{
		mSpinTimer -= Time.deltaTime;
		SpinShip(mSpinning);
		
		if (mSpinTimer <= 0f)
		{
			mSpinning = 0f;
			mMainShip.transform.rotation = Quaternion.identity;
			
			if(secondShipOnHip)
			{
				mSecondShip.transform.rotation = Quaternion.identity;
			}
			else
			{
				
				mSecondShip.transform.rotation = Quaternion.Euler(0,0,180);
			}
		}
	}//END of SpinControl()

	//Make the player drift toward the bottom of the screen ~Adam
	protected virtual void DriftDown()
	{
		if(mMoveDir.y < 0f && mDriftDown)
		{
			foreach (ParticleSystem shipTrail in this.GetComponentsInChildren<ParticleSystem>())
			{
				if(shipTrail.gameObject != mDamageParticles)
				{
					shipTrail.enableEmission = false;
				}
			}
			
			
			if(mDropSpeed < mMaxDropSpeed)
			{
				mDropSpeed += mDropAccelRate;
			}
			else
			{
				mDropSpeed = mMaxDropSpeed;
			}
			
		}
		else
		{
			foreach (ParticleSystem shipTrail in this.GetComponentsInChildren<ParticleSystem>())
			{
				if(shipTrail.gameObject != mDamageParticles)
				{
					if(!(mShipRecovered && !secondShipOnHip))
					{
						shipTrail.enableEmission = true;
					}
					else
					{
						shipTrail.enableEmission = false;
					}
				}
			}
			
			mDropSpeed -= mDropDeccelRate;
			
			if(mDropSpeed <= 0.01f)
			{
				mDropSpeed = 0.01f;
			}
		}

		//Make the player drift faster towards the bottom while firing ~Adam
		if(mToggleFireOn && Time.timeScale != 0f)
		{
			//Don't drift down from firing if you have a second ship pointing down (because it would be pushing you back up) ~Adam
			if(!(mShipRecovered && !secondShipOnHip) )
			{
				//Commented lines leftover from when rate of fire scaled with level number ~Adam
				//transform.position += new Vector3(0f,-0.00255f*Application.loadedLevel, 0f);
				//transform.position += new Vector3(0f,-0.06375f*Application.loadedLevel/(Application.levelCount-3), 0f);
				transform.position += new Vector3(0f,-0.06375f*16/(Application.levelCount-3), 0f);
			}
			//Decrease the timer on triple bullets while firing ~Adam
			mThreeBulletTimer -= Time.deltaTime;
		}
	}//END of DriftDown()


	protected virtual void TakeDirectionalInput()
	{
		mInputHorizontal = 0f;
		mInputVertical = 0f;
		
		//If statement for avoiding getting NaN returns when paused
		if(!mPauseMan.isPaused && !mPauseMan.isPrePaused)
		{

			
			if(mPlayerInputDevice.LeftStick.X > 0.3f || mPlayerInputDevice.LeftStick.X < -0.3f)
			{
				mInputHorizontal = mPlayerInputDevice.LeftStick.X;
			}
			if(mPlayerInputDevice.LeftStick.Y > 0.3f || mPlayerInputDevice.LeftStick.Y < -0.3f)
			{
				mInputVertical = mPlayerInputDevice.LeftStick.Y;
			}


			//take in arrow keys if there's no player 2 ~Adam
			if(mPlayerTwo == null || !mPlayerTwo.isActiveAndEnabled)
			{
				if(Input.GetKey(KeyCode.UpArrow))
					mInputVertical = 1;
				if(Input.GetKey(KeyCode.LeftArrow))
					mInputHorizontal = -1;
				if(Input.GetKey(KeyCode.DownArrow))
					mInputVertical = -1;
				if(Input.GetKey(KeyCode.RightArrow))
					mInputHorizontal = 1;
			}

			//Doing straight keyboard bindings instead of input.getaxis because of conflicts with two gamepads
			if(Input.GetKey(KeyCode.W))
				mInputVertical = 1;
			if(Input.GetKey(KeyCode.A))
				mInputHorizontal = -1;
			if(Input.GetKey(KeyCode.S))
				mInputVertical = -1;
			if(Input.GetKey(KeyCode.D))
					mInputHorizontal = 1;

			
			
			//Gamepad D-Pad input ~Adam
			if(mPlayerInputDevice.DPadDown.IsPressed)
			{
				mInputVertical = -1f;
			}
			if(mPlayerInputDevice.DPadUp.IsPressed)
			{
				mInputVertical = 1f;
			}
			if(mPlayerInputDevice.DPadLeft.IsPressed)
			{
				mInputHorizontal = -1f;
			}
			if(mPlayerInputDevice.DPadRight.IsPressed)
			{
				mInputHorizontal = 1f;
			}
		}
		
		
		mMainShipAnimator.SetInteger("Direction", Mathf.RoundToInt(mInputHorizontal));
		mSecondShipAnimator.SetInteger("Direction", Mathf.RoundToInt(mInputHorizontal));
		
		
		//Delete the ship if we've returned to the title screen ~Adam
		if(Application.loadedLevel == 0)
		{
			Destroy(this.gameObject);
		}
		
		
		//For making the ship drift down when not trying to go up ~Adam
		if(mInputVertical > 0f)
		{
			mDriftDown = false;
		}
		else
		{
			mDriftDown = true;
		}
		
		SetMovementDirection(mInputHorizontal, mInputVertical);



	}//END of TakeDirectionalInput()

	protected virtual void SetMovementDirection(float horizontal, float vertical)
	{
		//Movement input for mouse/touch on mobile
		if(Input.GetMouseButton(0) && (Application.isMobilePlatform)  && Time.timeScale != 0f)
		{
			Vector3 screenPos = Camera.main.WorldToScreenPoint(this.transform.position);
			
			Vector3 transitionAboveFinger = new Vector3(0f, Screen.height * 0.1f, 0f);
			Vector3 translationDirection = Vector3.Normalize(Input.mousePosition + transitionAboveFinger - screenPos);
			
			
			//For making the ship drift down when not trying to go up
			if (Input.mousePosition.y + 10f + Screen.height * 0.1f > screenPos.y - 10f)
			{
				mDriftDown = false;
			}
			else
			{
				mDriftDown = true;
			}
			
			#if UNITY_ANDROID
			mMoveDir = Vector3.Lerp(mMoveDir, new Vector3(translationDirection.x, translationDirection.y, 0f) * 2f * mMovementSpeed * Time.deltaTime, 0.5f);
			#else
			mMoveDir = Vector3.Lerp(mMoveDir, new Vector3(translationDirection.x, translationDirection.y, 0f)*2f*mMovementSpeed*Time.deltaTime, 0.08f);
			#endif
			
		}
		
		
		//Set the Movement Direction when not on mobile ~Adam
		else if (horizontal != 0.0f || vertical != 0.0f && Time.timeScale != 0f)
		{
			
			
			//Left ~Adam
			if (horizontal < 0.0f && vertical == 0.0f)
			{
				mMoveDir = Vector3.Lerp(mMoveDir, new Vector3((2f*mMovementSpeed * -1.0f) * Time.deltaTime, 0.0f, 0.0f), 0.08f);
			}
			//Right ~Adam
			else if (horizontal > 0.0f && vertical == 0.0f)
			{
				mMoveDir = Vector3.Lerp(mMoveDir,new Vector3(2f*mMovementSpeed * Time.deltaTime, 0.0f, 0.0f), 0.08f);
			}
			//Down ~Adam
			else if (vertical < 0.0f && horizontal == 0.0f)
			{ 
				mMoveDir = Vector3.Lerp(mMoveDir, new Vector3(0.0f, (2f*mMovementSpeed * -1.0f) * Time.deltaTime, 0.0f), 0.08f);
			}
			//Up ~Adam
			else if (vertical > 0.0f && horizontal == 0.0f)
			{
				mMoveDir = Vector3.Lerp(mMoveDir, new Vector3(0.0f, 2f*mMovementSpeed * Time.deltaTime, 0.0f), 0.08f);
			}
			//Up+Right ~Adam
			else if (vertical > 0.0f && horizontal > 0.0f)
			{
				mMoveDir = Vector3.Lerp(mMoveDir, Vector3.Normalize(new Vector3(1f,1f,0))*2f*mMovementSpeed * Time.deltaTime , 0.08f);
			}
			//Up+Left ~Adam
			else if (vertical > 0.0f && horizontal < 0.0f)
			{
				mMoveDir = Vector3.Lerp(mMoveDir, Vector3.Normalize(new Vector3(-1f,1f,0))*2f*mMovementSpeed * Time.deltaTime , 0.08f);
			}
			//Down+Right ~Adam
			else if (vertical < 0.0f && horizontal > 0.0f)
			{
				mMoveDir = Vector3.Lerp(mMoveDir, Vector3.Normalize(new Vector3(1f,-1f,0))*2f*mMovementSpeed * Time.deltaTime, 0.08f);
			}
			//Down+Left ~Adam
			else if (vertical < 0.0f && horizontal < 0.0f)
			{
				mMoveDir = Vector3.Lerp(mMoveDir, Vector3.Normalize(new Vector3(-1f,-1f,0))*2f*mMovementSpeed * Time.deltaTime, 0.08f);
			}
		}
	}//END of SetMovementDirection()

	protected virtual void TakeFiringInput()
	{
		//Keyboard and mouse input and InControl Gamepad input ~Adam
		//Fire used to be via toggle ~Adam
//		if(mPlayerInputDevice.Action1.WasPressed || mPlayerInputDevice.Action4.WasPressed || Input.GetButtonDown("FireGun"))
//		{
//			Debug.Log("InControl button pressed");
//			ToggleFire();
//		}
		
		//Firing via hold-to-fire ~Adam
		if(mPlayerInputDevice.Action1.IsPressed || mPlayerInputDevice.Action4.IsPressed || Input.GetButton("FireGun"))
		{
			mToggleFireOn = true;
		}
		else if(mToggleFireOn)
		{
			mToggleFireOn = false;
		}
		//Fire held super weapon ~Adam
		//Can hold multiple super weapons.  They fire in a priority order: Laser Fist, then Big Blast ~Adam
		//Have to wait for one to finish firing before firing another ~Adam
		if( (mPlayerInputDevice.RightTrigger.WasPressed || Input.GetButtonDown("FireSuperGun")) && !mBigBlast.activeSelf && !mLaserFist.activeSelf)
		{
			//Prevent from firing while the "Get Ready" message is up ~Adam
			if(FindObjectOfType<GetReady>() == null)
			{
				//Fire the Laser Fist with priority ~Adam
				if(mHaveLaserFist)
				{
					mLaserFist.SetActive(true);
					mHaveLaserFist = false;

					if (mShielded && AchievementManager.instance != null)
                    {
                        AchievementManager.instance.PostAchievement("EverythingIveGot");
                    }
					
					Camera.main.GetComponent<CameraShaker> ().RumbleController(.3f, 5.5f);
				}
				//Fire the Big Blast ~Adam
				else if(mHaveBigBlast)
				{
					mBigBlast.SetActive(true);
					mHaveBigBlast = false;
					
					Camera.main.GetComponent<CameraShaker> ().RumbleController(.6f, 2f);
				}
			}
		}
	}//END of TakeFiringInput()

	//Thruster control for hovering ~Adam
	protected virtual void TakeThrusterInput()
	{
		if ( (mPlayerInputDevice.Action2.IsPressed || mPlayerInputDevice.Action3.IsPressed || Input.GetButton ("Thrusters")) && !mHoverDisabled) {
			//Slow down movement while hovering~Adam
			mMoveDir *= 0.95f;

			isHovering = true;
			
			mDropSpeed -= mDropDeccelRate * 3f;
			if (mDropSpeed <= 0.01f) {
				mDropSpeed = 0.00f;
			}
			
			if (mMoveDir.y < -0.2f) {
				foreach (ParticleSystem shipTrail in this.GetComponentsInChildren<ParticleSystem>()) {
					if (shipTrail.gameObject != mDamageParticles) {
						shipTrail.enableEmission = false;
					}
				}
			} else {
				foreach (ParticleSystem shipTrail in this.GetComponentsInChildren<ParticleSystem>()) {
					if (shipTrail.gameObject != mDamageParticles) {
						if (!(mShipRecovered && !secondShipOnHip)) {
							shipTrail.enableEmission = true;
						} else {
							shipTrail.enableEmission = false;
						}
					}
				}
			}
		} else {

			isHovering = false;
		}
		
	}//END of TakeThrusterInput()

	//So bullets know who fired them ~Adam
	protected virtual void SetBulletNumber(PlayerBulletController bullet)
	{
		switch(playerIndex)
		{
		case PlayerIndex.One:
			bullet.mPlayerBulletNumber = 1;
			break;
		case PlayerIndex.Two:
			bullet.mPlayerBulletNumber = 2;
			break;
		case PlayerIndex.Three:
			bullet.mPlayerBulletNumber = 3;
			break;
		case PlayerIndex.Four:
			bullet.mPlayerBulletNumber = 4;
			break;
		default:
			break;
		}
	}
	public virtual void Respawn()
	{
		mPlayerInputDevice = null;
//		mPlayerInputMeta = "";
		ManageInputDevice();
	}
	#endregion

	#region Code for externally toggling the ability to hover.  Was first put in for the "Flappy Bird" parody level ~Adam
	public void DisableHover(bool temp, float disableTime)
	{
		if(temp)
		{
			StartCoroutine(HoverDisableTimer(disableTime));
		}
		else
		{
			mHoverDisabled = true;
		}
	}

	public void EnableHover()
	{
		mHoverDisabled = false;
	}

	protected IEnumerator HoverDisableTimer(float disableTime)
	{				
		mHoverDisabled = true;
		yield return new WaitForSeconds(disableTime);
		mHoverDisabled = false;
	}
	#endregion

}//END of MonoBehavior