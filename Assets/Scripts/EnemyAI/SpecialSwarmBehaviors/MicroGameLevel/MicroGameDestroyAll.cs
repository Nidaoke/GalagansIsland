using UnityEngine;
using System.Collections;

namespace MicroGameLevel
{
	public class MicroGameDestroyAll : MonoBehaviour 
	{
		[SerializeField] private MicroGameManager mManager;
		[SerializeField] private float mGameDuration = 10f;
		[SerializeField] private Rigidbody2D mRigidBody;
		[SerializeField] private float mGravityDirection = -1f;
		[SerializeField] ForceFieldBase mWeaponLockField;
		[SerializeField] private string mInstructionMessage = "Destroy Everything!";
		float mTimer;

		// Use this for initialization
		void Start () 
		{
			mTimer = mGameDuration;
			StartCoroutine(RunDestroyAllGame());
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

		IEnumerator RunDestroyAllGame()
		{
			mManager.SetUpMessage(mInstructionMessage, 2f);
			yield return new WaitForSeconds(2f);

			yield return new WaitForSeconds(mGameDuration);
			mWeaponLockField.TurnOn();
			yield return new WaitForSeconds(1f);
			mRigidBody.gravityScale = mGravityDirection;

			bool wonGame = !FindObjectOfType<LevelKillCounter>().mRemainingEnemy;



			if(wonGame)
			{
				mManager.RewardPlayer();
			}
			else
			{
				mManager.PunishPlayer();
			}
			yield return new WaitForSeconds(3f);
			mWeaponLockField.TurnOff();
			Destroy(this.gameObject);
		}//END of RunDestroyAllGame()

	}
}
