using UnityEngine;
using System.Collections;

//This is for an object that can be shot to disable other objects in the scene ~Adam

public class ShootableGenerator : MonoBehaviour 
{

	[SerializeField] private ForceFieldBase[] mLinkedForceFields;

	[SerializeField] private int mMaxHitPoints = 20;
	[SerializeField] private int mCurrentHitPoints = 20;
	[SerializeField] private bool mCanDie = true; //If false, then it goes into a cooldown state before regaining its hitpoints ~Adam
	[SerializeField] private float mRepairCooldownTime = 15f;

	[SerializeField] private bool mToggleInSequence = false; //Cycles through fields turning them on and off.  Don't combine with fields that auto-toggle ~Adam 
	[SerializeField] private int mCurrentToggleField = 0;  //Which force field to toggle on.  If it's set to -1 the toggling sequence stops. ~Adam
	[SerializeField] private float mToggleSequenceInterval = 10f;
	[SerializeField] private Animator mAnimator;

	[SerializeField] private SpriteRenderer mFillBarSprite;
	[SerializeField] private Transform mFillBarScaler;

	[SerializeField] private GameObject[] mOtherTogglableObjects; // For if we want to have the generator toggle ShieldKillers or EnemySpawners or something ~Adam
																	//Although I should probaby just re-format the thing as several different inheritted scripts for different object types...~Adm

	// Use this for initialization
	void Start () 
	{
		mCurrentHitPoints = mMaxHitPoints;
		if(mToggleInSequence)
		{
			StartCoroutine(StartToggleSequence());
		}
	}
	


	void OnTriggerEnter(Collider other)
	{
		PlayerBulletController playerBullet = other.GetComponent<PlayerBulletController>();
		if(playerBullet != null)
		{
			Destroy(other.gameObject);
			if(mCurrentHitPoints > 0)
			{
				TakeDamage();
			}
		}
	}

	void TakeDamage()
	{
		mCurrentHitPoints -= 1;

		//Adjust the color of the health bar ~Adam
		if(mFillBarSprite != null)
		{
			if(mCurrentHitPoints*1f > mMaxHitPoints*2f/3f)
			{
				mFillBarSprite.color = Color.green;
			}
			else if(mCurrentHitPoints*1f > mMaxHitPoints/3f)
			{
				mFillBarSprite.color = Color.yellow;
			}
			else
			{
				mFillBarSprite.color = Color.red;
			}
		}

		//If out of hit points, either destroy self or go into the damaged/repairing state ~Adam
		if(mCurrentHitPoints <= 0)
		{
			mCurrentHitPoints = 0;
			mCurrentToggleField = -1;
			TurnAllFieldsOff();
			if(mCanDie)
			{
				Destroy(this.gameObject);
			}
			else
			{
				if(mAnimator != null)
				{
					mAnimator.SetBool("GeneratorBroken",true);
				}
				StartCoroutine(SelfRepair());
			}
		}
		//Make the health bar go down.  This is last to make sure it doesn't go negative and start stretching below the machine ~Adam
		if(mFillBarScaler != null)
		{
			mFillBarScaler.localScale = new Vector3 (1f, (mCurrentHitPoints*1f)/mMaxHitPoints,1f);
		}
	}

	//Wait a while and then turn back on, going back to full health ~Adam
	IEnumerator SelfRepair()
	{
		yield return new WaitForSeconds(mRepairCooldownTime);
		mCurrentHitPoints = mMaxHitPoints;
		if(mFillBarSprite != null)
		{
			mFillBarSprite.color = Color.green;
		}
		if(mFillBarScaler != null)
		{
			mFillBarScaler.localScale = new Vector3 (1f,1f,1f);
		}

		if(mAnimator != null)
		{
			mAnimator.SetBool("GeneratorBroken",false);
		}

		if(mToggleInSequence)
		{
			mCurrentToggleField = 0;
			StartCoroutine(StartToggleSequence());
		}
		else
		{
			TurnAllFieldsOn();
		}
	}//END of SelfRepair()

	//Take into account that we might be starting a level partway through the toggle sequence ~Adam
	IEnumerator StartToggleSequence()
	{
		if(mCurrentToggleField >= 0 && mLinkedForceFields.Length > 0)
		{
			TurnAllFieldsOff();
			if(mCurrentToggleField>=mLinkedForceFields.Length)
			{
				mCurrentToggleField = 0;
			}
			mLinkedForceFields[mCurrentToggleField].TurnOn();
			yield return new WaitForSeconds(mToggleSequenceInterval);
			StartCoroutine(ToggleSequence());
		}
		else
		{
			TurnAllFieldsOff();
		}
	}//END of StartToggleSequence()

	//Turn off the previous force field and turn on the next one ~Adam
	IEnumerator ToggleSequence()
	{
		if(mCurrentToggleField >= 0 && mLinkedForceFields.Length > 0)
		{
			mLinkedForceFields[mCurrentToggleField].TurnOff();
			mCurrentToggleField++;
			if(mCurrentToggleField>=mLinkedForceFields.Length)
			{
				mCurrentToggleField = 0;
			}
			mLinkedForceFields[mCurrentToggleField].TurnOn();
			yield return new WaitForSeconds(mToggleSequenceInterval);
			StartCoroutine(ToggleSequence());
		}
		else
		{
			TurnAllFieldsOff();
		}
	}//END of ToggleSequence()

	void TurnAllFieldsOff()
	{
		foreach(ForceFieldBase linkedObject in mLinkedForceFields)
		{
			linkedObject.TurnOff();
		}
		foreach(GameObject otherLinkedObject in mOtherTogglableObjects)
		{
			otherLinkedObject.SetActive(false);
		}
	}//END of TurnAllFieldsOff()
	void TurnAllFieldsOn()
	{
		foreach(ForceFieldBase linkedObject in mLinkedForceFields)
		{
			linkedObject.TurnOn();
		}
		foreach(GameObject otherLinkedObject in mOtherTogglableObjects)
		{
			otherLinkedObject.SetActive(true);
		}
	}//END of TurnAllFieldsOn()

	//Returns both the current hitpoints and the max hitpoints in an array of length 2 ~Adam
	public int[] GetHitPoints()
	{
		int[] currentAndMaxHP = new int[]{mCurrentHitPoints,mMaxHitPoints};
		return currentAndMaxHP;
	}

}

