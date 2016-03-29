using UnityEngine;
using System.Collections;

//Use this as the parts of the bosss that are meant to be shot by the player.   ~Adam
//This script is meant to be inherited from ~Adam

public class BossWeakPoint : MonoBehaviour 
{
	public BossCentral mBossCentral;
	public int mHitPonts;
	[SerializeField] protected bool mDestroyObjectOnDeath = true;
	public SpriteRenderer mDamageSprite;
	public GameObject mDestrucitonEffect;
	[SerializeField] protected Transform mDestructionPoint;
	public bool mActiveWeakPoint = true;
	// Use this for initialization
	protected virtual void Start () 
	{
		mBossCentral.mCurrentHealth += mHitPonts;
		mBossCentral.mTotalHealth += mHitPonts;
	}//END of Start()
	
	// Update is called once per frame
	protected virtual void Update () 
	{

		//Return color to white in between hits ~Adam
		if(mDamageSprite != null)
		{
			mDamageSprite.color = Color.Lerp (mDamageSprite.color, Color.white, 0.1f);
		}

	}//END of Update()


	protected virtual void OnTriggerEnter(Collider other)
	{

		if(other.GetComponent<PlayerBulletController>()!=null)
		{
			//Do damage all the time for main bullets an 5% of the time for side bullets ~Adam
			if(!other.GetComponent<PlayerBulletController>().mSideBullet || Random.value <= 0.05f)
			{
				TakeDamage ();
			}
			//Always at least flash like it got hit ~Adam
			else
			{
				if(mDamageSprite != null)
				{
					mDamageSprite.color = Color.Lerp (mDamageSprite.color, Color.red, 1f);
				}
			}
			Destroy (other.gameObject);

		}
	}

	public virtual void TakeDamage()
	{
		//Subtract health from this individual part ~Adam
		mHitPonts --;
		//Subtract health from main boss ~Adam
		if(mBossCentral.mCurrentHealth >0)
		{
			mBossCentral.mCurrentHealth--;
		}
		if(mBossCentral.mCurrentHealth <0)
		{
			mBossCentral.mCurrentHealth = 0;
		}

		if(mDamageSprite != null)
		{
			mDamageSprite.color = Color.Lerp (mDamageSprite.color, Color.red, 1f);
		}

		//Destroy this weakpoint when it runs out of hit points ~Adam
		if(mHitPonts <= 0)
		{
			WeakPointDestruction();
		}

	}//END of TakeDamage()

	protected virtual void WeakPointDestruction()
	{
		//Spawn death effect ~Adam
		if(mDestrucitonEffect != null)
		{
			GameObject destructionEffect = Instantiate (mDestrucitonEffect,transform.position,Quaternion.identity) as GameObject;
			if(mDestructionPoint != null)
			{
				destructionEffect.transform.position = mDestructionPoint.position;
			}
		}
		//Either destroy this game object or just turn off ~Adam
		if(mDestroyObjectOnDeath)
		{
			Destroy (this.gameObject);
		}
		else
		{
			enabled = false;
		}
	}//END of WeakPointDestrcution()
}
