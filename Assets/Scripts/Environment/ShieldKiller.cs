using UnityEngine;
using System.Collections;

public class ShieldKiller : MonoBehaviour 
{
	[SerializeField] bool mRotateRandom;
	[SerializeField] bool mSpinActive;
	[SerializeField] float mSpinSpeed;

	[SerializeField] bool mTeleports;
	[SerializeField] bool mMoveActive;

	[SerializeField] float[] mBounds;
	[SerializeField] float mZPos = -2.5f;
	Vector3 mMoveTarget;
	public float mMoveSpeed = 15f;

	[SerializeField] float mTimer = 0;
	[SerializeField] float mDownTime = 10f;
	[SerializeField] float mChargeTime = 3f;
	[SerializeField] float mActiveTime = 5f;


	[SerializeField] GameObject mCore;
	[SerializeField] GameObject mBeam;


	// Use this for initialization
	void Start () 
	{
		mMoveTarget = transform.position;
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Increment the timer ~Adam
		mTimer += Time.deltaTime;

		//Hide the shield killer ~Adam
		if(mTimer <= mDownTime)
		{
			mCore.SetActive (false);
		}
		//Reveal ~Adam
		else if(mTimer <= mDownTime+mChargeTime+mActiveTime)
		{
			//Reveal the core and pre-beams ~Adam
			mCore.SetActive (true);

			//Turn the shield-killing beams on ~Adam
			if(mTimer >= mDownTime+mChargeTime)
			{
				mBeam.SetActive (true);
			}

			//Move around if that is enabled ~Adam
			if(mMoveActive)
			{
				Movement();
			}
			//Spin around if that is enabled ~Adam
			if(mSpinActive)
			{
				transform.RotateAround (transform.position, Vector3.forward, mSpinSpeed*0.5f);
			}

		}
		//Turn off and reset ~Adam
		else
		{
			mCore.SetActive (false);
			mBeam.SetActive (false);

			//Start next time with a new rotation ~Adam
			if(mRotateRandom)
			{
				transform.localRotation = Quaternion.Euler (new Vector3(0,0, Random.Range (0,360)));
				if(Random.Range (-1f,1f) <0f)
				{
					mSpinSpeed *= -1f;
				}
			}

			//Appear next time at a new position ~Adam
			if(mTeleports && mBounds.Length >= 4)
			{
				transform.position = new Vector3(Random.Range (mBounds[0],mBounds[1]),Random.Range (mBounds[2],mBounds[3]), mZPos);
			}

			mTimer = 0f;
		}

	}



	void Movement()
	{
		transform.position = Vector3.Lerp(transform.position, mMoveTarget, mMoveSpeed*0.001f * Time.timeScale);
		
		if(Vector3.Distance (transform.position, mMoveTarget) < 7f && mBounds.Length >= 4)
		{
			mMoveTarget = new Vector3(Random.Range (mBounds[0],mBounds[1]), Random.Range (mBounds[2],mBounds[3]),mZPos);
		}
	}//END of BowMovement()
}
