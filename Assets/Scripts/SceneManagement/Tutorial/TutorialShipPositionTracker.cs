using UnityEngine;
using System.Collections;

public class TutorialShipPositionTracker : MonoBehaviour 
{
	[SerializeField] private GameObject mP1Ship;
	[SerializeField] private Vector3 mShipPos = Vector3.zero;

	
	// Update is called once per frame
	void Update () 
	{
		if(mP1Ship == null)
		{
			if(FindObjectOfType<PlayerOneShipController>()!=null)
			{
				mP1Ship = FindObjectOfType<PlayerOneShipController>().gameObject;
			}
		}

		if(mP1Ship != null)
		{
			if(Application.loadedLevelName == "Tutorial")
			{
				mShipPos = mP1Ship.transform.position;
			}
			else if (Application.loadedLevel == 1)
			{
				mP1Ship.transform.position = mShipPos;
				Debug.Log ("Repositioned Ship");
				Destroy (this.gameObject);
			}
		}
		if(Application.loadedLevelName != "Tutorial" && Application.loadedLevel != 1)
		{
			Debug.Log ("Dying because wrong scene");
			Destroy (this.gameObject);
		}

	}
}
