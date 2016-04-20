using UnityEngine;
using System.Collections;

namespace MicroGameLevel
{
	public class MicroGameShootColor : MonoBehaviour 
	{

		[SerializeField] private MicroGameManager mManager;
		[SerializeField] private float mGameDuration = 10f;
		ScoreManager mScoreMan;

		[SerializeField] private GameObject[] mShipsToShoot;
		[SerializeField] private GameObject[] mShipsToAvoid;
		[SerializeField] private Rigidbody2D mRigidBody;
		[SerializeField] private float mGravityDirection = -1f;
		[SerializeField] private string mInstructionMessage = "Shoot Only Red!";
		[SerializeField] ForceFieldBase mWeaponLockField;

		float mTimer;

		// Use this for initialization
		void Start () 
		{
			mTimer = mGameDuration;
			StartCoroutine(ShootColorGame());
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


		IEnumerator ShootColorGame()
		{
			mWeaponLockField.TurnOn();
			mManager.SetUpMessage(mInstructionMessage, 2f);
			yield return new WaitForSeconds(2f);
			mWeaponLockField.TurnOff();


			yield return new WaitForSeconds(mGameDuration);
			mWeaponLockField.TurnOn();

			yield return new WaitForSeconds(1f);

			mRigidBody.gravityScale = mGravityDirection;

			bool shotRightColor = true;
			bool avoidedWrongColor = true;

			foreach (GameObject coloredShip in mShipsToShoot)
			{
				if(!(coloredShip == null))
				{
					shotRightColor = false;
					Debug.Log("A ship to shoot was not destroyed");
				}
			}
			foreach (GameObject uncoloredShip in mShipsToAvoid)
			{
				if(uncoloredShip == null)
				{
					avoidedWrongColor = false;
					Debug.Log("A ship to avoid was destroyed");
				}
			}
			Debug.Log(shotRightColor + ", "+avoidedWrongColor);
			bool wonGame = (shotRightColor && avoidedWrongColor);

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
		}//END of ShootColorGame()
	}
}