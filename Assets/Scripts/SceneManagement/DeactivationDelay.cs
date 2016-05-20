using UnityEngine;
using System.Collections;

public class DeactivationDelay : MonoBehaviour 
{
	[SerializeField] private float mDeactivationDelayTime = 8f;
	[SerializeField] private bool mDisableMonoBeaviour = true;
	[SerializeField] private MonoBehaviour mScriptToDeactivate;
	[SerializeField] private bool mDisableObject = false;
	[SerializeField] private GameObject mObjectToDeactivate;

	// Use this for initialization
	void Start () 
	{
		StartCoroutine(StartDelay());
	}

	IEnumerator StartDelay()
	{
		yield return new WaitForSeconds(mDeactivationDelayTime);
		if(mDisableObject && mObjectToDeactivate != null)
		{
			mObjectToDeactivate.SetActive(false);
		}
		if(mDisableMonoBeaviour && mScriptToDeactivate != null)
		{
			mScriptToDeactivate.enabled = false;
		}
	}
}
