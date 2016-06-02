using UnityEngine;
using System.Collections;

public class FSCBossTeleporter : MonoBehaviour 
{
	[SerializeField] private FSCBossController mPortalBoss;

	[SerializeField] private GameObject[] mBossPortals;


	[SerializeField] private float mTeleportTime;
	int mTargetPortal;
	[SerializeField] private float[] mPortalBounds;

	// Use this for initialization
	void Start () 
	{
		mPortalBoss.gameObject.SetActive(true);
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public void StartTeleport()
	{
		mBossPortals[0].transform.position = mPortalBoss.transform.position;

		for(int i = 1; i <mBossPortals.Length; i++)
		{
			while (Vector3.Distance(mBossPortals[i].transform.position, mBossPortals[i-1].transform.position) < 20f)
			{
				mBossPortals[i].transform.position = new Vector3(Random.Range(mPortalBounds[0], mPortalBounds[1]),Random.Range(mPortalBounds[0], mPortalBounds[1]),mBossPortals[i].transform.position.z);
			}
		}

		mTargetPortal = Random.Range(1,mBossPortals.Length);
		mBossPortals[0].SetActive(true);
		mPortalBoss.gameObject.SetActive(false);

		StartCoroutine(TeleportWait());


	}

	IEnumerator TeleportWait()
	{
		yield return new WaitForSeconds(2f);
		foreach(GameObject portal in mBossPortals)
		{
			portal.SetActive(true);
		}
		mBossPortals[0].SetActive(false);

		mPortalBoss.transform.position = mBossPortals[mTargetPortal].transform.position;

		yield return new WaitForSeconds(mTeleportTime);
		mPortalBoss.gameObject.SetActive(true);

		foreach(GameObject portal in mBossPortals)
		{
			portal.SetActive(false);
		}

		mPortalBoss.StartMouthLaser();

	}
}
