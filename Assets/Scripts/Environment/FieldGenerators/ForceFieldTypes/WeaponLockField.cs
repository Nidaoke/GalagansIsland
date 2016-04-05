using UnityEngine;
using System.Collections;

public class WeaponLockField : ForceFieldBase 
{

	void OnTriggerStay(Collider other)
	{
		PlayerShipController playerShip = other.GetComponent<PlayerShipController>();
		if(playerShip != null)
		{
			if(playerShip.heatLevel<=0.1f)
			{
				playerShip.heatLevel = 0.1f;
			}
			playerShip.mToggleFireOn = false;
			playerShip.isOverheated = true;
		}
	}
	void OnTriggerExit(Collider other)
	{
		PlayerShipController playerShip = other.GetComponent<PlayerShipController>();
		if(playerShip != null)
		{
			playerShip.isOverheated = false;
		}
	}
}
