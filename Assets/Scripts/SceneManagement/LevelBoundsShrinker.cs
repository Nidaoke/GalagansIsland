using UnityEngine;
using System.Collections;

public class LevelBoundsShrinker : MonoBehaviour 
{
	ScoreManager mScoreMan;
	PlayerShipController mP1Ship;
	PlayerShipController mP2Ship;
	[SerializeField] private Transform[] mBorderBoxes;
	[SerializeField] private float mTurnLength = 15f;
	bool mTurnGameRunning = false;


	float[] mDefaultBounds = new float[4]; //For setting player bounds back to normal at the end ~Adam
	float[] mCurrentBounds = new float[4]; //For adjusting player bounds while the level is running ~Adam

	[SerializeField] private Vector3 mBoxScale = Vector3.one;
	[SerializeField] private Vector3 mPlayerTurnBoxSize;
	[SerializeField] private Vector3 mEnemyTurnBoxSize;

	[SerializeField] bool mTurnGameOver = false;

	[SerializeField] ForceFieldBase mWeaponLocker;

	// Use this for initialization
	void Start () 
	{
		mScoreMan = FindObjectOfType<ScoreManager>();
		mP1Ship = mScoreMan.mPlayerAvatar.GetComponent<PlayerShipController>();
		mP2Ship = mScoreMan.mPlayer2Avatar.GetComponent<PlayerShipController>();

		if(mP1Ship != null)
		{
			mDefaultBounds = mP1Ship.GetShipBounds();
		}
		else if (mP2Ship != null)
		{
			mDefaultBounds = mP2Ship.GetShipBounds();
		}

		mBoxScale = mPlayerTurnBoxSize;
		StartCoroutine(WaitForStartShrink());
	}//END of Start()
	
	// Update is called once per frame
	void Update () 
	{
		transform.localScale = Vector3.Lerp (transform.localScale, mBoxScale, Time.deltaTime);
		mBorderBoxes[0].localScale = new Vector3(1f/transform.localScale.x,1f,1f);
		mBorderBoxes[1].localScale = new Vector3(1f/transform.localScale.x,1f,1f);
		mBorderBoxes[2].localScale = new Vector3(1f,1f/transform.localScale.y,1f);
		mBorderBoxes[3].localScale = new Vector3(1f,1f/transform.localScale.y,1f);

		mCurrentBounds = new float[]{mBorderBoxes[0].transform.position.x+2.5f,mBorderBoxes[1].transform.position.x-2.5f,
			mBorderBoxes[2].transform.position.y+2.5f,mBorderBoxes[3].transform.position.y-2.5f};

	if(!mTurnGameOver)
	{
		if(mP1Ship != null)
		{
			mP1Ship.SetShipBounds(mCurrentBounds);
		}
		if(mP2Ship != null)
		{
			mP2Ship.SetShipBounds(mCurrentBounds);
		}
	}
	}//END of Update()

	IEnumerator WaitForStartShrink()
	{
		yield return new WaitForSeconds(10f);
		StartCoroutine(WaitForPlayerTurn());
	}
	IEnumerator WaitForPlayerTurn()
	{
		yield return new WaitForSeconds(mTurnLength);
		if(!mTurnGameOver)
		{
			mBoxScale = mEnemyTurnBoxSize;
			StartCoroutine(WaitForEnemyTurn());
		}
		else
		{
			StartCoroutine(WaitForEndGrowth());
		}
	}
	IEnumerator WaitForEnemyTurn()
	{
		mWeaponLocker.TurnOn();
		yield return new WaitForSeconds(mTurnLength);
		mWeaponLocker.TurnOff();
		if(!mTurnGameOver)
		{
			mBoxScale = mPlayerTurnBoxSize;
			StartCoroutine(WaitForPlayerTurn());
		}
		else
		{
			StartCoroutine(WaitForEndGrowth());
		}
	}

	IEnumerator WaitForEndGrowth()
	{
		mBoxScale = Vector3.one;
		yield return new WaitForSeconds(5f);
		EndTurnGame();
	}
	void EndTurnGame()
	{
		if(mP1Ship != null)
		{
			mP1Ship.SetShipBounds(mDefaultBounds);
		}
		if(mP2Ship != null)
		{
			mP2Ship.SetShipBounds(mDefaultBounds);
		}
	}
}
