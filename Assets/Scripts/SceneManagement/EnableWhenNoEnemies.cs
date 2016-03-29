using UnityEngine;
using System.Collections;

public class EnableWhenNoEnemies : MonoBehaviour 
{
	[SerializeField] private float mCheckInterval = 3f;
	[SerializeField] GameObject[] mObjectsToEnable;
	int mEnableCount = 0;
	[SerializeField] private LevelKillCounter mKillCounter;

	// Use this for initialization
	void Start () 
	{
		StartCheckingLoop();
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}



	//This should be running in a constant loop for cycling behavior states without being called every frame ~Adam
	IEnumerator EnemyCheckLoop()
	{
		yield return new WaitForSeconds(mCheckInterval);
		if(mKillCounter != null && !mKillCounter.mRemainingEnemy)
		{
			if(mEnableCount < mObjectsToEnable.Length)
			{
				mObjectsToEnable[mEnableCount].SetActive(true);
				mEnableCount++;
			}
		}
		StartCheckingLoop();
	}//END of TrackingLoop()

	//Keeps TrackingLoop() from calling itself directly, which seems like a terrible idea ~Adam
	void StartCheckingLoop()
	{
		StartCoroutine(EnemyCheckLoop());
	}//END of StartTrackingLoop()


}
