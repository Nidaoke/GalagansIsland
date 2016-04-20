using UnityEngine;
using System.Collections;

//When player bullets enter this field, they get reversed in direction and become capable of hurting the player ~Adam

//I need to update this for handling two players ~Adam

public class BulletReflectionField : ForceFieldBase 
{
	[SerializeField] private GameObject mReflectedBullet;
	[SerializeField] private bool mP1ShipInside = false;
	[SerializeField] private bool mP2ShipInside = false;
	


	// Use this for initialization
	void Start () 
	{
		base.Start();
	}//END of Start()

	void Update()
	{
		//Toggle between using the collsion collider (more accurate defleciton) or the trigger collider (works better on two-player) ~Adam
		//Also, make sure that the collision is disabled so bullets can pass through when the field is inactive ~Adam
		if (mCurrentState == ForceFieldState.ON && !(mP1ShipInside || mP2ShipInside) )
		{
			mCollisionCollider.enabled = true;
		}else
		{
			mCollisionCollider.enabled = false;
		}

	}//END of Update

	void OnTriggerEnter(Collider other)
	{

		if(mCurrentState == ForceFieldState.ON && this.enabled)
		{
			if(mP1ShipInside || mP2ShipInside)
			{
			
				PlayerBulletController playerBullet = other.gameObject.GetComponent<PlayerBulletController>();
				if(playerBullet != null &&
					!(mP1ShipInside && playerBullet.mPlayerBulletNumber == 1) && 
					!(mP2ShipInside && playerBullet.mPlayerBulletNumber == 2) )
				{
					Transform newBulletTransform = other.transform;
					newBulletTransform.Rotate(0f,0f,180f);
					Destroy(other.gameObject);
					GameObject reflectedBullet;
					reflectedBullet = Instantiate(mReflectedBullet, newBulletTransform.position, newBulletTransform.rotation) as GameObject;
				}
			}
		}
		if(other.GetComponent<PlayerOneShipController>() != null)
		{
			mP1ShipInside = true;
		}
		else if(other.GetComponent<PlayerTwoShipController>() != null)
		{
			mP2ShipInside = true;
		}
	}//END of OnTriggerEnter()

	void OnTriggerExit(Collider other)
	{
		if(other.GetComponent<PlayerOneShipController>() != null)
		{
			mP1ShipInside = false;
		}
		else if(other.GetComponent<PlayerTwoShipController>() != null)
		{
			mP2ShipInside = false;
		}
	}//END of OnTriggerExit()

	void OnCollisionEnter(Collision other)
	{
		if(other.gameObject.GetComponent<PlayerShipController>() != null)
		{
			mP1ShipInside = true;
		}
		//Need a this.enabled check because Unity's OnTrigger and OnCollision functions get called even when a script is disabled ~Adam
			//But we still want to track the player's entering and leaving ~Adam
		if(mCurrentState == ForceFieldState.ON && this.enabled)
		{
			PlayerBulletController playerBullet = other.gameObject.GetComponent<PlayerBulletController>();
			if(playerBullet != null)
			{
				Transform newBulletTransform = other.transform;
				newBulletTransform.Rotate(0f,0f,180f);
				Destroy(other.gameObject);
				GameObject reflectedBullet;
				reflectedBullet = Instantiate(mReflectedBullet, newBulletTransform.position, newBulletTransform.rotation) as GameObject;
			}
		}
	}//END of OnCollisionEnter()

	void OnCollisionExit(Collision other)
	{
		if(other.gameObject.GetComponent<PlayerShipController>() != null)
		{
			mP1ShipInside = false;
		}
	}//END of OnCollisionExit

	//Make it so that bullets don't keep colliding with the field when it's turned off ~Adam
	public override void TurnOn()
	{
		base.TurnOn();
		mCollisionCollider.enabled = true;
	}//END of TurnOn()

	public override void TurnOff()
	{
		base.TurnOff();
		mCollisionCollider.enabled = false;
	}//END of TurnOff()

	public override void SetForceFieldState(ForceFieldState newState)
	{
		base.SetForceFieldState(newState);
		if(newState == ForceFieldState.ON)
		{
			mCollisionCollider.enabled = true;
		}
		else
		{
			mCollisionCollider.enabled = false;
		}
	}//END of SetForceFieldState()
}
