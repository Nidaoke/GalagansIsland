using UnityEngine;
using System.Collections;

//The base script for force fields that do things to objects that enter them ~Adam
//Meant to be inheritted from rather than used directly ~Adam
public class ForceFieldBase : MonoBehaviour 
{
	public enum ForceFieldState{ACTIVATING, ON, DEACTIVATING, OFF};

	[SerializeField] protected ForceFieldState mCurrentState = ForceFieldState.ACTIVATING;
	[SerializeField] protected bool mAutoToggle = false; //For toggling on and off on a timer.  Don't combine this with a ShootableGenerator on a sequence toggler ~Adam
	[SerializeField] protected float mAutoToggleOffInterval = 5f;
	[SerializeField] protected float mAutoToggleOnInterval = 10f;

	[SerializeField] protected bool mDelayedStart = false;
	[SerializeField] protected float mStartDelayTime = 8f;
	[SerializeField] protected Collider mTriggerCollider;
	[SerializeField] protected Collider mCollisionCollider;


	// Use this for initialization
	protected virtual void Start () 
	{
		
		if(mCurrentState == ForceFieldState.ACTIVATING)
		{
			TurnOn();
		}
		if(mAutoToggle)
		{
			if(mCurrentState == ForceFieldState.ACTIVATING || mCurrentState == ForceFieldState.ON)
			{
				StartCoroutine(AutoToggleOn());
			}
			else if(mCurrentState == ForceFieldState.DEACTIVATING || mCurrentState == ForceFieldState.OFF)
			{
				StartCoroutine(AutoToggleOff());
			}
		}

	}





	// Update is called once per frame
	protected virtual void Update () 
	{
	
	}

	public virtual void TurnOn()
	{
		//Play an animation to turn on, at the end of which SetForceFieldState() is called ~Adam
		//For now we'll just call it directly until we have art ~Adam
		SetForceFieldState(ForceFieldState.ON);
	}

	public virtual void TurnOff()
	{
		//Play an animation to turn off, at the end of which SetForceFieldState() is called ~Adam
		//For now we'll just call it directly until we have art ~Adam
		SetForceFieldState(ForceFieldState.OFF);
	}

	public virtual void SetForceFieldState(ForceFieldState newState)
	{
		mCurrentState = newState;
	}

	protected virtual IEnumerator AutoToggleOn()
	{
		TurnOn();
		yield return new WaitForSeconds(mAutoToggleOnInterval);
		StartCoroutine(AutoToggleOff());
	}
	protected virtual IEnumerator AutoToggleOff()
	{
		TurnOff();
		yield return new WaitForSeconds(mAutoToggleOffInterval);
		StartCoroutine(AutoToggleOn());
	}


	protected virtual void OnTriggerEnter(Collider other)
	{

	}
	protected virtual void OnTriggerStay(Collider other)
	{

	}
	protected virtual void OnTriggerExit(Collider other)
	{

	}

	protected virtual void OnCollisionEnter(Collision other)
	{

	}
	protected virtual void OnTriggerStay(Collision other)
	{

	}
	protected virtual void OnTriggerExit(Collision other)
	{

	}
}
