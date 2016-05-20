using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

//Script for the level in the third set of 25 where we rapidly do a bunch of small minigames using existing mechanics. ~Adam
//The player gets points if they win a game and gets shot if they lose. ~Adam
//Randomize the order in which the games are played each time. ~Adam
namespace MicroGameLevel
{
	public class MicroGameManager : MonoBehaviour 
	{
		ScoreManager mScoreMan;
		PlayerOneShipController mP1Ship;
		PlayerTwoShipController mP2Ship;
		[SerializeField] private LevelKillCounter mKillCoutner;

		int mCurrentGame = 0;
		[SerializeField] private List<GameObject> mMicroGames;
		[SerializeField] private List<GameObject> mMicroGamesRandomized;

		[SerializeField] private GameObject mPunishmentBeams;

		[Header("For the instruction box:")]
		[SerializeField] private GameObject mInstructionBox;
		[SerializeField] private Text mInstructionText;

		[SerializeField] private GameObject mTimerBox;
		[SerializeField] private Text mTimerText;

		// Use this for initialization
		void Start () 
		{
			StartCoroutine(FindScoreManagerAndPlayerShips());
			//Randomize the order the games are played in ~Adam
			RandomizeGameOrder();

		}//END of Start()
		


		//Called at Start to move the games from one list to another in a random order ~Adam
		void RandomizeGameOrder()
		{
			int gameCount = mMicroGames.Count;
			for (int i = 0; i < gameCount; i++)
			{
				int randomGame = Random.Range(0,mMicroGames.Count);
				mMicroGamesRandomized.Add(mMicroGames[randomGame]);
				mMicroGames.RemoveAt(randomGame);
			}
		}//END of RandomizeGameOrder()

		IEnumerator FindScoreManagerAndPlayerShips()
		{
			yield return new WaitForSeconds(1f);
			mScoreMan = FindObjectOfType<ScoreManager>();
			mP1Ship = mScoreMan.mPlayerAvatar.GetComponent<PlayerOneShipController>();
			mP2Ship = mScoreMan.mPlayer2Avatar.GetComponent<PlayerTwoShipController>();
			yield return new WaitForSeconds(4f);
			mInstructionBox.SetActive(false);
			yield return new WaitForSeconds(2f);
			PlayNextGame();
		}//END of FindScoreManagerAndPlayerShips()

		public ScoreManager GetScoreMan()
		{
			return mScoreMan;
		}//END of 
			
		public void PlayNextGame()
		{
			if(mCurrentGame < mMicroGamesRandomized.Count && mMicroGamesRandomized.Count >0)
			{
				mMicroGamesRandomized[mCurrentGame].SetActive(true);
				mTimerBox.SetActive(true);
				mCurrentGame++;
			}
			//End the level if there are no games left ~Adam
			else
			{
				mKillCoutner.mKillCount = mKillCoutner.mRequiredKills+10;
			}
		}//END of 
			
		//Lets other scripts set messages and display times ~Adam
		public void SetUpMessage(string message, float messageDuration)
		{
			StartCoroutine(DisplayMessage(message, messageDuration));
		}//END of 

		//Set a message to display for a bit then disappear ~Adam
		IEnumerator DisplayMessage(string message, float messageDuration)
		{
			mInstructionText.text = message;
			mInstructionBox.SetActive(true);
			yield return new WaitForSeconds(messageDuration);
			mInstructionBox.SetActive(false);
		}//END of 

		#region Activate un-dodgeable shield-killers if the player fails a micro-game ~Adam
		public void PunishPlayer()
		{
			mTimerBox.SetActive(false);
			StartCoroutine(FirePunishmentBeams());
		}//END of 

		IEnumerator FirePunishmentBeams()
		{
			StartCoroutine(DisplayMessage("Failure!", 5f));
			mPunishmentBeams.SetActive(true);
			yield return new WaitForSeconds(5f);
			mPunishmentBeams.SetActive(false);
			yield return new WaitForSeconds(1f);
			PlayNextGame();
		}//END of 
		#endregion

		#region Give a bunch of points if a player wins a micro-game ~Adam
		public void RewardPlayer()
		{
			mTimerBox.SetActive(false);
			StartCoroutine(GivePlayerReward());
		}//END of 

		IEnumerator GivePlayerReward()
		{
			StartCoroutine(DisplayMessage("Success!", 5f));

			//Split the awarded points up if there are two players ~Adam
			if(mScoreMan.mP2Score >0)
			{
				mScoreMan.AdjustScore(50,true);
				mScoreMan.AdjustScore(50,false);
			}
			else
			{
				mScoreMan.AdjustScore(100,true);
			}

			yield return new WaitForSeconds(6f);
			PlayNextGame();
		}//END of 
		#endregion

		public void SetTimerText(string timeRemaining)
		{
			mTimerText.text = timeRemaining;
		}
	}
}