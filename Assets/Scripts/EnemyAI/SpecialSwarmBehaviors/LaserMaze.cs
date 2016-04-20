using UnityEngine;
using System.Collections;



public class LaserMaze : MonoBehaviour 
{
	[SerializeField] private float mMazeFallSpeed = 5f;
	[SerializeField] private float mTotalFallTime = 90f;
	[SerializeField] private GameObject mGoingUp;
	[SerializeField] private GameObject mGoingDown;
	[SerializeField] private ScoreManager mScoreMan;
	int mPlayerStartLives = 100;
	[SerializeField] private GameObject[] mPrizeObjects;
	// Use this for initialization
	void Start () 
	{
		StartCoroutine(GetPlayerStartLives());
		StartCoroutine(ReverseDirection());
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.Translate(Vector3.down*mMazeFallSpeed*Time.deltaTime);
	}

	IEnumerator GetPlayerStartLives()
	{
		yield return new WaitForSeconds(1f);
		mScoreMan = FindObjectOfType<ScoreManager>();
		mPlayerStartLives = mScoreMan.mLivesRemaining;
	}

	IEnumerator ReverseDirection()
	{
		yield return new WaitForSeconds(mTotalFallTime);
		mGoingUp.SetActive(false);
		mGoingDown.SetActive(true);
		mMazeFallSpeed *= -1f;
		yield return new WaitForSeconds(mTotalFallTime);
		int hitsTaken = mPlayerStartLives - mScoreMan.mLivesRemaining;
		if(hitsTaken <= 0)
		{
			GameObject superWeaponPrize;
			superWeaponPrize = Instantiate(mPrizeObjects[0],new Vector3(0,10f,-2f), Quaternion.identity) as GameObject;
		}
		if(hitsTaken <= 3)
		{
			GameObject shieldPrize;
			shieldPrize = Instantiate(mPrizeObjects[1],new Vector3(20f,10f,-2f), Quaternion.identity) as GameObject;
		}
		if(hitsTaken <= 10)
		{
			GameObject spreadFirePrize;
			spreadFirePrize =Instantiate(mPrizeObjects[2],new Vector3(-20f,10f,-2f), Quaternion.identity) as GameObject;
		}
	}

}
