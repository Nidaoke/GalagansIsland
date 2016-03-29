﻿using UnityEngine;
using System.Collections;
using InControl;
using XInputDotNetPure;
using UnityEngine.UI;

public class LDBossDeathWeapon : MonoBehaviour 
{

	public GameObject player;
	public bool ram;

	public LDBossGenericScript mBossCentral;
	public int mHealthThreshHold = 30;
	public GameObject mDeathWeapon;
	public float mOverheatSpeed = 2.0f;

	public float mRamSpeed = 3.0f;
	public float mRamDistance = 5.0f;
	Image mButtonIcon;
	// Use this for initialization
	void Start () 
	{
		player = GameObject.FindGameObjectWithTag ("Player");

		mButtonIcon = GameObject.Find("Death Weapon Icon").GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (player == null) {

			player = GameObject.FindGameObjectWithTag ("Player");
		}

		//Turn weapon off if overheated or dead ~Adam
		if (mBossCentral.mOverheated || mBossCentral.mDying) {
			mDeathWeapon.SetActive (false);
			mButtonIcon.enabled = false;
		}
		//Do stuff when below the Health Threshhold ~Adam
		else if (mBossCentral.mCurrentHealth <= mHealthThreshHold) {
			//Turn on the UI Icon for Right Trigger ~Adam
			mButtonIcon.enabled = true;

			//Fire weapon with Right Trigger button ~Adam
			if (InputManager.ActiveDevice.RightTrigger.IsPressed) {
				mDeathWeapon.SetActive (true);
				mBossCentral.mCurrentOverheat += Time.deltaTime * mOverheatSpeed;

				if (ram) {

					transform.position = Vector3.MoveTowards (transform.position, player.transform.position+transform.up*mRamDistance, mRamSpeed);
				}
			}
			//Turn Off Weapon when Right Trigger is released ~Adam
			else {
				mDeathWeapon.SetActive (false);
				mBossCentral.mCurrentOverheat -= Time.deltaTime;
			}
		} else {

			mButtonIcon.enabled = false;
		}

	}//END of Update()
}
