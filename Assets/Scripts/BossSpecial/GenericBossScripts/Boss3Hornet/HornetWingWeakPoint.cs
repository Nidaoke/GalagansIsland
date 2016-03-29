using UnityEngine;
using System.Collections;

public class HornetWingWeakPoint : BossWeakPoint 
{
	[SerializeField] private GameObject[] mWingSpawners;

	protected override void WeakPointDestruction()
	{
		//Turn off the spawners ~Adam
		foreach(GameObject wingSpawner in mWingSpawners)
		{
			wingSpawner.SetActive (false);
		}
		//For giving player 2 points when we blow up the wings ~Adam
		PlayerTwoShipController playerTwo = new PlayerTwoShipController();
		if(FindObjectOfType<PlayerTwoShipController>() != null)
		{
			playerTwo = FindObjectOfType<PlayerTwoShipController>();
		}
		//Kill the Wings ~Adam
		foreach(EnemyShipAI wingEnemy in FindObjectsOfType<EnemyShipAI>())
		{
			if(playerTwo != null && Random.value < 0.5f)
			{
				wingEnemy.mKillerNumber = 2;
			}
			wingEnemy.EnemyShipDie ();
		}
		base.WeakPointDestruction ();
	}
}
