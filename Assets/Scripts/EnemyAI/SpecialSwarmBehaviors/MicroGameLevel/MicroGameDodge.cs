using UnityEngine;
using System.Collections;

namespace MicroGameLevel
{
	public class MicroGameDodge : MonoBehaviour 
	{

		[SerializeField] private MicroGameManager mManager;
		[SerializeField] private float mGameDuration = 10f;
		ScoreManager mScoreMan;
		PlayerOneShipController mP1Ship;
		bool mP1WasShielded;
		PlayerTwoShipController mP2Ship;
		bool mP2WasShielded;
		[SerializeField] private Rigidbody2D mRigidBody;
		[SerializeField] private float mGravityDirection = -1f;
		[SerializeField] private string mInstructionMessage = "Dodge!";

		float mTimer;

		// Use this for initialization
		void Start () 
		{
			mTimer = mGameDuration;
			mScoreMan = mManager.GetScoreMan();
			if(mScoreMan.mPlayerAvatar != null)
			{
				mP1Ship = mScoreMan.mPlayerAvatar.GetComponent<PlayerOneShipController>();
				mP1WasShielded = mP1Ship.mShielded;
				mP1Ship.mShielded = false;
			}
			if(mScoreMan.mPlayer2Avatar != null)
			{
				mP2Ship = mScoreMan.mPlayer2Avatar.GetComponent<PlayerTwoShipController>();
				mP2WasShielded = mP2Ship.mShielded;
				mP2Ship.mShielded = false;
			}

			StartCoroutine(RunDodgeGame());
		}//END of Start() 

		void Update()
		{
			if(mTimer > 0f)
			{
				mManager.SetTimerText(mTimer.ToString());
			}
			else
			{
				mManager.SetTimerText("0.000000");
			}
			mTimer -= Time.deltaTime;
		}//END of Update()

		IEnumerator RunDodgeGame()
		{
			mManager.SetUpMessage(mInstructionMessage, 2f);
			yield return new WaitForSeconds(2f);

			int startLives = mScoreMan.mLivesRemaining;

			yield return new WaitForSeconds(mGameDuration);
			mRigidBody.gravityScale = mGravityDirection;
			bool wonGame = (startLives <= mScoreMan.mLivesRemaining);

			if(mP1Ship != null)
			{
				mP1Ship.mShielded = mP1WasShielded;
			}
			if(mP2Ship != null)
			{
				mP2Ship.mShielded = mP2WasShielded;
			}

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
		}//END of RunDodgeGame()

	}
}
