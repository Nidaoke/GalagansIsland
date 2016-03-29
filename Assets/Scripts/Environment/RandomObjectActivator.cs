using UnityEngine;
using System.Collections;


public class RandomObjectActivator : MonoBehaviour 
{

	[SerializeField] private GameObject[] mRandomObjects;
	[SerializeField] private float mInitialWaitTime = 15f;
	[SerializeField] private float mActiveTime = 30f;
	[SerializeField] private float mInactiveTime = 30f;
	[SerializeField] int mTargetRandomObject = 0;
	// Use this for initialization
	void Start () 
	{
		if(mRandomObjects.Length > 0)
		{	
			StartCoroutine(WaitAtStart());
		}
	}

	IEnumerator WaitAtStart()
	{
		yield return new WaitForSeconds(mInitialWaitTime);
		StartCoroutine(ActivateRandom());
	}

	IEnumerator ActivateRandom()
	{
		mTargetRandomObject = Random.Range(0,mRandomObjects.Length);
		mRandomObjects[mTargetRandomObject].SetActive(true);
		yield return new WaitForSeconds(mActiveTime);
		StartCoroutine(DeactivateTarget());
	}

	IEnumerator DeactivateTarget()
	{
		mRandomObjects[mTargetRandomObject].SetActive(false);
		yield return new WaitForSeconds(mInactiveTime);
		StartCoroutine(ActivateRandom());
	}
}
