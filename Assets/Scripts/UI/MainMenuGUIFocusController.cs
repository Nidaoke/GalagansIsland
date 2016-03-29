using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InControl;

public class MainMenuGUIFocusController : MonoBehaviour 
{


	public List<string> mMainMenuButtonNames = new List<string>();
	
	public int mMainMenuButtonFocus = 0;
	
	public float mUIFocusTimer = 0f;
	public ResetScore mScoreResetter;
	public GetSome mGameStarter;

	//For opening/closing the volume control menu ~Adam
	VolumeControlSliders mVolumeMenu;

	[HideInInspector] public float mStartupTimer = 0f;

	void Start()
	{
		mMainMenuButtonNames.Add("InsertCoin");//0
		mMainMenuButtonNames.Add("QuitGame");//1
		mMainMenuButtonNames.Add("StartCoOp");//2
		mMainMenuButtonNames.Add("Options");//3
//		mMainMenuButtonNames.Add("ResetStart");//4
//		mMainMenuButtonNames.Add("ResetAsk");//5
//		mMainMenuButtonNames.Add("ResetCancel");//6
//		mMainMenuButtonNames.Add("ResetConfirm");//7
		mVolumeMenu = FindObjectOfType<VolumeControlSliders>();
	}


	// Update is called once per frame
	void Update () 
	{
		if(mUIFocusTimer > 0f && Input.GetAxis ("Horizontal") == 0 && Input.GetAxis ("Vertical") == 0 && Input.GetAxis ("HorizontalP2") == 0 && Input.GetAxis ("VerticalP2") == 0)
		{
			//mUIFocusTimer -= Time.deltaTime;
			if(mStartupTimer <= 0f)
			{
				mUIFocusTimer = -1f;
			}
			else
			{
				if(Time.timeScale != 0)
				{
					mStartupTimer -= Time.deltaTime;
				}
				else
				{
					mStartupTimer -= 0.01f;
				}
			}
		}

		if ( (Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("Thrusters") || Input.GetButtonDown("FireGun") || (InputManager.ActiveDevice.Action1.IsPressed && mUIFocusTimer<=0f)) && !mVolumeMenu.mMenuOpen)
		{

			switch(mMainMenuButtonFocus)
			{
			case 0:
				if(mGameStarter.isActiveAndEnabled == true)
				{
					mGameStarter.mSuperLaser.SetActive(true);
				}
				break;
			case 1:
				if(mGameStarter.isActiveAndEnabled == true)
				{
					Application.Quit();
				}
				break;
			case 2:
				if(mGameStarter.isActiveAndEnabled == true)
				{
					FindObjectOfType<CoOpSelector>().mCoOpEnabled = true;
					Destroy (mGameStarter.mSuperLaser);
					mGameStarter.mCoOpLaser.SetActive(true);
				}
				break;
			case 3:
				if(mUIFocusTimer<=0f)
				{
					mVolumeMenu.mMenuOpen = true;
				}
				break;
				//Used to be the Reset Score Menu ~Adam
			/*case 4:
				mScoreResetter.StartScoreReset();
				break;
			case 6:
				mScoreResetter.CancelScoreReset();
				break;
			case 7:
				mScoreResetter.ConfirmScoreReset();
				break;*/
			default:
				break;
			}
			if(InputManager.ActiveDevice.Action1.IsPressed)
			{
				mUIFocusTimer+=0.2f;
			}
		}

	}


	void OnGUI()
	{
		if(!mVolumeMenu.mMenuOpen)
		{


			//Left Input ~Adam
			if( (Input.GetAxis ("Horizontal") < 0f ||Input.GetAxis ("HorizontalP2") < 0f || InputManager.ActiveDevice.DPadLeft.IsPressed) && mUIFocusTimer <= 0f )
			{
				//Move from Insert Coin to Options ~Adam
				if(mMainMenuButtonFocus == 0 && mGameStarter.isActiveAndEnabled == true)
				{
					mMainMenuButtonFocus = 3;
					ResetFocusTimer();
				}
				//Move from Start Co-Op to Options
				else if(mMainMenuButtonFocus == 2 && mGameStarter.isActiveAndEnabled == true)
				{
					mMainMenuButtonFocus = 3;
					ResetFocusTimer();
				}//Move from Quit Game to Insert Coin ~Adam
				else if (mMainMenuButtonFocus == 1)
				{
					mMainMenuButtonFocus = 0;
					ResetFocusTimer();
				}
			}

			//Right Input ~Adam
			if( (Input.GetAxis ("Horizontal") > 0f || Input.GetAxis ("HorizontalP2") > 0f || InputManager.ActiveDevice.DPadRight.IsPressed) && mUIFocusTimer <= 0f)
			{
				//Move from insert Options to Insert Coin ~Adam
				if(mMainMenuButtonFocus == 3 && mGameStarter.isActiveAndEnabled == true)
				{
					mMainMenuButtonFocus = 0;
					ResetFocusTimer();
				}
				//Move from Insert Coin to Quit Game ~Adam
				else if(mMainMenuButtonFocus == 0)
				{
					mMainMenuButtonFocus = 1;
					ResetFocusTimer();
				}
				//Move from Start Co-Op to Quit Game ~Adam
				else if(mMainMenuButtonFocus == 2)
				{
					mMainMenuButtonFocus = 1;
					ResetFocusTimer();
				}
			}


			//Move from focusing on "Insert Coin" to focusing on the CoOp Start (down) ~Adam
			else if( ( (Input.GetAxis ("Vertical") < 0f || Input.GetAxis ("VerticalP2") < 0f || InputManager.ActiveDevice.DPadDown.IsPressed) && mUIFocusTimer<= 0f && mMainMenuButtonFocus == 0) && !Application.isMobilePlatform )
			{
				mMainMenuButtonFocus = 2;
				ResetFocusTimer();
			}
			//Move from focusing on CoOpStart to focusing on the "Insert Coin" (up) ~Adam
			else if((Input.GetAxis ("Vertical") > 0f || Input.GetAxis ("VerticalP2") > 0f || InputManager.ActiveDevice.DPadUp.IsPressed) && mUIFocusTimer<= 0f && mMainMenuButtonFocus == 2)
			{
				mMainMenuButtonFocus = 0;
				ResetFocusTimer();
			}


			//Move from focusing on "Insert Coin" to focusing on score reset ~Adam
			/*else if((Input.GetAxis ("Horizontal") < 0f || InputManager.ActiveDevice.DPadLeft.IsPressed) && mUIFocusTimer<= 0f && mMainMenuButtonFocus == 0)
			{
				mMainMenuButtonFocus = 2;
				ResetFocusTimer();
			}*/

			//Move from score reset to insert coin ~Adam
			/*else if((Input.GetAxis ("Horizontal") > 0f && mUIFocusTimer <= 0f || InputManager.ActiveDevice.DPadRight.IsPressed) && mMainMenuButtonFocus == 2 && mGameStarter.isActiveAndEnabled == true)
			{
				mMainMenuButtonFocus = 0;
				mUIFocusTimer = 0.2f;
			}*/

			//Toggle between canceling/confirming the high score reset ~Adam
			/*else if((Input.GetAxis ("Horizontal") != 0f || InputManager.ActiveDevice.DPadRight.IsPressed || InputManager.ActiveDevice.DPadLeft.IsPressed) && mUIFocusTimer<= 0f && (mMainMenuButtonFocus == 5 || mMainMenuButtonFocus == 4))
			{
				if(mMainMenuButtonFocus == 4)
				{
					mMainMenuButtonFocus = 5;
					ResetFocusTimer();
				}
				else if(mMainMenuButtonFocus == 5)
				{
					mMainMenuButtonFocus = 4;
					ResetFocusTimer();
				}
				
			}*/
		}
	}//END of OnGUI()

	void ResetFocusTimer()
	{
		mUIFocusTimer = 0.25f;
	}

}
