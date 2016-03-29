using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using InControl;

public class CheckpointLoadMenu : MonoBehaviour 
{

	//For what part of the menu is being highlighted ~Adam

	[SerializeField] private Text mYesText;
	[SerializeField] private Text mNoText;


	
	[SerializeField] private Color mNormalColor;
	[SerializeField] private Color mFocusColor;
	public bool mUIFocus = false;
	public float mUIFocusTimer = 0.2f;

	// Use this for initialization
	void Start () 
	{



	}
	
	// Update is called once per frame
	void Update () 
	{


		if(mUIFocusTimer > 0f)
		{
			
			mUIFocusTimer -= 0.01f;
			
		}

		else if(mUIFocusTimer <= 0f)
		{
			if (Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("Thrusters") || Input.GetButtonDown("FireGun") || (InputManager.ActiveDevice.Action1.IsPressed))
			{
				if(Application.loadedLevelName != "EndGame")
				{
					if(mUIFocus)
					{
						Application.LoadLevel(PlayerPrefs.GetInt ("CheckPointedLevel") );
					}
					else
					{
						PlayerPrefs.SetInt ("CheckPointedLevel", 0);

						Application.LoadLevel(1);
					}
				}
				else
				{
					if(mUIFocus)
					{
						if(FindObjectOfType<EndGame>()!= null)
						{
							FindObjectOfType<EndGame>().ReloadGame();
						}
					}
					else
					{
						PlayerPrefs.SetInt ("CheckPointedLevel", 0);
						if(FindObjectOfType<EndGame>()!= null)
						{
							FindObjectOfType<EndGame>().ReloadGame();
						}
					}
				}
			}

			//Choose between Yes or No (Left/Right Input)~Adam
			if(Input.GetAxisRaw ("Horizontal") != 0 || InputManager.ActiveDevice.DPadLeft.WasPressed|| InputManager.ActiveDevice.DPadRight.WasPressed)
			{
				mUIFocus = !mUIFocus;
				mUIFocusTimer = 0.25f;
			}										
		}

		if(mUIFocus)
		{
			mNoText.color = mNormalColor;
			mYesText.color = mFocusColor;
		}
		else
		{
			mNoText.color = mFocusColor;
			mYesText.color = mNormalColor;
		}
	
	}//END of Update()
	

	
	
	void ResetFocusTimer()
	{
		mUIFocusTimer = 0.25f;
	}
}
