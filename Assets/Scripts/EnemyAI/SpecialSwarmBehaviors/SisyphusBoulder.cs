using UnityEngine;
using System.Collections;

public class SisyphusBoulder : MonoBehaviour 
{
	enum BoulderState {CLIMBING, HANGING, FALLING};
	BoulderState mBoulderState = BoulderState.CLIMBING;
	//public bool mClimbing = true;

	public float mClimbSpeed = 5f;
	public float mFallSpeed = 15f;

	[SerializeField] private GameObject mBoulderSpawners;

	[SerializeField] private int mMaxRespawnWaves = 3;
	int mRespawnWave = 0;

	[SerializeField] private float mHangTime = 3f;
	[SerializeField] private float mClimbRespawnDelay = 5f;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Push the boulder up to the top ~Adam
		if(mBoulderState == BoulderState.CLIMBING && Time.timeScale > 0f)
		{
			//transform.position += Vector3.up*mClimbSpeed;
			transform.position = Vector3.Lerp (transform.position, new Vector3(0f,24f,-2f), 0.001f*mClimbSpeed);
			if(transform.position.y < 20f)
			{
				transform.RotateAround (transform.position, Vector3.forward, mClimbSpeed*0.5f/transform.localScale.x);
			}
			else
			{
				//Give the player several seconds to shoot a path throug the boulder without respawning enemies before it drops ~Adam
				StartCoroutine(WaitToDrop(mHangTime));
			}
		}
		//Boulder falls back down ~Adam
		else if (mBoulderState == BoulderState.FALLING && Time.timeScale > 0f)
		{
			//transform.position += Vector3.up*mClimbSpeed;
			transform.position = Vector3.Lerp (transform.position, new Vector3(0f,-53f,-2f), 0.001f*mFallSpeed);
			//transform.RotateAround (transform.position, Vector3.forward, mFallSpeed*-0.1f);
			if(transform.position.y <= -52.5f)
			{
				//Beging climbing again, but use coroutine to delay the respawning of enemies so the player can dodge through ~Adam
				StartCoroutine(StartClimb());

			}
		}

	}//END of Update()

	IEnumerator WaitToDrop(float hangTime)
	{
		mBoulderSpawners.SetActive(false);
		mBoulderState = BoulderState.HANGING;
		yield return new WaitForSeconds(hangTime);
		mBoulderState = BoulderState.FALLING;
	}//END of WaitToDrop()

	IEnumerator StartClimb()
	{
		mBoulderState = BoulderState.CLIMBING;

		yield return new WaitForSeconds(mClimbRespawnDelay);

		if(mRespawnWave < mMaxRespawnWaves)
		{
			mBoulderSpawners.SetActive(true);
			mRespawnWave++;
		}
	}//END of StartClimb()
}
