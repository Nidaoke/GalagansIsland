using UnityEngine;
using System.Collections;

public class LDGameEnder : MonoBehaviour 
{

	float mTimer = 5f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		mTimer-= Time.deltaTime;
		if(mTimer<= Time.deltaTime)
		{
			if(FindObjectOfType<LDHeroShipAI>()!= null)
			{
				Destroy(FindObjectOfType<LDHeroShipAI>().gameObject);
			}
			if(FindObjectOfType<LDBossGenericScript>()!= null)
			{
				Destroy(FindObjectOfType<LDBossGenericScript>().gameObject);
			}
			Application.LoadLevel (0);
		}
	}
}
