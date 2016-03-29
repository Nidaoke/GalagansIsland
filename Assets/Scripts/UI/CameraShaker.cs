using UnityEngine;
using System.Collections;
using XInputDotNetPure; // Pluging for game pad rumble
using InControl;

public class CameraShaker : MonoBehaviour 
{

	public float rumbleTime;

	float strength = 2.5f;
	float duration = .25f;

	private float mRedShakeStrength = .55f;
	private float mRedShakeDuration = .3f;

	private float mTealShakeStrength = .36f;
	private float mTealShakeDuration = .2f;
	
	private float mGreenShakeStrength = .6f;
	private float mGreenShakeDuration = .2f;

	private float mPurpleShakeStrength = .75f;
	private float mPurpleShakeDuration = .3f;

	private float mDeathShakeStrength = 1f;
	private float mDeathShakeDuration = .4f;

	private float mEnemyShakeStrength = .1f; //was 5.0f Changed to 2.5f for Florida Con
	private float mEnemyShakeDuration = .1f;

	private float mShootShakeStrength = .2f;
	private float mShootShakeDuration = .1f;

	float mShakeTime = 0f;
	Vector3 mStartingPosition;
	// Use this for initialization
	void Start () 
	{
		mStartingPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () 
	{

		if (rumbleTime <= 0) {

			GamePad.SetVibration (0, 0, 0);
			GamePad.SetVibration (PlayerIndex.Two, 0, 0);

			rumbleTime = 0;
		} else {

			rumbleTime -= Time.deltaTime;
		}

		if (mShakeTime > 0 && Time.timeScale != 0f)
		{
			transform.position = mStartingPosition+(Random.insideUnitSphere * strength);
			mShakeTime -= Time.deltaTime;
#if !UNITY_ANDROID
			//Took these out for better customizable Rumble ~ Jonathan
			//GamePad.SetVibration(0, strength, strength);
			//GamePad.SetVibration(PlayerIndex.Two, strength, strength);

			

#endif
			//InputManager.ActiveDevice.Vibrate(strength);
		}
		else
		{
#if !UNITY_ANDROID
			//GamePad.SetVibration(0, 0, 0);
			//GamePad.SetVibration(PlayerIndex.Two, 0, 0);

#endif
			//InputManager.ActiveDevice.Vibrate(0f);
			mShakeTime = 0f;
			transform.position = mStartingPosition;
		}
	}

	public void RumbleController(float rumbleStrength, float rumbleDuration)
	{
		if(PlayerPrefs.GetInt("RumbleOn")==0)
		{
			GamePad.SetVibration(0, rumbleStrength, rumbleStrength);
			GamePad.SetVibration(PlayerIndex.Two, rumbleStrength, rumbleStrength);

			rumbleTime = rumbleDuration;
		}
		else
		{
			rumbleTime = 0f;
		}
	}

	public void ShakeCamera()
	{
		strength = mShootShakeStrength;
		mShakeTime = mShootShakeDuration;
	}
	public void ShakeCameraEnemy(){

		mShakeTime = mEnemyShakeDuration;
		strength = mEnemyShakeStrength;
	}
	public void ShakeCameraDeath(){

		mShakeTime = mDeathShakeDuration;
		strength = mDeathShakeStrength;
	}
	public void ShakeCameraPurple(){

		mShakeTime = mPurpleShakeDuration;
		strength = mPurpleShakeStrength;
	}
	public void ShakeCameraTeal(){
		
		mShakeTime = mTealShakeDuration;
		strength = mTealShakeStrength;
	}
	public void ShakeCameraRed(){
		
		mShakeTime = mRedShakeDuration;
		strength = mRedShakeStrength;
	}
	public void ShakeCameraGreen(){
		
		mShakeTime = mGreenShakeDuration;
		strength = mGreenShakeStrength;
	}
}
