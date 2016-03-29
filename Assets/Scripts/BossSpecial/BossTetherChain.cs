using UnityEngine;
using System.Collections;

public class BossTetherChain : MonoBehaviour 
{
	[SerializeField] GameObject mPuller;
	public void ReleaseChain()
	{
		transform.parent.GetComponent<ParticleSystem>().Play ();
		//mPullers[releasedChain].transform.SetParent (null);
		mPuller.GetComponent<Rigidbody>().velocity = Vector3.down * 15f;
		gameObject.SetActive (false);
	}
}
