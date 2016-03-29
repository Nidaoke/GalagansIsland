using UnityEngine;
using System.Collections;

public class CreditsSpawner : MonoBehaviour 
{
	public enum CreditsAlign{Rand, Center, Last};

	public GameObject laser;


	[SerializeField] private GameObject mCreditsBlockPrefab;
	[SerializeField] private string[] mCredits; //Each person's name/job is an entry in this array ~Adam
	[SerializeField] private CreditsAlign[] mUseRandomPositions; //For ensuring that certain lines are centered
	[SerializeField] private float[] mNextLineTime; //How long to take between spawning each line


	public bool mRollingCredits = false;
	[SerializeField] private float mTimeToReturnToStart = 30f;

	Vector3 mCreditsSpawnPosition = new Vector3(0f,-40f, -2f);

	float mCreditsTimer = 0f;
	float mNewCreditTime = 1f;
	int mCreditNumber = 0;
	// Use this for initialization
	void Start () 
	{
		GetComponent<Renderer>().material.color = new Color(0f,0f,0f,0f);
		AudioListener.volume = 1;
		PlayerPrefs.SetInt ("CheckPointedLevel", 0); //Reset the Checkpoints

	}
	
	// Update is called once per frame
	void Update () 
	{
		if(mRollingCredits)
		{
			mTimeToReturnToStart -= Time.deltaTime;

			//Spawn a new credit block every couple of seconds `Adam
			mCreditsTimer+= Time.deltaTime;

			if(mCreditsTimer >= mNewCreditTime && mCreditNumber < mCredits.Length)
			{
				mNewCreditTime+=mNextLineTime[mCreditNumber];

				if(mUseRandomPositions[mCreditNumber] == CreditsAlign.Rand)
				{
					mCreditsSpawnPosition = new Vector3(Random.Range(-10f,10f),-40f,-2f);
				}
				else if(mUseRandomPositions[mCreditNumber] == CreditsAlign.Center)
				{
					mCreditsSpawnPosition = new Vector3(0,-40f,-2f);
				}

				GameObject creditBlock;
				creditBlock = Instantiate(mCreditsBlockPrefab, mCreditsSpawnPosition,Quaternion.identity) as GameObject;
				creditBlock.GetComponent<CreditsBlock>().mCreditText = mCredits[mCreditNumber];
				mCreditNumber++;
			}

			if(mTimeToReturnToStart <= 5f){

				if(laser != null){

					laser.SetActive(true);
				}
			}

			if(mTimeToReturnToStart <= 4f)
			{
				GetComponent<Renderer>().enabled = true;
				GetComponent<Renderer>().material.color = Color.Lerp(GetComponent<Renderer>().material.color, new Color(0f,0f,0f,1f), 0.01f);
			}

			//Return to Main Menu ~Adam
			if (mTimeToReturnToStart <= 0f)
			{
				Destroy(laser);

				ScoreManager scoreMan = FindObjectOfType<ScoreManager>();

				PlayerOneShipController p1Ship = FindObjectOfType<PlayerOneShipController>();
				PlayerTwoShipController p2Ship = FindObjectOfType<PlayerTwoShipController>();


				if(scoreMan != null)
				{
					Destroy(scoreMan.gameObject);
				}
				if(p1Ship != null)
				{
					Destroy(p1Ship.gameObject);
				}
				if(p2Ship != null)
				{
					Destroy(p2Ship.gameObject);
				}

				Application.LoadLevel(0);
			}
		}
	}
}
