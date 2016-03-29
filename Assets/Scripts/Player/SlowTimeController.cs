using UnityEngine;
using System.Collections;

public class SlowTimeController : MonoBehaviour 
{
	[HideInInspector] public float mSlowTimeTimer = 0f; //how many FRAMES of slow time are left -Adam
	[HideInInspector] public bool mSlowTimeActive = false;

	[HideInInspector] public float mSlowTimeScale = 1f;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		if(mSlowTimeTimer <= 0f && mSlowTimeActive == true)
		{
			//Time.timeScale = 1f;
			mSlowTimeScale = 1f;
			mSlowTimeActive = false;
		}
		else if (mSlowTimeActive)
		{
			mSlowTimeTimer -= .2f;
			//mSlowTimeTimer -= Time.deltaTime;
		}
	}

	//Slow down the time scale for a certain number of FRAMES/Update() calls
	public void SlowDownTime(float timeScaling, float slowDuration)
	{
		//Time.timeScale = timeScaling;
		mSlowTimeScale = timeScaling;
		mSlowTimeTimer = slowDuration;
		mSlowTimeActive = true;
	}
}
