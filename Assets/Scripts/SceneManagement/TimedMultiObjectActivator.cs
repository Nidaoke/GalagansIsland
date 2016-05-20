using UnityEngine;
using System.Collections;


//Script for turning on one object on at a time (or a group at a time via parenting) on set time intervals from the start of the scene ~Adam
public class TimedMultiObjectActivator : MonoBehaviour 
{
	[SerializeField] private GameObject[] mObjectsToActivate;
	[SerializeField] private float[] mStageActivationIntervals; //The time since the previous object was activated to start the next one ~Adam
	[SerializeField] private int mActivationStage = 0;


	// Use this for initialization
	void Start () 
	{
		//Make sure that there are actually things to turn on and timers are set for all of them ~Adam
		if(mObjectsToActivate.Length > 0 && mObjectsToActivate.Length == mStageActivationIntervals.Length)
		{	
			StartCoroutine(ActivateStage());
		}
	}//END of Start()




	IEnumerator ActivateStage()
	{
		yield return new WaitForSeconds(mStageActivationIntervals[mActivationStage]);
		mObjectsToActivate[mActivationStage].SetActive(true);
		mActivationStage++;
		if(mActivationStage<mObjectsToActivate.Length)
		{
			StartCoroutine(ActivateStage());
		}
	}//END of ActivateStage()
}
