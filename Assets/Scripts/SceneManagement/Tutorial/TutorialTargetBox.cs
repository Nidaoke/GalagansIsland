using UnityEngine;
using System.Collections;

//For boxes the player ship has to fly into on the tutorial ~Adam
public class TutorialTargetBox : MonoBehaviour 
{
	[SerializeField] private TutorialController mTutCon;
	public bool mCleared = false;
	[SerializeField] bool mRequiresHover = false;
	[SerializeField] float mHoverTimer = 3f;
	bool mHoveringInside = false;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		//slowly go green while being hovered inside ~Adam
		if(mHoveringInside)
		{
			mHoverTimer -= Time.deltaTime;
			GetComponent<Light>().color = Color.Lerp (GetComponent<Light>().color, Color.green, 0.006f);

			if(mHoverTimer <=0f)
			{
				mCleared = true;
			}
		}

		//Turn Green when cleared ~Adam
		if(mCleared)
		{
			GetComponent<Light>().color = Color.green;
		}
	}

	//Get cleared on Enter if not requiring hover ~Adam
	void OnTriggerEnter(Collider other)
	{
		if(other.GetComponent<PlayerShipController>() != null && !mRequiresHover)
		{
			mCleared = true;
		}
	}

	//Check whether or not it's being hovered inside ~Adam
	void OnTriggerStay(Collider other)
	{
		if(other.GetComponent<PlayerShipController>() != null && mRequiresHover)
		{
			if(other.GetComponent<PlayerShipController>().isHovering)
			{
				mHoveringInside = true;
			}
			else
			{
				mHoveringInside = false;
			}
		}
	}

	//Check when it is left and register that nothing is hovering inside anymore~Adam
	void OnTriggerExit(Collider other)
	{
		if(other.GetComponent<PlayerShipController>())
		{
			mHoveringInside = false;
		}
	}

}
