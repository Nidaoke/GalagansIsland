using UnityEngine;
using System.Collections;

//When player bullets enter this field, they get reversed in direction and become capable of hurting the player ~Adam

public class BulletReflectionField : ForceFieldBase 
{
	[SerializeField] private GameObject mReflectedBullet;
	[SerializeField] private bool mP1ShipInside = false;
	[SerializeField] private bool mP2ShipInside = false;
	


	// Use this for initialization
	void Start () 
	{
		base.Start();
	}

	// Update is called once per frame
	void Update () 
	{

	}


	void OnTriggerEnter(Collider other)
	{
		Debug.Log(other.gameObject.name + "Entered the force field");


		if(other.GetComponent<PlayerShipController>() != null)
		{
			Debug.Log("Player ship is inside Reflection Field!");
			mP1ShipInside = true;
			if(mCollisionCollider!= null)
			{
				mCollisionCollider.enabled = false;
			}
		}
//		if(mCurrentState == ForceFieldState.ON)
//		{
//			PlayerBulletController playerBullet = other.gameObject.GetComponent<PlayerBulletController>();
//			if(playerBullet != null)
//			{
//				if(!mP1ShipInside)
//				{
//				Transform newBulletTransform = other.transform;
//				newBulletTransform.Rotate(0f,0f,180f);
//				Destroy(other.gameObject);
//				GameObject reflectedBullet;
//				reflectedBullet = Instantiate(mReflectedBullet, newBulletTransform.position, newBulletTransform.rotation) as GameObject;
//				}
//			}
//		}
	}

	void OnTriggerExit(Collider other)
	{
		Debug.Log(other.gameObject.name + "Exited the force field");


		if(other.GetComponent<PlayerShipController>() != null)
		{
			Debug.Log("Player ship is outside Reflection Field!");
			mP1ShipInside = false;
			mCollisionCollider.enabled = true;
		}
	}

	void OnCollisionEnter(Collision other)
	{
		Debug.Log(other.gameObject.name + "collided with" + this.gameObject.name);
		if(mCurrentState == ForceFieldState.ON)
		{
			if(other.gameObject.GetComponent<PlayerShipController>() != null)
			{
				Debug.Log("Player ship is inside Reflection Field!");
				mP1ShipInside = true;
			}
			PlayerBulletController playerBullet = other.gameObject.GetComponent<PlayerBulletController>();
			if(playerBullet != null)
			{
				if(!mP1ShipInside)
				{
					Transform newBulletTransform = other.transform;
					newBulletTransform.Rotate(0f,0f,180f);
					Destroy(other.gameObject);
					GameObject reflectedBullet;
					reflectedBullet = Instantiate(mReflectedBullet, newBulletTransform.position, newBulletTransform.rotation) as GameObject;
				}
			}
		}
	}

	void OnCollisionExit(Collision other)
	{
		Debug.Log(other.gameObject.name + "collided with" + this.gameObject.name);
		if(mCurrentState == ForceFieldState.ON)
		{
			if(other.gameObject.GetComponent<PlayerShipController>() != null)
			{
				Debug.Log("Player ship is outside Reflection Field!");
				mP1ShipInside = false;
			}
		}
	}
}
