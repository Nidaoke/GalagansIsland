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

	//Have two collider components, one for bouncing things off with physics, and one for things passing through ~Adam
		//Currently the BulletReflectionField makes uses of both at once for different things ~Adam
	[SerializeField] protected Collider mTriggerCollider;
	[SerializeField] protected Collider mCollisionCollider;


	[SerializeField] protected SpriteRenderer mFieldGraphic;
	[SerializeField] protected Animator mAnimator;

	// Use this for initialization
	protected virtual void Start () 
	{
		if(mAnimator != null)
		{
			mAnimator.enabled = true;
		}
		switch (mCurrentState)
		{
		case ForceFieldState.ACTIVATING:
			TurnOn();
			break;
		case ForceFieldState.DEACTIVATING:
			TurnOff();
			break;
		case ForceFieldState.ON:
			TurnOn();
			break;
		case ForceFieldState.OFF:
			HideGraphic();
			TurnOff();
			break;
			default:
			break;
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

	}//END of Start()





	// Update is called once per frame
	protected virtual void Update () 
	{
	
	}//END of Update()

	public virtual void TurnOn()
	{
		//Play an animation to turn on, at the end of which SetForceFieldState() is called ~Adam
		if(mAnimator != null)
		{
			mFieldGraphic.enabled = true;
			SetForceFieldState(ForceFieldState.ACTIVATING);
			//mAnimator.Play("ForceFieldActivate");
			mAnimator.SetInteger("ActivationState",1);
		}
		//If we're missing an animation, just turn it on directly ~Adam
		else
		{
			mFieldGraphic.enabled = true;
			SetForceFieldState(ForceFieldState.ON);
		}
	}//END of TurnOn()

	public virtual void TurnOff()
	{
		//Play an animation to turn off, at the end of which SetForceFieldState() is called ~Adam
		if(mAnimator != null)
		{
			SetForceFieldState(ForceFieldState.DEACTIVATING);
			mAnimator.SetInteger("ActivationState",-1);
		}
		//If we're missing an animation, just turn it off directly ~Adam
		else
		{
			SetForceFieldState(ForceFieldState.OFF);
		}
	}//END of TurnOff()

	public virtual void HideGraphic()
	{
		mFieldGraphic.enabled = false;
	}//END of HideGraphic()

	//This is primarily for changing state at the end of an animation ~Adam
	public virtual void SetForceFieldState(ForceFieldState newState)
	{
		mCurrentState = newState;
	}//END of SetForceFieldState()

	protected virtual IEnumerator AutoToggleOn()
	{
		TurnOn();
		yield return new WaitForSeconds(mAutoToggleOnInterval);
		StartCoroutine(AutoToggleOff());
	}//END of AutoToggleOn()
	protected virtual IEnumerator AutoToggleOff()
	{
		TurnOff();
		yield return new WaitForSeconds(mAutoToggleOffInterval);
		StartCoroutine(AutoToggleOn());
	}//END of AutoToggleOff()


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
