using UnityEngine;
using System.Collections;

//This is for parts of the boss that are meant to hurt the player by just touching them ~Adam

public class BossWeaponCollision : MonoBehaviour 
{
	ScoreManager mScoreMan;
	[SerializeField] private int mDamage = 3;

	//For putting a delay on how soon the beam can hurt in order to let the particle effect fully extend ~Adam
		//We'll do a more thorough fix later ~Adam
	public float mSafeTime = 0f; 

	// Use this for initialization
	void Start () 
	{
		mScoreMan = FindObjectOfType<ScoreManager>();
		Debug.Log ("Start: Weapon safe to touch");
		mSafeTime = 0f;
	}//END of Start()


	void Awake()
	{
		Debug.Log ("Awake: Weapon safe to touch");
		mSafeTime = 0f;
	}

	void OnEnable() 
	{
		Debug.Log ("Enabled: Weapon safe to touch");
		mSafeTime = 0f;
	}

	// Update is called once per frame
	void Update () 
	{
		mSafeTime += Time.deltaTime;
	}//END of Update()


	void OnTriggerEnter(Collider other)
	{
		if(other.GetComponent<PlayerShipController>()!= null && mScoreMan.mPlayerSafeTime <= 0f  && mSafeTime >= 1.5f)
		{
			//Do extra damage ~Adam
			if(mDamage > 1)
			{
				if(other.GetComponent<PlayerOneShipController>()!= null && !other.GetComponent<PlayerShipController>().mShielded)
				{
					mScoreMan.mP1Lives -= (mDamage-1);
					mScoreMan.mLivesRemaining -= (mDamage-1);
					//Don't remove more lives than the player has ~Adam
					if(mScoreMan.mP1Lives <= 1)
					{
						mScoreMan.mP1Lives = 1;
					}
					if(mScoreMan.mLivesRemaining <= 1)
					{
						mScoreMan.mLivesRemaining = 1;
					}
				}
				if(other.GetComponent<PlayerTwoShipController>()!= null && !other.GetComponent<PlayerShipController>().mShielded)
				{
					mScoreMan.mP2Lives -= (mDamage-1);
					mScoreMan.mLivesRemaining -= (mDamage-1);
					//Don't remove more lives than the player has ~Adam
					if(mScoreMan.mP2Lives <= 1)
					{
						mScoreMan.mP2Lives = 1;
					}
					if(mScoreMan.mLivesRemaining <= 1)
					{
						mScoreMan.mLivesRemaining = 1;
					}
				}
			}
//			for (int i = 0; i < mDamage-1;i++)
//			{
//				mScoreMan.HitAPlayer(other.gameObject);
//				mScoreMan.mPlayerSafeTime = -1f;
//			}
			//Do a minimum of 1 point of damage and do the usual stuff that happens when damaged ~Adam
			mScoreMan.HitAPlayer(other.gameObject);
			//Debug.Log("Visual Hindrance!");
		}
	}
}
