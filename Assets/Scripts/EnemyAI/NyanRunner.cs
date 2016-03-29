using UnityEngine;
using System.Collections;

public class NyanRunner : MonoBehaviour 
{
	[SerializeField] private float mActivationDelay = 15f;
	[SerializeField] private float mspeed = 6f;
	[SerializeField] private Transform[] mRunPoints;
	int mNextPoint = 0;
	[SerializeField] GameObject mHeldObjects;
	[SerializeField] GameObject mSpawners;
	bool mActivated = false;
	[SerializeField] private float mLeftBound = -34f;
	[SerializeField] private float mRightBound = 34f;
	float mVertMoveDir = 1f;
	
	// Use this for initialization
	void Start () 
	{
		StartCoroutine(StartActivationDelay());
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Start moving Right if it's currently spawning things ~Adam
		if(mActivated)
		{
			transform.Translate(Vector3.right*Time.deltaTime*mspeed*2f);


		}

		#region Move up and down to be able to go between pipes ~Adam
		if(transform.position.y<mRunPoints[mNextPoint].position.y)
		{
			mVertMoveDir = 1f;
		}
		else
		{
			mVertMoveDir = -1f;
		}
		transform.Translate(Vector3.up*mspeed*mVertMoveDir*Time.deltaTime);
		#endregion

		//Track which pipe to go to next ~Adam
		if(Mathf.Abs( transform.position.x - mRunPoints[mNextPoint].position.x) <= 1f)
		{
			mNextPoint++;
			if(mNextPoint >= mRunPoints.Length)
			{
				mNextPoint = 0;
			}
		}

		//Disable spawners when near right bound to avoid enemies having to run all the way across the screen
		if(transform.position.x >= mRightBound-20f)
		{
			mSpawners.SetActive(false);
		}
		else
		{
			mSpawners.SetActive(true);
		}

		//Teleport back to the left side when the edge is reached ~Adam
		if(transform.position.x >= mRightBound)
		{
			transform.position = new Vector3(mLeftBound, transform.position.y, transform.position.z);
		}
	}//END of Update()

	IEnumerator StartActivationDelay()
	{
		yield return new WaitForSeconds(mActivationDelay);
		mActivated = true;
		mHeldObjects.SetActive(true);
	}//END of StartActivationDelay()

}
