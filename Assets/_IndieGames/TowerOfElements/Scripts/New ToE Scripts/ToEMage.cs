using UnityEngine;
using System.Collections;
using InControl;

namespace TowerOfElements
{
	public class ToEMage : MonoBehaviour 
	{
		public ToEManager mManager;
		public ToEGoblinSpawner mGoblinSpawner;

		[SerializeField] private AudioSource mAudioSource;
		[SerializeField] private AudioClip mEarthSpell;
		[SerializeField] private AudioClip mLightSpell;
		[SerializeField] private AudioClip mFireSpell;

		[SerializeField] private Animator mAnimator;

		#region Get rid of these bools and replace them with input checks ~Adam
		bool mPlaceholderRightGreenInput;
		bool mPlaceholderLeftGreenInput;
		bool mPlaceholderRightBlueInput;
		bool mPlaceholderLeftBlueInput;
		bool mPlaceholderRightRedInput;
		bool mPlaceholderLeftRedInput;
		#endregion

		// Use this for initialization
		void Start () 
		{
		
		}//END of Start()
		
		// Update is called once per frame
		void Update () 
		{

			//Right Green pressed
			if(Input.GetKeyDown(KeyCode.K) || InputManager.ActiveDevice.RightStickButton.WasPressed || InputManager.ActiveDevice.RightTrigger.WasPressed)
			{
				mAudioSource.PlayOneShot(mEarthSpell);
				mAnimator.Play ("MageEarth");

				transform.position = new Vector3(3.55f, 6.5f, 0f);

				if(!mManager.mGameOver && mGoblinSpawner.mGoblinsRight.Count>0)
				{
					if(mGoblinSpawner.mGoblinsRight[0].mColor == ToEGoblin.EnemyColor.green)
					{
						mGoblinSpawner.mGoblinsRight[0].GoblinDie();
					}
					//Reset combo and lose a point if you pressed the wrong color
					else
					{
						mManager.mCombo = 0;
						mManager.mMultiplier = 1;
						mManager.mScore--;
					}
				}
			}
			//Left Green pressed
			if(Input.GetKeyDown(KeyCode.S) || InputManager.ActiveDevice.LeftStickButton.WasPressed || InputManager.ActiveDevice.LeftTrigger.WasPressed)
			{
				mAudioSource.PlayOneShot(mEarthSpell);
				mAnimator.Play ("MageEarth");

				transform.position = new Vector3(-3.55f, 6.5f, 0f);

				if(!mManager.mGameOver && mGoblinSpawner.mGoblinsLeft.Count>0)
				{
					if(mGoblinSpawner.mGoblinsLeft[0].mColor == ToEGoblin.EnemyColor.green)
					{
						mGoblinSpawner.mGoblinsLeft[0].GoblinDie();
					}
					//Reset combo and lose a point if you pressed the wrong color
					else
					{
						mManager.mCombo = 0;
						mManager.mMultiplier = 1;
						mManager.mScore--;
					}
				}
			}

			//Right Blue pressed
			if(Input.GetKeyDown(KeyCode.J) || (InputManager.ActiveDevice.RightStickY.WasPressed && InputManager.ActiveDevice.RightStickY.Value > 0) )
			{
				mAudioSource.PlayOneShot(mLightSpell);
				mAnimator.Play ("MageLight");

				transform.position = new Vector3(3.55f, 6.5f, 0f);

				if(!mManager.mGameOver && mGoblinSpawner.mGoblinsRight.Count>0)
				{
					if(mGoblinSpawner.mGoblinsRight[0].mColor == ToEGoblin.EnemyColor.blue)
					{
						mGoblinSpawner.mGoblinsRight[0].GoblinDie();
					}
					//Reset combo and lose a point if you pressed the wrong color
					else
					{
						mManager.mCombo = 0;
						mManager.mMultiplier = 1;
						mManager.mScore--;
					}
				}
			}
			//Left Blue pressed
			if(Input.GetKeyDown(KeyCode.D) || (InputManager.ActiveDevice.LeftStickY.WasPressed && InputManager.ActiveDevice.LeftStickY.Value > 0) )
			{
				mAudioSource.PlayOneShot(mLightSpell);
				mAnimator.Play ("MageLight");

				transform.position = new Vector3(-3.55f, 6.5f, 0f);

				if(!mManager.mGameOver && mGoblinSpawner.mGoblinsLeft.Count>0)
				{
					if(mGoblinSpawner.mGoblinsLeft[0].mColor == ToEGoblin.EnemyColor.blue)
					{
						mGoblinSpawner.mGoblinsLeft[0].GoblinDie();
					}
					//Reset combo and lose a point if you pressed the wrong color
					else
					{
						mManager.mCombo = 0;
						mManager.mMultiplier = 1;
						mManager.mScore--;
					}
				}
			}

			//Right Red pressed
			if(Input.GetKeyDown(KeyCode.L) || (InputManager.ActiveDevice.RightStickY.WasPressed && InputManager.ActiveDevice.RightStickY.Value < 0) )
			{
				mAudioSource.PlayOneShot(mFireSpell);
				mAnimator.Play ("MageFire");
				
				transform.position = new Vector3(3.55f, 6.5f, 0f);

				if(!mManager.mGameOver && mGoblinSpawner.mGoblinsRight.Count>0)
				{
					if(mGoblinSpawner.mGoblinsRight[0].mColor == ToEGoblin.EnemyColor.red)
					{
						mGoblinSpawner.mGoblinsRight[0].GoblinDie();
					}
					//Reset combo and lose a point if you pressed the wrong color
					else
					{
						mManager.mCombo = 0;
						mManager.mMultiplier = 1;
						mManager.mScore--;
					}
				}
			}
			//Left Red pressed
			if(Input.GetKeyDown(KeyCode.D) || (InputManager.ActiveDevice.LeftStickY.WasPressed && InputManager.ActiveDevice.LeftStickY.Value < 0) )
			{
				mAudioSource.PlayOneShot(mFireSpell);
				mAnimator.Play ("MageFire");
				
				transform.position = new Vector3(-3.55f, 6.5f, 0f);

				if(!mManager.mGameOver && mGoblinSpawner.mGoblinsLeft.Count>0)
				{
					if(mGoblinSpawner.mGoblinsLeft[0].mColor == ToEGoblin.EnemyColor.red)
					{
						mGoblinSpawner.mGoblinsLeft[0].GoblinDie();
					}
					//Reset combo and lose a point if you pressed the wrong color
					else
					{
						mManager.mCombo = 0;
						mManager.mMultiplier = 1;
						mManager.mScore--;
					}
				}
			}

		}//END of Update()
	}
}
