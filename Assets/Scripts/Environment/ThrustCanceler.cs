using UnityEngine;
using System.Collections;

public class ThrustCanceler : MonoBehaviour 
{

	[SerializeField] private float mActivationDelay = 30f;
	bool mThrustCancelOn = false;
	[SerializeField] private PlayerOneShipController mP1Ship;
	[SerializeField] private PlayerTwoShipController mP2Ship;
	[SerializeField] private GameObject mGraphicEffect;
	[SerializeField] private bool mPeriodic = false;
	[SerializeField] private float mPeriodicFireDuration = 3f;
	[SerializeField] private float mPeriodicCooldownDuration = 5f;
	[SerializeField] private bool mGiveWarning = false;
	[SerializeField] private float mWarningDuration = 3f;
	[SerializeField] private bool mTempCancelation = false;
	[SerializeField] private float mTempCancelationDuration = 10f;


	[SerializeField] LevelKillCounter mKillCounter;

	// Use this for initialization
	void Start () 
	{
		StartCoroutine(StartActivationDelay());
	}



	// Update is called once per frame
	void Update () 
	{
		if(mKillCounter.mLevelComplete)
		{
			if(mP1Ship != null)
			{
				mP1Ship.EnableHover();
			}
			if(mP2Ship != null)
			{
				mP2Ship.EnableHover();
			}
		}
	}

	//For first-time fire ~Adam
	IEnumerator StartActivationDelay()
	{
		yield return new WaitForSeconds(mActivationDelay);
		mP1Ship = FindObjectOfType<PlayerOneShipController>();
		mP2Ship = FindObjectOfType<PlayerTwoShipController>();

		if(!mGiveWarning)
		{
			StartCoroutine(InstantFire());
		}
		else
		{
			StartCoroutine(WarningDelay());
		}
	}//END of StartActivationDelay()

	IEnumerator WarningDelay()
	{
		mGraphicEffect.SetActive(true);
		yield return new WaitForSeconds(mWarningDuration);
		GetComponent<BoxCollider>().enabled = true;

		if (mPeriodic)
		{
			yield return new WaitForSeconds(mPeriodicFireDuration);
			StartCoroutine(PeriodicRestartDelay());
		}

	}//END of WarningDelay()

	IEnumerator InstantFire()
	{
		mGraphicEffect.SetActive(true);
		GetComponent<BoxCollider>().enabled = true;
		if (mPeriodic)
		{
			yield return new WaitForSeconds(mPeriodicFireDuration);
			StartCoroutine(PeriodicRestartDelay());
		}
	}

	IEnumerator PeriodicRestartDelay()
	{
		mGraphicEffect.SetActive(false);
		GetComponent<BoxCollider>().enabled = false;
		yield return new WaitForSeconds(mPeriodicCooldownDuration);

		if(!mGiveWarning)
		{
			StartCoroutine(InstantFire());
		}
		else
		{
			StartCoroutine(WarningDelay());
		}
	}//END of PeriodicRestartDelay()

	void OnTriggerEnter(Collider other)
	{
		//Turn off the ship's thrusters ~Adam
		if(other.GetComponent<PlayerShipController>() != null)
		{
			if(!mPeriodic)
			{
				GetComponent<BoxCollider>().enabled = false;
				mGraphicEffect.SetActive(false);
			}

			if(mP1Ship != null)
			{
				mP1Ship.DisableHover(mTempCancelation, mTempCancelationDuration);
			}
			if(mP2Ship != null)
			{
				mP2Ship.DisableHover(mTempCancelation, mTempCancelationDuration);
			}
		}
	}//END of OnTriggerEnter()


}


