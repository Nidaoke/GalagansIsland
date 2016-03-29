using UnityEngine;
using System.Collections;

public class LDBlobWeakPoint : LDBossWeakPoint 
{
	public GameObject mBossBody;
	public SpriteRenderer mMainBodySprite;




	public override void TakeDamage()
	{
		

		mBossBody.GetComponent<LDBlobBoss>().mhealth --;
		base.TakeDamage ();
		//For flashing when hit ~Adam
		if(mMainBodySprite != null)
		{
			mMainBodySprite.color = Color.Lerp (mMainBodySprite.color, Color.red, 1f);
		}
	
		

		
	}
}
