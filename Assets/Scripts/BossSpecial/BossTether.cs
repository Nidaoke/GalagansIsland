using UnityEngine;
using System.Collections;


public class BossTether : MonoBehaviour 
{
	public GameObject mTetheredBoss;
	[SerializeField] private GameObject[] mTethers;

	[SerializeField] private float mTimer = 0f;
	[SerializeField] private float mBreakTime;
	[SerializeField] private float mDeathTime;
	bool mHasBroken = false;

	// Use this for initialization
	void Start () 
	{

	}//END of Start()
	
	// Update is called once per frame
	void Update () 
	{
		//Increase the timer ~Adam
		mTimer += Time.deltaTime;

		//Stay attached to the boss~Adam
		transform.position = mTetheredBoss.transform.position;

		//Play the animations for breaking the chains ~Adam
		if(mTimer >= mBreakTime && !mHasBroken)
		{
			foreach(GameObject tether in mTethers)
			{
				tether.GetComponent<Animator>().enabled = true;
			}
			mHasBroken = true;
		}

		if(mTimer >= mDeathTime)
		{
			Destroy (this.gameObject);
		}

	}//END of Update()


}
