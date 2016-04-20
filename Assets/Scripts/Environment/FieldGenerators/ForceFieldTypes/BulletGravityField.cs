using UnityEngine;
using System.Collections;

//Adjusts the rigidbody velocities of player bullets that enter to alter their trajectories ~Adam

public class BulletGravityField : ForceFieldBase 
{
	[SerializeField] private float mGravityPullStrength = -100f; //Negative for pulling to the center, positive for pushing outward ~Adam

	// Use this for initialization
	void Start () 
	{
		base.Start();
	}//END of Start
	



	void OnTriggerEnter(Collider other)
	{
		//Need a this.enabled check because Unity's OnTrigger and OnCollision functions get called even when a script is disabled ~Adam		if(mCurrentState == ForceFieldState.ON && this.enabled)  
		if(mCurrentState == ForceFieldState.ON && this.enabled)
		{
			PlayerBulletController playerBullet = other.GetComponent<PlayerBulletController>();
			if(playerBullet != null)
			{
				Rigidbody bulletRigidbody = other.GetComponent<Rigidbody>();
				Vector3 forcePull = Vector3.Normalize(other.transform.position - this.transform.position)*mGravityPullStrength;

				bulletRigidbody.AddForce(forcePull);
				//Make sure the bullet is facing the same way it's going ~Adam
				bulletRigidbody.gameObject.transform.up = bulletRigidbody.velocity;
			}
		}
	}//END of OnTriggerEnter()
	void OnTriggerStay(Collider other)
	{
		if(mCurrentState == ForceFieldState.ON && this.enabled)
		{
			PlayerBulletController playerBullet = other.GetComponent<PlayerBulletController>();
			if(playerBullet != null)
			{
				Rigidbody bulletRigidbody = other.GetComponent<Rigidbody>();
				Vector3 forcePull = Vector3.Normalize(other.transform.position - this.transform.position)*mGravityPullStrength;

				bulletRigidbody.AddForce(forcePull);
				//Make sure the bullet is facing the same way it's going ~Adam
				bulletRigidbody.gameObject.transform.up = bulletRigidbody.velocity;
			}
		}
	}//END of OnTriggerStay
}
