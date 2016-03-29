using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TowerOfElements
{
	public class ToEGoblin : MonoBehaviour 
	{
		public enum EnemyColor { red, green, blue}

		public EnemyColor mColor;
		public ToEManager mManager;
		public ToEGoblinSpawner mGoblinSpawner;
		public bool mRightRope;
		public GameObject mDeathEffect;
		public float mSpeed = 1f;

		// Use this for initialization
		void Start () 
		{
			
		}//END of Start()
		
		// Update is called once per frame
		void Update () 
		{
			//Climb up the rope when it's not Game Over ~Adam
			if(!mManager.mGameOver)
			{
				transform.Translate(0, mSpeed * Time.deltaTime, 0);

				//End the game if this reaches the top ~Adam
				if (gameObject.transform.position.y > 3.5f)
				{
					mManager.mGameOver = true;
				}
			}
		}//END of Update()


		public void GoblinDie()
		{
			//Spawn death effect
			Instantiate (mDeathEffect,transform.position,Quaternion.identity);

			//Alter the combo and score
			mManager.IncreaseCombo();
			mManager.mScore += mManager.mMultiplier;
			if(mRightRope)
			{
				mGoblinSpawner.mGoblinsRight.Remove(this);
			}
			else
			{
				mGoblinSpawner.mGoblinsLeft.Remove(this);
			}
			Destroy (this.gameObject);
		}//END of GoblinDie()
	}
}