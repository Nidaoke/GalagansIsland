using UnityEngine;
using System.Collections;

namespace MicroGameLevel
{
public class MicroGameDontShoot : MonoBehaviour 
	{

		[SerializeField] private MicroGameManager mManager;
		[SerializeField] private float mGameDuration = 10f;
		[SerializeField] private Rigidbody2D mRigidBody;
		[SerializeField] private float mGravityDirection = -1f;
		[SerializeField] private string mInstructionMessage = "Don't Shoot!";
		ScoreManager mScoreMan;
		PlayerOneShipController mP1Ship;
		PlayerTwoShipController mP2Ship;

		bool mGameRunning = false;
		[SerializeField] private bool mWeaponFired = false;

		float mTimer;

		// Use this for initialization
		void Start () 
		{
			mTimer = mGameDuration;
			mScoreMan = mManager.GetScoreMan();
			if(mScoreMan.mPlayerAvatar != null)
			{
			mP1Ship = mScoreMan.mPlayerAvatar.GetComponent<PlayerOneShipController>();
			}
			if(mScoreMan.mPlayer2Avatar != null)
			{
				mP2Ship = mScoreMan.mPlayer2Avatar.GetComponent<PlayerTwoShipController>();
			}
			StartCoroutine(RunDontShootGame());
		}//END of Start() 

		void Update()
		{
			if(mGameRunning)
			{
				if(mP1Ship != null && mP1Ship.mToggleFireOn)
				{
					Debug.Log(mP1Ship.gameObject.name + "'s toggle fire is " + mP1Ship.mToggleFireOn);
					mWeaponFired = true;
				}
				if(mP2Ship != null && mP2Ship.mToggleFireOn)
				{
					Debug.Log(mP2Ship.gameObject.name + "'s toggle fire is " + mP2Ship.mToggleFireOn);
					mWeaponFired = true;
				}

				if(mTimer > 0f)
				{
					mManager.SetTimerText(mTimer.ToString());
				}
				else
				{
					mManager.SetTimerText("0.000000");
				}
				mTimer -= Time.deltaTime;
			}

		}//END of Update()



		IEnumerator RunDontShootGame()
		{
			mManager.SetUpMessage(mInstructionMessage, 2f);
			yield return new WaitForSeconds(2f);
			mGameRunning = true;
			yield return new WaitForSeconds(mGameDuration);
			mRigidBody.gravityScale = mGravityDirection;
			bool wonGame = !mWeaponFired;



			if(wonGame)
			{
				mManager.RewardPlayer();
			}
			else
			{
				mManager.PunishPlayer();
			}
			yield return new WaitForSeconds(3f);
			Destroy(this.gameObject);
		}//END of RunDontShootGame()

	}
}
