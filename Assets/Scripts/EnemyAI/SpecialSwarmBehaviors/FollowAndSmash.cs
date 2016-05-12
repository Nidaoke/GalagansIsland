using UnityEngine;
using System.Collections;

public class FollowAndSmash : MonoBehaviour 
{
	ScoreManager mScoreMan;
	Vector3 mTargetPos = Vector3.zero;

	enum TrackingState {FOLLOW, HANG, DROP};

	[SerializeField] private TrackingState mTrackState = TrackingState.FOLLOW;

	[SerializeField] private float mFollowTime = 10f;
	[SerializeField] private float mHangTime = 5f;
	[SerializeField] private float mDropTime = 5f;
	[SerializeField] private float mSpeed = 5f;
	[SerializeField] private float mStartDelayTime = 0f;
	// Use this for initialization
	void Start () 
	{
		mScoreMan = FindObjectOfType<ScoreManager>();
		if(mScoreMan != null && mScoreMan.mPlayerAvatar != null)
		{
			mTargetPos = mScoreMan.mPlayerAvatar.transform.position;
		}
		StartCoroutine(StartDelay());
	}//END of Start()
	
	// Update is called once per frame
	void Update () 
	{
		if(mScoreMan == null )
		{
			mScoreMan = FindObjectOfType<ScoreManager>();
		}
		else if (mTrackState == TrackingState.FOLLOW)
		{
			if(mScoreMan.mP1Score >= mScoreMan.mP2Score && mScoreMan.mPlayerAvatar != null)
			{
				mTargetPos = mScoreMan.mPlayerAvatar.transform.position;
			}
			else if (mScoreMan.mPlayer2Avatar != null)
			{
				mTargetPos = mScoreMan.mPlayer2Avatar.transform.position;
			}
		}

		switch (mTrackState)
		{
		case TrackingState.FOLLOW:
		case TrackingState.HANG:
			transform.Translate(Vector3.Normalize(new Vector3(mTargetPos.x, 0f,transform.position.z) - transform.position)*mSpeed*Time.deltaTime);
//			transform.position = Vector3.Lerp(transform.position, new Vector3(mTargetPos.x, 0f,transform.position.z), mSpeed*Time.deltaTime);
			break;
		case TrackingState.DROP:
			transform.Translate(Vector3.Normalize(new Vector3(mTargetPos.x, -25f,transform.position.z) - transform.position)*mSpeed*10f*Time.deltaTime);
		//	transform.position = Vector3.Lerp(transform.position, new Vector3(mTargetPos.x, -25f,transform.position.z), mSpeed*5f*Time.deltaTime);
			break;
		default:
			break;
		}


	}//END of Update()


	//Timer so it does start doing it's cycle until after the repair station has gone through
	IEnumerator StartDelay()
	{
		Debug.Log("Waiting for " + mStartDelayTime + " seconds at start");
		yield return new WaitForSeconds(mStartDelayTime);
		StartTrackingLoop();
	}

	//This should be running in a constant loop for cycling behavior states without being called every frame ~Adam
	IEnumerator TrackingLoop()
	{
		mTrackState = TrackingState.FOLLOW;
		yield return new WaitForSeconds(mFollowTime);
		mTrackState = TrackingState.HANG;
		yield return new WaitForSeconds(mHangTime);
		mTrackState = TrackingState.DROP;
		yield return new WaitForSeconds(mDropTime);
		mTrackState = TrackingState.HANG;
		yield return new WaitForSeconds(mHangTime/2f);
		StartTrackingLoop();
	}//END of TrackingLoop()

	//Keeps TrackingLoop() from calling itself directly, which seems like a terrible idea ~Adam
	void StartTrackingLoop()
	{
		Debug.Log("New loop started");
		StartCoroutine(TrackingLoop());
	}//END of StartTrackingLoop()



}
