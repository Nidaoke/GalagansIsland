using UnityEngine;
using System.Collections;

//For turning on the second player ship when we do Co-Op Mode ~Adam

public class CoOpSelector : MonoBehaviour 
{
	public bool mCoOpEnabled = false;

	public bool mTutorialSelector = false;

	// Use this for initialization
	void Start () 
	{
	
	}

	void Awake()
	{
		DontDestroyOnLoad(this.gameObject);
	}

	// Update is called once per frame
	void Update () 
	{
		//Once we're actually in-game, enable the sencond player if co-op is enabled. ~Adam
		if(Application.loadedLevel != 0 && !(mTutorialSelector && Application.loadedLevelName =="Tutorial") )
		{
			Debug.Log ("Not on title screen!");
			if(mCoOpEnabled)
			{
				if(FindObjectOfType<GameStartObjectPointer>() != null)
				{
					FindObjectOfType<GameStartObjectPointer>().ActivateMultiPlayer();
				}
//				//Find the main player ship so we can find the inactive P2 ship ~Adam
//				if(FindObjectOfType<PlayerOneShipController>() != null)
//				{
//					Debug.Log ("Found Player 1");
//					PlayerOneShipController player = FindObjectOfType<PlayerOneShipController>();
//					//Make sure there's a player 2 ship present and then enable it ~Adam
//					if(player.mPlayerTwo != null)
//					{
//						Debug.Log("Found Player 2");
//						player.mPlayerTwo.gameObject.SetActive(true);
//						Debug.Log("Activated Player 2");
//						GameObject.Find ("P1ShipEmotes").SetActive (false);
//						GameObject.Find("P2ShipUI").SetActive (true);
//						GameObject.Find("P1ShipUI").SetActive (true);
//						GameObject.Find("P1ShipUI_SPR").SetActive (false);
//						GameObject.Find("P1ShipUI_SPL").SetActive (false);
//					}
//					else
//					{
//						Debug.Log ("No Player 2");
//						GameObject.Find ("P1ShipEmotes").SetActive (false);
//						GameObject.Find("P2ShipUI").SetActive (false);
//						GameObject.Find("P1ShipUI").SetActive (false);
//						GameObject.Find("P1ShipUI_SPR").SetActive (true);
//						GameObject.Find("P1ShipUI_SPL").SetActive (true);
//					}
//
//				}
//				FindObjectOfType<ScoreManager>().StartCoOpMode();
			}
			else
			{
				if(FindObjectOfType<GameStartObjectPointer>() != null)
				{
					FindObjectOfType<GameStartObjectPointer>().ActivateSinglePlayer();
				}
//				Debug.Log ("No Player 2");
//				GameObject.Find ("P1ShipEmotes").SetActive (false);
//				GameObject.Find("P2ShipUI").SetActive (false);
//				GameObject.Find("P1ShipUI").SetActive (false);
//				GameObject.Find("P1ShipUI_SPR").SetActive (true);
//				GameObject.Find("P1ShipUI_SPL").SetActive (true);
			}
			//Delete self so that we don't get duplicate copies of the object piling up on the main menu scene ~Adam
			Debug.Log("Destroying CoOp selector");
			Destroy(this.gameObject);
		}
	}
}
