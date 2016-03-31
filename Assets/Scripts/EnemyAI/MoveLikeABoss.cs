using UnityEngine;
using System.Collections;

//Code copied from the BossCentral script so that we can have other objects (like whole swarms) move around like the original bosses do ~Adam
//First made for the remix level where the player fights swarms that look like the original 5 bosses ~Adam

public class MoveLikeABoss : MonoBehaviour 
{
	public Vector3 mMoveTarget = new Vector3(0f,0f,-2f);
	public float mMoveSpeed = 15f;
	public float[] mBounds;

	[SerializeField] private bool mCanMove = true;
	[SerializeField] private float mInitialWait = 8f;
	// Use this for initialization
	void Start () 
	{
		if(!mCanMove)
		{
			StartCoroutine(StartDelay());
		}
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(mCanMove)
		{
			BossMovement();
		}
	}


	protected virtual void BossMovement()
	{
		transform.position = Vector3.Lerp(transform.position, mMoveTarget, mMoveSpeed*0.001f * Time.timeScale);

		if(Vector3.Distance (transform.position, mMoveTarget) < 7f && mBounds.Length >= 4)
		{
			mMoveTarget = new Vector3(Random.Range (mBounds[0],mBounds[1]), Random.Range (mBounds[2],mBounds[3]),transform.position.z);
		}
	}//END of BossMovement()

	IEnumerator StartDelay()
	{
		yield return new WaitForSeconds (mInitialWait);
		mCanMove = true;
	}
}
