using UnityEngine;
using System.Collections;

//On the DarkTome level the tome randomly cycles between casting various "Spells" where it "summons" new minions into swarms
	//that obestrct the player's acces to parts of the screen and then fall off screen until they kill all the enemies in them by going out of bounds ~Adam

public class DarkTomeSpells : MonoBehaviour 
{
	int mSpellToCast = 0;
		//0: "Fire Spell" like in the "Abandon Hope" level
		//1: Summon Skull
		//2: Summon Tentacles
		//3: Activate Shield Killer and Thrust Canceler

	[SerializeField] private float mSpellCastInterval = 30f;

	[SerializeField] private EnemyShipSpawner mFireSpawner;
	[SerializeField] private EnemyShipSpawner mSkullSpawner;
	[SerializeField] private EnemyShipSpawner mTentalceSpawnerRight;
	[SerializeField] private EnemyShipSpawner mTentacleSpawnerLeft;

	[SerializeField] private Rigidbody mSkullRigidbody;
	[SerializeField] private Rigidbody mTentacleRigidbody;

	[SerializeField] private GameObject mBeamHolder;
	[SerializeField] private GameObject mSpawnPortal;

	Vector3 mRigidbodyReturnPos;

	// Use this for initialization
	void Start () 
	{
		StartCoroutine(InitialWait());
	}//END of Start()
	
	IEnumerator InitialWait()
	{
		yield return new WaitForSeconds(15);
		ChooseSpell();
	}//END of ()

	void ChooseSpell()
	{
		mSpawnPortal.SetActive(true);
		mSpellToCast = Random.Range(0,5);
		switch (mSpellToCast)
		{
		case 0:
			SummonFire();
			break;
		case 1:
			StartCoroutine(SummonSkull());
			break;
		case 2:
			StartCoroutine(SummonTentacles());
			break;
		case 3:
			StartCoroutine(FireBeams());
			break;
		}
		StartCoroutine(SpellCooldown());

	}//END of ChooseSpell

	IEnumerator SpellCooldown()
	{
		yield return new WaitForSeconds(mSpellCastInterval);
		ChooseSpell();
	}//END of SpellCooldown()

	void SummonFire()
	{
		mFireSpawner.mSpawning = true;
	}//END of SummonFire()

	IEnumerator SummonSkull()
	{
		mSkullSpawner.mSpawning = true;
		yield return new WaitForSeconds(10);
		mRigidbodyReturnPos = mSkullRigidbody.transform.position;
		mSkullRigidbody.constraints &= ~RigidbodyConstraints.FreezePositionY;
		yield return new WaitForSeconds(3);
		mSkullRigidbody.transform.position = Vector3.down*500f;
		yield return new WaitForSeconds(1);
		Debug.Log("Returning skull to its propper placer");
		mSkullRigidbody.constraints = RigidbodyConstraints.FreezeAll;
		mSkullRigidbody.transform.position = mRigidbodyReturnPos;
	}//END of SummonSkull()

	IEnumerator SummonTentacles()
	{
		mTentacleSpawnerLeft.mSpawning = true;
		mTentalceSpawnerRight.mSpawning = true;
		yield return new WaitForSeconds(10);
		mRigidbodyReturnPos = mTentacleRigidbody.transform.position;
		mTentacleRigidbody.constraints &= ~RigidbodyConstraints.FreezePositionY;
		yield return new WaitForSeconds(3);
		mTentacleRigidbody.transform.position = Vector3.down*500f;
		yield return new WaitForSeconds(1);
			Debug.Log("Returning tentacles to their propper placer");
		mTentacleRigidbody.constraints = RigidbodyConstraints.FreezeAll;
		mTentacleRigidbody.transform.position = mRigidbodyReturnPos;
	}//END of SummonTentacles()

	IEnumerator FireBeams()
	{
		mBeamHolder.SetActive(true);
		yield return new WaitForSeconds(10f);
		mBeamHolder.SetActive(false);
	}//END of FireBeams()
}
