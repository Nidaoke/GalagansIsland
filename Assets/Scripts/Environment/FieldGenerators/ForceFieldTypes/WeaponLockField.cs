using UnityEngine;
using System.Collections;

//The player can't fire their main gun while in this field, getting stuck in an artificial Overheat state ~Adam
//It shouldn't mess with the "Cool Fire" achievement and the overheat meter will fall the same as if the player had just let go of the button ~Adam

public class WeaponLockField : ForceFieldBase 
{

	void OnTriggerStay(Collider other)
	{
		//Need a this.enabled check because Unity's OnTrigger and OnCollision functions get called even when a script is disabled ~Adam
		if(mCurrentState == ForceFieldState.ON && this.enabled)
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
	}//END of OnTriggerStay()
	void OnTriggerExit(Collider other)
	{
		if(mCurrentState == ForceFieldState.ON && this.enabled)
		{
			PlayerShipController playerShip = other.GetComponent<PlayerShipController>();
			if(playerShip != null)
			{
				playerShip.isOverheated = false;
			}
		}
	}//END of OnTriggerExit()
}
