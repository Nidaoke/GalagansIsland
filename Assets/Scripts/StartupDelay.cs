using UnityEngine;
using System.Collections;

public class StartupDelay : MonoBehaviour 
{
	[SerializeField] private float mStartupDelayTime = 8f;
	[SerializeField] private bool mEnableMonoBeaviour = true;
	[SerializeField] private MonoBehaviour mScriptToActivate;
	[SerializeField] private bool mEnableObject = false;
	[SerializeField] private GameObject mObjectToActivate;

	// Use this for initialization
	void Start () 
	{
		StartCoroutine(StartDelay());
	}
	
	IEnumerator StartDelay()
	{
		yield return new WaitForSeconds(mStartupDelayTime);
		if(mEnableObject && mObjectToActivate != null)
		{
			mObjectToActivate.SetActive(true);
		}
		if(mEnableMonoBeaviour && mScriptToActivate != null)
		{
			mScriptToActivate.enabled = true;
		}
	}
}
