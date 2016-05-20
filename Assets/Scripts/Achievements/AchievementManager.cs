using UnityEngine;
using System.Collections.Generic;
using System;
using Steamworks;
//using ManagedSteam;

namespace Assets.Scripts.Achievements
{
    [Serializable]
    public class AchievementBase
    {
        public string AchievementSteamID;
        public bool IsUnlocked;
    }

    public class AchievementManager : MonoBehaviour
    {
        public static AchievementManager instance;

        public List<AchievementBase> AchievementList;

        //Level Dependent Achievements
		public int currentLevel = -1;
        [SerializeField] private int livesBeforeBoss;
        private ScoreManager scoreManager;

        //overheat Achievement
        public int numberOfOverheats;

        //RepairStation Dependent Stats
        [Header("Repair Station Stats")]
        public AchievementStatInt RepairShip100Times;
        public AchievementStatInt UpgradeWeapons100Times;
        public AchievementStatInt UpgradeSpeed100Times;
        public AchievementStatInt UpgradeOnlyWeapons;
        public AchievementStatInt UpgradeOnlySpeed;

        //Kill Counters
        [Header("Kill Stats")]
        public AchievementStatInt KillCounter1;
        public AchievementStatInt KillCounter2;
        public AchievementStatInt KillCounter3;
        public AchievementStatInt KillCounter4;
        public AchievementStatInt KillCounter5;

        //Other Counters
        [Header("Other Stats")]
        public AchievementStatInt BackStab;
        public AchievementStatTimer FriendOMine;
        public AchievementStatInt UpgradesCollected;
        public AchievementStatInt IDontCare;

        void Awake()
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }

        void Start()
        {
			if(PlayerPrefs.GetInt ("HasPlayedWithAchievements") != 23)
			{
				Debug.Log("Reseting PlayerPrefs!");
				int highScore = PlayerPrefs.GetInt ("highscore");
				float bgm = PlayerPrefs.GetFloat("BGMVolume");
				float sfx = PlayerPrefs.GetFloat("SFXVolume");
				PlayerPrefs.DeleteAll ();
				PlayerPrefs.SetInt ("highscore", highScore);
				PlayerPrefs.SetFloat ("BGMVolume", bgm);
				PlayerPrefs.SetFloat ("SFXVolume", sfx);

				RepairShip100Times.ResetValue();
				UpgradeWeapons100Times.ResetValue();
				UpgradeSpeed100Times.ResetValue();
				UpgradeOnlyWeapons.ResetValue();
				UpgradeOnlySpeed.ResetValue();

				KillCounter1.ResetValue();
				KillCounter2.ResetValue();
				KillCounter3.ResetValue();
				KillCounter4.ResetValue();
				KillCounter5.ResetValue();

				BackStab.ResetValue();
				FriendOMine.ResetValue();
				UpgradesCollected.ResetValue();
				IDontCare.ResetValue();

				Debug.Log("PlayerPrefs Reset!");
				PlayerPrefs.SetInt ("HasPlayedWithAchievements", 23);
			}
            LoadAchievements();
        }

        void Update()
        {
            HandleLevelAchievements();
        }

        /*private bool IsAchievementUnlocked(string achievementID)
        {
            foreach (var achiv in AchievementList)
            {
                if (achiv.AchievementSteamID == achievementID)
                {
                    return achiv.IsUnlocked;
                }
            }
            
        }*/

        private void SetAchievementAsUnlocked(string achievementID)
        {
            foreach (var achiv in AchievementList)
            {
                if (achiv.AchievementSteamID == achievementID)
                {
                    achiv.IsUnlocked = true;
                    PlayerPrefs.SetInt("ACHIV_" + achievementID, 1);
                    return;
                }
            }
            HandleMasterOfGalangans();
        }

        private void LoadAchievements()
        {
            foreach (var achiv in AchievementList)
            {
                if (PlayerPrefs.HasKey("ACHIV_" + achiv.AchievementSteamID))
				{
                    achiv.IsUnlocked = true;
                }
                else
                {
                    achiv.IsUnlocked = false;
                }
            }
        }

        public void PostAchievement(string achievementID)
        {
			Debug.Log ("PA has been called" + achievementID);

			foreach (var achiv in AchievementList)
			{
				if (achiv.AchievementSteamID == achievementID) {
					Debug.Log ("FOIUND IT!");

					if (achiv.IsUnlocked) {

						Debug.Log ("Achievement Unloacked alrady");
					} else {

						Debug.Log ("Achievement " + achievementID + " has been Unlocked!");
						try {
							//Steamworks.SteamInterface.Stats.SetAchievement(achievementID);

							SteamUserStats.SetAchievement (achievementID);
						} catch (Exception e) {
							Debug.LogError ("Count not sand Achievement to Steam: " + e.Message);
						}
						SetAchievementAsUnlocked (achievementID);
					}
				} else {

					//Debug.Log ("Found " + achiv.AchievementSteamID + ", not " + achievementID);
				}
			}


        }

        private void HandleLevelAchievements()
        {
            if (currentLevel != Application.loadedLevel)
            {
                try
                {
                    scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
                }
                catch (Exception e)
                {

                }

                currentLevel = Application.loadedLevel;
                switch (currentLevel)
                {
					case 1:
						numberOfOverheats = 0;
						break;
				case 2:

					Debug.Log ("SecondLevel");
                        if (scoreManager.mLivesRemaining == 100)
                        {
						Debug.Log ("100 Lives Left!");
                            PostAchievement("CantTouchThis"); // No lifes lost till lvl 1.
                        }
                        break;
                    case 6:
                        if (scoreManager.mLivesRemaining == 100)
                        {
                            PostAchievement("FasterThanLight"); // No lifes lost till lvl 5.
                        }
                        livesBeforeBoss = scoreManager.mLivesRemaining;
                        break;
                    case 7:
                        PostAchievement("A_Boss1");
                        if (livesBeforeBoss <= scoreManager.mLivesRemaining)
                        {
                            PostAchievement("A_Boss1F");
                        }
                        break;
                    case 12:
                        livesBeforeBoss = scoreManager.mLivesRemaining;
                        break;
                    case 13:
                        PostAchievement("A_Boss2");
                        if (livesBeforeBoss <= scoreManager.mLivesRemaining)
                        {
                            PostAchievement("A_Boss2F");
                        }
                        break;
                    case 18:
                        livesBeforeBoss = scoreManager.mLivesRemaining;
                        break;
                    case 19:
                        PostAchievement("A_Boss3");
                        if (livesBeforeBoss <= scoreManager.mLivesRemaining)
                        {
                            PostAchievement("A_Boss3F");
                        }
                        break;
                    case 24:
                        livesBeforeBoss = scoreManager.mLivesRemaining;
                        break;
                    case 25:
                        PostAchievement("A_Boss4");
                        if (livesBeforeBoss <= scoreManager.mLivesRemaining)
                        {
                            PostAchievement("A_Boss4F");
                        }
                        break;
                    case 30:
                        livesBeforeBoss = scoreManager.mLivesRemaining;
                        break;
                    case 32:

                        break;
                }
            }
        }

		public void LastBossCheck()
		{
			PostAchievement("A_Boss5");
			PostAchievement("ItsOverIsntIt");
			if (livesBeforeBoss <= scoreManager.mLivesRemaining)
			{
				PostAchievement("A_Boss5F");
			}
			if (numberOfOverheats == 0)
			{
				PostAchievement("CoolFire");
			}
		}

        private void HandleMasterOfGalangans()
        {
            int amountOfFinishedAchievements = 0;
            foreach(var achiv in AchievementList)
            {
                if (achiv.IsUnlocked) amountOfFinishedAchievements++;
            }
            
            if (amountOfFinishedAchievements >= AchievementList.Count-1)
            {
                PostAchievement("MasterofGalagan");
            }
        }
    }
}