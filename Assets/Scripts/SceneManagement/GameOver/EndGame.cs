using UnityEngine;
using System.Collections;

public class EndGame : MonoBehaviour 
{

	public GameObject evilLaugh;

	public float waitSeconds = 12f;
	public GameObject ship;

	//For reloading an earlier level when there's a checkpoint ~Adam
	public GameObject mCoOpSelector; 
	public GameObject mCheckPointCanvas;
	

	void Update () 
	{

		//Cursor.visible = true;

		waitSeconds -= Time.deltaTime;

		if (waitSeconds <= 4) 
		{
			evilLaugh.SetActive(true);
		}

		if (waitSeconds <= -0.5f && !mCheckPointCanvas.activeInHierarchy) 
		{
			if(PlayerPrefs.GetInt ("CheckPointedLevel") != 0)
			{
				mCheckPointCanvas.SetActive (true);
			}
			else
			{
				ReloadGame ();
			}
		}

		if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse1)  && !mCheckPointCanvas.activeInHierarchy) 
		{
			if(PlayerPrefs.GetInt ("CheckPointedLevel") != 0)
			{
				mCheckPointCanvas.SetActive (true);
			}
			else
			{
				ReloadGame ();
			}
		}
	}

	public void ReloadGame()
	{
		evilLaugh.SetActive(false);
		Destroy(ship);
//		if(FindObjectOfType<ScoreManager>().gameObject != null)
//		{
//			Destroy(FindObjectOfType<ScoreManager>().gameObject);
//		}
		if(PlayerPrefs.GetInt ("CheckPointedLevel") != 0)
		{
			if(FindObjectOfType<ScoreManager>() != null)
			{
				Destroy (FindObjectOfType<ScoreManager>().gameObject);
			}
			if(GameObject.Find ("Game_HUD") != null)
			{
				Destroy (GameObject.Find ("Game_HUD").gameObject);
			}
			mCoOpSelector.SetActive (true);
			Application.LoadLevel(PlayerPrefs.GetInt ("CheckPointedLevel"));
		}
		else
		{
			PlayerPrefs.SetInt ("CheckPointedLevel", 0);
			Application.LoadLevel("Credits");
		}

	}
}
