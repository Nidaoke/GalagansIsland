using UnityEngine;
using System.Collections;

public class Snowfall : MonoBehaviour 
{
	[SerializeField] private float mTopPos = 38f;
	[SerializeField] private float mBottomPos = -48f;
	[SerializeField] private float mFallSpeed = 5f;

	[SerializeField] private float mStartTime = 10f;
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Delay falling to stagger out the snowflakes and ensure the swarm is built first ~Adam
		if(mStartTime >=0f)
		{
			mStartTime -= Time.deltaTime;
		}
		else
		{
			//Fall ~Adam
			transform.position += Vector3.down*mFallSpeed*Time.deltaTime;
			//Teleport back to the top once off the bottom of the screen ~Adam
			if(transform.position.y <= mBottomPos)
			{
				transform.position = new Vector3(transform.position.x, mTopPos, transform.position.z);
			}
		}
	}
}
