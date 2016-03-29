using UnityEngine;
using System.Collections;

public class LevelKillCounter : MonoBehaviour 
{
	[SerializeField] private bool mAllowLevelSkip = false;
	public int mRequiredKills = 30;
	public int mKillCount = 0;
	float mLevelCompleteTimer = 5f;

	public bool mLevelComplete = false;

	bool mCoOpMode = false;

	[SerializeField] private GameObject mLevelCompleteMessage;

	//For spawning new waves after a certain number of enemies have been killed.
	//Make sure that these two arrays are the same length!
	[SerializeField] private EnemyShipSpawner[] mShipWaves;
	[SerializeField] private EnemyShipSpawner[] mInactiveWaves;

	[SerializeField] private int[] mShipWaveRequiredKills;

	[SerializeField] private GUIStyle mLevelCompleteStyle;

	public bool mRemainingEnemy;

	//Keep levels from ending too early~Adam
	[SerializeField] private float mMinimumLeaveTime = 20f;
	float mLeaveTimer = 0f;


	public float mP1ShieldTime = 0f;
	public float mP2ShieldTime = 0f;

	float mAudioFadeInterval = 0.1f;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(FindObjectOfType<PlayerTwoShipController>() != null)
		{
			mCoOpMode = true;
		}
		mLeaveTimer += Time.deltaTime;
//		Debug.Log(Time.time);
		if(mLevelComplete)
		{
			EnemyShipAI[] leftoverEnemies;
			leftoverEnemies = FindObjectsOfType<EnemyShipAI>();
			foreach(EnemyShipAI enemyToDelete in leftoverEnemies)
			{
				Destroy(enemyToDelete.gameObject);
			}
		}

		mRemainingEnemy = (FindObjectOfType<EnemyShipAI>() != null);

		if(mKillCount >= mRequiredKills && !mRemainingEnemy && (mLeaveTimer > mMinimumLeaveTime) )
		{
			if(mRemainingEnemy == false)
			{
				if(!mLevelComplete)
				{
					if(FindObjectOfType<PlayerOneShipController>()!=null)
					{
						mP1ShieldTime = FindObjectOfType<PlayerOneShipController>().mShieldTimer;
					}
					if(FindObjectOfType<PlayerTwoShipController>()!=null)
					{
						mP2ShieldTime = FindObjectOfType<PlayerTwoShipController>().mShieldTimer;
					}
					mAudioFadeInterval = FindObjectOfType<BGMVolumeController>().mStartingVolume/10f;
				}

				mLevelComplete = true;


			}
		}
		if(mLevelComplete)
		{
			mLevelCompleteTimer -= Time.deltaTime;

			if(FindObjectOfType<PlayerOneShipController>()!=null)
			{
				FindObjectOfType<PlayerOneShipController>().mShieldTimer = mP1ShieldTime;
			}
			if(FindObjectOfType<PlayerTwoShipController>()!=null)
			{
				FindObjectOfType<PlayerTwoShipController>().mShieldTimer = mP2ShieldTime;
			}
		}

		if (mLevelCompleteTimer <= 0f)
		{
			Debug.Log("Loading next level");
			if(Application.loadedLevel == Application.levelCount-3)
			{
				Destroy(FindObjectOfType<PlayerShipController>().gameObject);
				//Destroy(FindObjectOfType<ScoreManager>().gameObject);

				Application.LoadLevel("Credits");
				Destroy(this.gameObject);
			}
			else
			{
				//If a level is successfully completed with all players alive go to th enext level~Adam
//				if( (FindObjectOfType<PlayerShipController>()!= null && !mCoOpMode) 
//					|| (mCoOpMode && (FindObjectOfType<PlayerShipController>() != null && FindObjectOfType<PlayerTwoShipController>() != null) ) )
//				{
					FindObjectOfType<PlayerShipController>().mShipStolen = false;
					Application.LoadLevel(Application.loadedLevel+1);
					FindObjectOfType<PlayerShipController>().mShipStolen = false;
//				}

//				//If it's co-op mode and one player finishes the level after the other one dies, go to game over screen ~Adam
//				else if(mCoOpMode && (FindObjectOfType<PlayerShipController>() == null || FindObjectOfType<PlayerTwoShipController>() == null) )
//				{
//					if(FindObjectOfType<PlayerShipController>() != null)
//					{
//						Destroy(FindObjectOfType<PlayerShipController>().gameObject);
//					}
//					if(FindObjectOfType<PlayerTwoShipController>() != null)
//					{
//						Destroy(FindObjectOfType<PlayerTwoShipController>().gameObject);
//					}
//					if(FindObjectOfType<ScoreManager>()!= null)
//					{
//						FindObjectOfType<ScoreManager>().mLevelInfoText.text = "\nGame Over";
//						FindObjectOfType<ScoreManager>().enabled = false;
//					}
//
//					Application.LoadLevel("EndGame");
//					Destroy(this.gameObject);
//
//				}

			}
		}

		if(mShipWaves.Length>0 && mShipWaves.Length == mShipWaveRequiredKills.Length && mShipWaves.Length == mInactiveWaves.Length)
		{
			for (int i = 0; i < mShipWaves.Length; i++)
			{
				if(mShipWaves[i] != null && mKillCount >= mShipWaveRequiredKills[i] && mShipWaves[i].enabled == false)
				{
					mShipWaves[i].enabled=true;
				}

				if(mInactiveWaves[i] != null && mKillCount >= mShipWaveRequiredKills[i] && mInactiveWaves[i].enabled == true)
				{
					mInactiveWaves[i].enabled=false;
				}

			}
		}
		if(Input.GetKeyDown("i") && mAllowLevelSkip)
		{
			mLevelComplete = true;
		}
	}//END of Update()

	void OnGUI()
	{
		mLevelCompleteStyle.fontSize = Mathf.RoundToInt(Screen.width*0.01f);

		if(mLevelComplete)
		{
//#if UNITY_ANDROID
//			GUI.Box(new Rect(Screen.width*0.2f, Screen.height*0.35f, Screen.width*0.60f, Screen.height*0.25f), "", mLevelCompleteStyle);
//#else
//            GUI.Box(new Rect(Screen.width*0.425f, Screen.height*0.4f, Screen.width*0.15f, Screen.height*0.05f), "", mLevelCompleteStyle);
//#endif

			//Fade out the audio ~Adam
			if(FindObjectOfType<BGMVolumeController>() != null)
			{
				FindObjectOfType<BGMVolumeController>().mStartingVolume -= (mAudioFadeInterval*Time.deltaTime);
			}

			mLevelCompleteMessage.SetActive(true);
		}
	}

	public void UpdateKillCounter()
	{
		mKillCount++;
	}
}
