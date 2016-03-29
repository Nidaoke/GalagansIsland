using UnityEngine;
using System.Collections;

public class LDBoss1 : LDBossGenericScript {

	public bool leftHornAlive = true;
	public bool rightHornAlive = true;
	
	public SpriteRenderer spriter;
	
	public Sprite rightHorn;
	public Sprite leftHorn;
	public Sprite noHorns;

	public override void Start ()
	{
		spriter = GetComponent<SpriteRenderer> ();
		base.Start ();
	}

	public override void Update ()
	{
		if (!leftHornAlive) 
		{
			mBounds[0] = -13f;
		}
		if (!rightHornAlive) 
		{
			mBounds[1] = 13f;
		}


		base.Update ();
	}
}
