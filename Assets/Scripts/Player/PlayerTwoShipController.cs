using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using InControl;
using XInputDotNetPure; // Required in C#
using Assets.Scripts.Achievements;


public class PlayerTwoShipController : PlayerShipController 
{
	


	
	// Use this for initialization
	protected override void Start () 
	{
		//Make sure we always have a reference to the score manager and set the current life percentage ~Adam
		if(FindObjectOfType<ScoreManager>() != null)
		{
			mScoreMan = FindObjectOfType<ScoreManager>();
			mPauseMan = mScoreMan.gameObject.GetComponent<PauseManager>();

		}


		//transform.localScale = new Vector3 (1.75f, 1.75f, 1.75f);
		
		//Adjust speed and scale for mobile ~Adam
		if (Application.isMobilePlatform)
		{
			mBaseMovementSpeed = 15.0f;
			transform.localScale = new Vector3(1.5f,1.5f,1.5f);
		}
		
		mShipCreationLevel = Application.loadedLevel;
		
		PlayerTwoShipController[] otherPlayerShips = FindObjectsOfType<PlayerTwoShipController>();
		//Debug.Log(otherPlayerShip.name);
		foreach(PlayerTwoShipController othership in otherPlayerShips)
		{
			if(othership.mShipCreationLevel < this.mShipCreationLevel)
			{
				Debug.Log("Found another ship so destroying self.");
				Destroy(this.gameObject);
			}
		}
		if(mScoreMan != null && mScoreMan.mPlayer2Avatar != null && mScoreMan.mPlayer2Avatar != this.gameObject)
		{
			Destroy(this.gameObject);
		}

		//mLastFramePosition = transform.position;
		
	}//END of Start()
	
	
	//Persist between level loads/reloads ~Adam
	protected override void Awake()
	{
		base.Awake();

	}//END of Awake()


	
	// Update is called once per frame
	protected override void Update () 
	{
        //Achievements
		if (Application.loadedLevelName == "Credits" && AchievementManager.instance != null)
        {
            AchievementManager.instance.PostAchievement("TeamWork");
        }

		if(mPlayerOne == null)
		{
			if(FindObjectOfType<PlayerOneShipController>()!=null)
			{
				mPlayerOne = FindObjectOfType<PlayerOneShipController>();
			}
		}
//		//Enable pausing if player one is dead ~Adam
//		if(mPlayerOne == null || !mPlayerOne.gameObject.activeInHierarchy)
//		{
//			GetComponent<PauseManager>().enabled = true;
//		}
//		else
//		{
//			GetComponent<PauseManager>().enabled = false;
//		}
		base.Update();
		

	}//END of Update()
	
	void LateUpdate () 
	{

		base.LateUpdate ();
	}//END of LateUpdate()
	
	//For flipping ships upside down in co-op mode ~Adam
	public override void OnCollisionEnter(Collision collision)
	{
		if(collision.gameObject.GetComponent<PlayerBulletController>() != null)
		{
			if(collision.gameObject.GetComponent<PlayerBulletController>().mPlayerBulletNumber != 2)
			{
				Destroy (collision.gameObject);
				if(mFlipTimer <= 0f && mPlayerOne.mSpinTimer <= 0)
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
	
	public void StartSpin()
	{
		base.StartSpin ();
	}//END of StartSpin()
	
	
	
	public void SpinShip(float spinDir)
	{
		base.SpinShip(spinDir);
	}//END of SpinShip()
	
	//For getting hit by boss beams ~Adam
	void OnParticleCollision(GameObject other)
	{
		base.OnParticleCollision (other);
	}//END of OnParticleCollision()

	//For taking weapon/movement damage ~Adam
	public void TakeStatDamage()
	{
		base.TakeStatDamage ();
	}//END of TakeStatDamage()

	
	
		#region breaking down parts of the Update() function for parts that will be different between Player1 and Player2
	//managing input devices on co-op mode ~Adam
	protected override void ManageInputDevice()
	{
			if(mPlayerInputDevice == null || mPlayerInputDevice == mPlayerOne.mPlayerInputDevice) 
		{
			//Debug.Log("Setting player 2 controller");
			if(mPlayerInputMeta == "")
			{
				mPlayerInputDevice = InputManager.ActiveDevice;
				mPlayerInputMeta = mPlayerInputDevice.Meta;
			}
			else if (mPlayerInputMeta == InputManager.ActiveDevice.Meta || mPlayerInputDevice == mPlayerOne.mPlayerInputDevice)
			{
				mPlayerInputDevice = InputManager.ActiveDevice;
				mPlayerInputMeta = mPlayerInputDevice.Meta;
			}
		}
		else
		{
			//Debug.Log("Player 2: "+mPlayerInputDevice.Name);
		}
	}//END of ManageInputDevice()

	//For Spinning the ship ~Adam
	protected override void SpinControl()
	{
		base.SpinControl();
	}//END of SpinControl()

	//Make the player drift toward the bottom of the screen ~Adam
	protected override void DriftDown()
	{
		base.DriftDown ();
		
	}//END of DriftDown()


	protected override void TakeDirectionalInput()
	{
		
		mInputHorizontal = 0f;
		mInputVertical = 0f;
		
		//If statement for avoiding getting NaN returns when paused
		if( FindObjectOfType<PauseManager>() == null || (FindObjectOfType<PauseManager>() != null && !FindObjectOfType<PauseManager>().isPaused && !FindObjectOfType<PauseManager>().isPrePaused) )
		{

			
			if(Input.GetKey(KeyCode.UpArrow))
				mInputVertical = 1;
			if(Input.GetKey(KeyCode.LeftArrow))
				mInputHorizontal = -1;
			if(Input.GetKey(KeyCode.DownArrow))
				mInputVertical = -1;
			if(Input.GetKey(KeyCode.RightArrow))
				mInputHorizontal = 1;
			
			if(mPlayerInputDevice.LeftStick.X > 0.3f || mPlayerInputDevice.LeftStick.X < -0.3f)
			{
				mInputHorizontal = mPlayerInputDevice.LeftStick.X;
			}
			if(mPlayerInputDevice.LeftStick.Y > 0.3f || mPlayerInputDevice.LeftStick.Y < -0.3f)
			{
				mInputVertical = mPlayerInputDevice.LeftStick.Y;
			}
			if(InputManager.Devices.Count > 1)
			{
				
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
		}
		mMainShipAnimator.SetInteger("Direction", Mathf.RoundToInt(mInputHorizontal));
		mSecondShipAnimator.SetInteger("Direction", Mathf.RoundToInt(mInputHorizontal));
		
		
		//Delete the ship if we've returned to the title screen
		if(Application.loadedLevel == 0)
		{
			Destroy(this.gameObject);
		}
		
		

		//Keyboard Movement Controls
		//For making the ship drift down when not trying to go up
		if(mInputVertical > 0f)
		{
			mDriftDown = false;
		}
		else
		{
			mDriftDown = true;
		}
		
		//Movement input for mouse/touch
		if(Input.GetMouseButton(0) && (Application.isMobilePlatform)  && Time.timeScale != 0f)
		{
			Vector3 screenPos = Camera.main.WorldToScreenPoint(this.transform.position);
			//Debug.Log(screenPos + ", " + Input.mousePosition);
			
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
		



		SetMovementDirection(mInputHorizontal, mInputVertical);



	}//END of TakeDirectionalInput()

	protected override void SetMovementDirection(float horizontal, float vertical)
	{
		base.SetMovementDirection(horizontal, vertical);
	}//END of SetMovementDirection()

	protected override void TakeFiringInput()
	{
		//Keyboard and mouse input and InControl Gamepad input ~Adam
		//Firing via toggle ~Adam
//		if( Input.GetButtonDown("FireGunP2") || (InputManager.Devices.Count >1 && (mPlayerInputDevice.Action1.WasPressed || mPlayerInputDevice.Action4.WasPressed) ) )
//		{
//			Debug.Log("InControl button pressed");
//			ToggleFire();
//		}
		
		//Firing via hold-to-fire ~Adam
		if( Input.GetButton("FireGunP2") || (InputManager.Devices.Count >1 && (mPlayerInputDevice.Action1.IsPressed || mPlayerInputDevice.Action4.IsPressed) ) )
		{
			mToggleFireOn = true;
		}
		else if(mToggleFireOn)
		{
			mToggleFireOn = false;
		}

		
		//Fire held super weapon ~Adam
		//Can hold multiple super weapons.  They fire in a priority order: Big Blast, then Laser Fist ~Adam
		//Have to wait for one to finish firing before firing another ~Adam
		if( ( (InputManager.Devices.Count >1 && mPlayerInputDevice.RightTrigger.WasPressed) || Input.GetButtonDown("FireSuperGunP2")) && !mBigBlast.activeSelf && !mLaserFist.activeSelf)
		{
			
			if(mHaveLaserFist)
			{
				mLaserFist.SetActive(true);
				mHaveLaserFist = false;
			}
			else if(mHaveBigBlast)
			{
				mBigBlast.SetActive(true);
				mHaveBigBlast = false;
			}
		}
	}//END of TakeFiringInput()

	//Thruster control for hovering ~Adam
	protected override void TakeThrusterInput()
	{
		if( !mHoverDisabled && (InputManager.Devices.Count >1 && (mPlayerInputDevice.Action2.IsPressed || mPlayerInputDevice.Action3.IsPressed) ) || Input.GetButton("ThrustersP2"))
		{
			//Slow down movement while hovering~Adam
			mMoveDir *= 0.95f;
			
			mDropSpeed -= mDropDeccelRate*3f;
			if(mDropSpeed <= 0.01f)
			{
				mDropSpeed = 0.00f;
			}
			
			if(mMoveDir.y < -0.2f)
			{
				foreach (ParticleSystem shipTrail in this.GetComponentsInChildren<ParticleSystem>())
				{
					if(shipTrail.gameObject != mDamageParticles)
					{
						shipTrail.enableEmission = false;
					}
				}
			}
			else
			{
				foreach (ParticleSystem shipTrail in this.GetComponentsInChildren<ParticleSystem>())
				{
					if(shipTrail.gameObject != mDamageParticles)
					{
						shipTrail.enableEmission = true;
					}
				}
			}
		}
	}//END of TakeThrusterInput()

	#endregion
	
}//END of MonoBehavior