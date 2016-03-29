using UnityEngine;
using System.Collections;

public class FlappyPipe : MonoBehaviour 
{

	[SerializeField] private float mLeftBound = -34f;
	[SerializeField] private float mRightBound = 34f;
	[SerializeField] private float mBottomBound = -20f;
	[SerializeField] private float mTopBound = 20f;
	[SerializeField] private float mSpeed = 5f;



	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.Translate(Vector3.left*mSpeed*Time.deltaTime);

		if(transform.position.x <= mLeftBound)
		{
			transform.position = new Vector3(mRightBound, Random.Range(mBottomBound,mTopBound), transform.position.z);
		}
	}
}
