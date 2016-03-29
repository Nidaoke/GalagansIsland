using UnityEngine;
using System.Collections;
using Assets.Scripts.IndieGamesLauncher;

public class IndieGameCollider : MonoBehaviour 
{
	ScoreManager mScoreMan;
	//public string mGameTitle;
	public int mIndieGameNumber;
		//1: Tower of Elements
		//2: 
		//3: 
	public float mDriftSpeed = 5.47f;

	// Use this for initialization
	void Start () 
	{
		transform.position = new Vector3(Random.Range (-10,10), Random.Range (0,20), -2); //Places at random position
		if(FindObjectOfType<PlayerShipController>() != null)
		{
			while(Vector3.Distance (transform.position, FindObjectOfType<PlayerShipController>().transform.position) < 5f) //Move down
			{
				transform.position = new Vector3(Random.Range (-10,10), Random.Range (0,20), -2);
			}
		}
		if(PlayerPrefs.GetInt("GoingToGame") != 0)
		{
			PlayerPrefs.SetInt("GoingToGame", 0);
			Destroy(this.gameObject);
		}
		GetComponent<AudioSource>().enabled=true;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(mScoreMan == null)
		{
			mScoreMan = FindObjectOfType<ScoreManager>();
		}
		else if(mScoreMan.mPlayer2Avatar != null || mScoreMan.mP2Score > 0)
		{
			Destroy(this.gameObject);
		}
		transform.Translate (Vector3.down* mDriftSpeed*Time.deltaTime); //Move down

	}

	void OnTriggerEnter(Collider other)
	{
		if(other.GetComponent<PlayerOneShipController>() != null)
		{
			PlayerPrefs.SetInt("MainLevelLeft", Application.loadedLevel);
			PlayerPrefs.SetInt("IndieGameKey", mIndieGameNumber);

			Application.LoadLevel("IndieGameTransition");
	
		}
	}
}
