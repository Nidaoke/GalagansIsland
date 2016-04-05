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


	// Use this for initialization
	void Start () 
	{
		mCurrentHitPoints = mMaxHitPoints;
		if(mToggleInSequence)
		{
			StartCoroutine(StartToggleSequence());
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
	
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
		if(mCurrentHitPoints <= 0)
		{
			mCurrentToggleField = -1;
			TurnAllFieldsOff();
			if(mCanDie)
			{
				Destroy(this.gameObject);
			}
			else
			{
				StartCoroutine(SelfRepair());
			}
		}
	}

	IEnumerator SelfRepair()
	{
		yield return new WaitForSeconds(mRepairCooldownTime);
		mCurrentHitPoints = mMaxHitPoints;

		if(mToggleInSequence)
		{
			mCurrentToggleField = 0;
			StartCoroutine(StartToggleSequence());
		}
		else
		{
			TurnAllFieldsOn();
		}
	}

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
	}

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
	}

	void TurnAllFieldsOff()
	{
		foreach(ForceFieldBase linkedObject in mLinkedForceFields)
		{
			linkedObject.TurnOff();
		}
	}
	void TurnAllFieldsOn()
	{
		foreach(ForceFieldBase linkedObject in mLinkedForceFields)
		{
			linkedObject.TurnOn();
		}
	}
}

