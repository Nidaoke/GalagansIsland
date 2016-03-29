using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//Script for disabling the player's gun at the start of each level to let swarms form

public class GetReady : MonoBehaviour 
{
	[SerializeField] private float mReadyTimer = 5f;

	public PlayerShipController mPlayer1Ship;
	public PlayerShipController mPlayer2Ship;
	[SerializeField] private Text mReadyText;
	[SerializeField] private string mStartText = "Get Ready!";
	[SerializeField] private string mFireText = "Fire Away!";
	ScoreManager mScoreMan;

	//For keeping overheat levels from resetting
	float mP1Overheat = 0f;
	float mP2Overheat = 0f;


	public float mP1ShieldTime = 0f;
	public float mP2ShieldTime = 0f;


	// Use this for initialization
	void Start () 
	{
		//Set the text that displays at the start of the level ~Adam
		mReadyText.text = mStartText;
		mScoreMan = FindObjectOfType<ScoreManager>();

		//Find the player ships ~Adam
		if(mScoreMan.mPlayerAvatar != null)
		{
			mPlayer1Ship = mScoreMan.mPlayerAvatar.GetComponent<PlayerShipController>();
			mP1Overheat = mPlayer1Ship.heatLevel;
		}
		if(mScoreMan.mPlayer2Avatar != null)
		{
			mPlayer2Ship = mScoreMan.mPlayer2Avatar.GetComponent<PlayerShipController>();
			mP2Overheat = mPlayer2Ship.heatLevel;
		}

		if(mPlayer1Ship !=null)
		{
			mP1ShieldTime = mPlayer1Ship.mShieldTimer;
		}
		if(mPlayer2Ship !=null)
		{
			mP2ShieldTime = mPlayer2Ship.mShieldTimer;
		}

	}//END of Start()
	
	// Update is called once per frame
	void Update () 
	{
		mReadyTimer -= Time.deltaTime;
		if(mReadyTimer > 1f)
		{
			//Count Down ~Adam

			//Turn off the player 1 ship's gun if the ship is present, else, find the ship ~Adam
			if(mPlayer1Ship!=null)
			{
				mPlayer1Ship.mToggleFireOn = false;
				mPlayer1Ship.isOverheated = true;
				mPlayer1Ship.heatLevel = mP1Overheat;
				mPlayer1Ship.mShieldTimer = mP1ShieldTime;
			}
			else
			{
				if(mScoreMan.mPlayerAvatar != null)
				{
					mPlayer1Ship = mScoreMan.mPlayerAvatar.GetComponent<PlayerShipController>();
				}
			}
			//Turn off the player 2 ship's gun if the ship is present, else, find the ship ~Adam
			if(mPlayer2Ship!=null)
			{
				mPlayer2Ship.mToggleFireOn = false;
				mPlayer2Ship.isOverheated = true;
				mPlayer2Ship.heatLevel = mP2Overheat;
				mPlayer2Ship.mShieldTimer = mP2ShieldTime;
			}
			else
			{
				if(mScoreMan.mPlayer2Avatar != null)
				{
					mPlayer2Ship = mScoreMan.mPlayer2Avatar.GetComponent<PlayerShipController>();
				}
			}


		}
		//Let the player fire and change the text message ~Adam
		else if(mReadyTimer > 0f)
		{
			if(mPlayer1Ship!=null)
			{
				mPlayer1Ship.isOverheated = false;
				//	mPlayer1Ship.heatLevel = mP1Overheat;
			}
			if(mPlayer2Ship!=null)
			{
				mPlayer2Ship.isOverheated = false;
				//	mPlayer2Ship.heatLevel = mP2Overheat;
			}
			mReadyText.text = mFireText;
		}
		//Delete self ~Adam
		else
		{

			Destroy(this.gameObject);
		}
	}//END of Update()
}
