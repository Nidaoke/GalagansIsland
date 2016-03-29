using UnityEngine;
using System.Collections;

public class ApolloArrowMovement : MonoBehaviour 
{
	//public ApolloBowMovement mBow;
	public Transform mBowTransform;
	public bool mShooting = false;
	Vector3 mFireDir;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(!mShooting)
		{
//			if(mBow.mScoreMan != null && (mBow.mPlayer1 != null || mBow.mPlayer2 != null) )
//			{
//				if(mBow.mPlayer1 != null && mBow.mScoreMan.mP1Score >= mBow.mScoreMan.mP2Score)
//				{
//					mFireDir = Vector3.Normalize (mBow.mPlayer1.transform.position - transform.position);
//				}
//				else if(mBow.mPlayer2 != null)
//				{
//					mFireDir = Vector3.Normalize (mBow.mPlayer2.transform.position - transform.position);
//				}
//
//			}
			transform.position = new Vector3(mBowTransform.position.x, mBowTransform.position.y, transform.position.z);
			transform.localRotation = mBowTransform.localRotation;
			mFireDir = transform.up*-1f;
		}

		else
		{
			transform.position += mFireDir;
			//transform.Translate (mFireDir);
		}
	}
}
