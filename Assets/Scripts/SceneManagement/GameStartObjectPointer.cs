using UnityEngine;
using System.Collections;

//This script is for finding and activating objects that we only want instantiated on the first level we play ~Adam
//This applies to whether we're starting from the beginning or starting at a later level via checkpoint

public class GameStartObjectPointer : MonoBehaviour 
{
	public GameObject mScoreManager;
	public GameObject mP1Ship;
	public GameObject mP2Ship;
	public GameObject mHUD;
	public GameObject mMobileHUD;
	public GameObject mAsteroidSpawner;
	public GameObject mIcicleStorm;

	[SerializeField] bool mDebugStart = false;
	// Use this for initialization
	void Start () 
	{
		if(mDebugStart)
		{
			ActivateSinglePlayer();
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ActivateSinglePlayer()
	{
		//Set checkpointed stats if we're not starting from the beginning ~Adam
		if(PlayerPrefs.GetInt("CheckPointedLevel") != 0)
		{
			SetCheckpointedStats();
		}

		mP1Ship.SetActive(true);
		mP2Ship.SetActive(false);
		mScoreManager.SetActive(true);
		mAsteroidSpawner.SetActive(true);
		if(Application.loadedLevelName != "Tutorial")
		{
			mIcicleStorm.SetActive(true);
		}
		if(!Application.isMobilePlatform)
		{
			mHUD.SetActive(true);
			mMobileHUD.SetActive(false);
		}
		else
		{
			mHUD.SetActive(true);
			mMobileHUD.SetActive(false);
		}
		//Set which parts of the UI should be enabled ~Adam
		if(GameObject.Find ("P1ShipEmotes") != null)
		{
			GameObject.Find ("P1ShipEmotes").SetActive (false);
		}
		GameObject.Find("P2ShipUI").SetActive (false);
		GameObject.Find("P1ShipUI").SetActive (false);
		GameObject.Find("P1ShipUI_SPR").SetActive (true);
		GameObject.Find("P1ShipUI_SPL").SetActive (true);
		mP1Ship.GetComponent<PlayerOneShipController>().SetPlayerOneUI(GameObject.Find("P1ShipUI_SPL").GetComponent<CoOpShipPanelUI>());


	}

	public void ActivateMultiPlayer()
	{

		//Not currently running checkpoints in co-op mode ~Adam
//		if(PlayerPrefs.GetInt (PlayerPrefs.GetInt("CheckPointedLevel") != 0) )
//		{
//			SetCheckpointedStats();
//		}

		mP1Ship.SetActive(true);
		mP2Ship.SetActive(true);
		mScoreManager.SetActive(true);
		mAsteroidSpawner.SetActive(true);
		mIcicleStorm.SetActive(true);
		
		if(!Application.isMobilePlatform)
		{
			mHUD.SetActive(true);
			mMobileHUD.SetActive(false);
		}
		else
		{
			mHUD.SetActive(true);
			mMobileHUD.SetActive(false);
		}
		mScoreManager.GetComponent<ScoreManager>().StartCoOpMode ();

		//Set which parts of the UI should be enabled ~Adam
		GameObject.Find ("P1ShipEmotes").SetActive (false);
		GameObject.Find("P2ShipUI").SetActive (true);
		GameObject.Find("P1ShipUI").SetActive (true);
		GameObject.Find("P1ShipUI_SPR").SetActive (false);
		GameObject.Find("P1ShipUI_SPL").SetActive (false);
		mP1Ship.GetComponent<PlayerOneShipController>().SetPlayerOneUI(GameObject.Find("P1ShipUI").GetComponent<CoOpShipPanelUI>());

	}

	//Restore player statistics to the last checkpointed state ~Adam
	//(Not currently runing checkpoints in Co-Op Mode) ~Adam
	void SetCheckpointedStats()
	{
		//Set checkpointed lives (score goes back to 0) ~Adam
		mScoreManager.GetComponent<ScoreManager>().mLivesRemaining = PlayerPrefs.GetInt("CheckPointedLivesRemaining");
		mScoreManager.GetComponent<ScoreManager>().mP1Lives = PlayerPrefs.GetInt("CheckPointedP1Lives");
//		mScoreManager.GetComponent<ScoreManager>().mP2Lives = PlayerPrefs.GetInt("CheckPointedP2Lives");

		//Set player 1 stats (lose super weapons) ~Adam
		mP1Ship.GetComponent<PlayerShipController>().mFireUpgrade = PlayerPrefs.GetFloat("CheckPointedP1FireUpgrade");
		mP1Ship.GetComponent<PlayerShipController>().mMoveUpgrade = PlayerPrefs.GetFloat("CheckPointedP1MoveUpgrade");
		mP1Ship.GetComponent<PlayerShipController>().mShieldTimer = PlayerPrefs.GetFloat("CheckPointedP1ShieldTime");
		mP1Ship.GetComponent<PlayerShipController>().mThreeBulletTimer = PlayerPrefs.GetFloat("CheckPointedP1SpreadTime");
		mP1Ship.GetComponent<PlayerShipController>().mShielded = true;
		mP1Ship.GetComponent<PlayerShipController>().mThreeBullet = true;
		mP1Ship.GetComponent<PlayerOneShipController>().mCheckPointsRemaining = PlayerPrefs.GetInt("CheckPointedCheckPointsRemaining");
		//For when we enable Co-Op checkpoints, set player 2 stats ~Adam
//		mP2Ship.GetComponent<PlayerShipController>().mFireUpgrade = PlayerPrefs.GetInt("CheckPointedP2FireUpgrade");
//		mP2Ship.GetComponent<PlayerShipController>().mMoveUpgrade = PlayerPrefs.GetInt("CheckPointedP2MoveUpgrade");
//		mP2Ship.GetComponent<PlayerShipController>().mShieldTimer = PlayerPrefs.GetInt("CheckPointedP2ShieldTime");
//		mP2Ship.GetComponent<PlayerShipController>().mThreeBulletTimer = PlayerPrefs.GetInt("CheckPointedP2SpreadTime");
//		mP2Ship.GetComponent<PlayerShipController>().mShielded = true;
//		mP2Ship.GetComponent<PlayerShipController>().mThreeBullet = true;
	}

}
