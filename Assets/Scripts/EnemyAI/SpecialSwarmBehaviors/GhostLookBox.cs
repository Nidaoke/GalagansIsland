using UnityEngine;
using System.Collections;

//This script is attched to a trigger box that makes it so when the player enters or leaves the box 
	//it makes formations change and toggles force fields ~Adam

public class GhostLookBox : MonoBehaviour 
{
	[SerializeField] private SwarmGrid[] mGhostArms;
	[SerializeField] private ForceFieldBase[] mForceFields;



	void OnTriggerEnter(Collider other)
	{
		if(other.GetComponent<PlayerShipController>() != null)
		{
			foreach (SwarmGrid arm in mGhostArms)
			{
				arm.ChangeFormation(1);
			}
			foreach (ForceFieldBase shield in mForceFields)
			{
				shield.TurnOn();
			}
		}
	}//END of OnTriggerEnter()

	void OnTriggerExit(Collider other)
	{
		if(other.GetComponent<PlayerShipController>() != null)
		{
			foreach (SwarmGrid arm in mGhostArms)
			{
				arm.ChangeFormation(0);
			}
			foreach (ForceFieldBase shield in mForceFields)
			{
				shield.TurnOff();
			}
		}
	}
}//END of OnTriggerExit()
