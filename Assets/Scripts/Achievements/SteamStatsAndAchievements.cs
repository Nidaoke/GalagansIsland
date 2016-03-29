using UnityEngine;
using System.Collections;
using System.ComponentModel;
using Steamworks;

namespace Assets.Scripts.Achievements
{

	public class SteamStatsAndAchievements : MonoBehaviour
	{

		//Level Dependent Achievements
		private int currentLevel = -1;
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
			//instance = this;
		}

		// Our GameID
		private CGameID m_GameID;

		// Did we get the stats from Steam?
		private bool m_bRequestedStats;
		private bool m_bStatsValid;

		// Should we store stats this frame?
		public bool m_bStoreStats;

		protected Callback<UserStatsReceived_t> m_UserStatsReceived;
		protected Callback<UserStatsStored_t> m_UserStatsStored;
		protected Callback<UserAchievementStored_t> m_UserAchievementStored;

		private void Update()
		{

			if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.O))
			{
				
				SteamUserStats.ResetAllStats(true);
			}



			if (!SteamManager.Initialized)
				return;

			if (!m_bRequestedStats)
			{
				// Is Steam Loaded? if no, can't get stats, done
				if (!SteamManager.Initialized)
				{
					m_bRequestedStats = true;
					return;
				}

				// If yes, request our stats
				bool bSuccess = SteamUserStats.RequestCurrentStats();

				// This function should only return false if we weren't logged in, and we already checked that.
				// But handle it being false again anyway, just ask again later.
				m_bRequestedStats = bSuccess;
			}

			if (!m_bStatsValid)
				return;

			// Get info from sources

			// Evaluate achievements


			//Store stats in the Steam database if necessary
			if (m_bStoreStats)
			{
				// already set any achievements in UnlockAchievement

				// set stats
				//SteamUserStats.SetStat("BluesCollected", BluesCollected);

				bool bSuccess = SteamUserStats.StoreStats();
				// If this failed, we never sent anything to the server, try
				// again later.
				m_bStoreStats = !bSuccess;
			}
		}

		private void OnUserStatsReceived(UserStatsReceived_t pCallback)
		{
			if (!SteamManager.Initialized)
				return;

			// we may get callbacks for other games' stats arriving, ignore them
			if ((ulong)m_GameID == pCallback.m_nGameID)
			{
				if (EResult.k_EResultOK == pCallback.m_eResult)
				{
					Debug.Log("Received stats and achievements from Steam\n");

					m_bStatsValid = true;

					/* load achievements
					foreach (Achievement_t ach in m_Achievements)
					{
						bool ret = SteamUserStats.GetAchievement(ach.m_eAchievementID.ToString(), out ach.m_bAchieved);
						if (ret)
						{
							ach.m_strName = SteamUserStats.GetAchievementDisplayAttribute(ach.m_eAchievementID.ToString(), "name");
							ach.m_strDescription = SteamUserStats.GetAchievementDisplayAttribute(ach.m_eAchievementID.ToString(), "desc");
						}
						else {
							Debug.LogWarning("SteamUserStats.GetAchievement failed for Achievement " + ach.m_eAchievementID + "\nIs it registered in the Steam Partner site?");
						}
					}*/

					/*SteamUserStats.GetStat("NumGames", out m_nTotalGamesPlayed);
                SteamUserStats.GetStat("NumWins", out m_nTotalNumWins);
                SteamUserStats.GetStat("NumLosses", out m_nTotalNumLosses);
                SteamUserStats.GetStat("FeetTraveled", out m_flTotalFeetTraveled);
                SteamUserStats.GetStat("MaxFeetTraveled", out m_flMaxFeetTraveled);
                SteamUserStats.GetStat("AverageSpeed", out m_flAverageSpeed);*/
				}
				else {
					Debug.Log("RequestStats - failed, " + pCallback.m_eResult);
				}
			}
		}

		//-----------------------------------------------------------------------------
		// Purpose: Our stats data was stored!
		//-----------------------------------------------------------------------------
		private void OnUserStatsStored(UserStatsStored_t pCallback)
		{
			// we may get callbacks for other games' stats arriving, ignore them
			if ((ulong)m_GameID == pCallback.m_nGameID)
			{
				if (EResult.k_EResultOK == pCallback.m_eResult)
				{
					Debug.Log("StoreStats - success");
				}
				else if (EResult.k_EResultInvalidParam == pCallback.m_eResult)
				{
					// One or more stats we set broke a constraint. They've been reverted,
					// and we should re-iterate the values now to keep in sync.
					Debug.Log("StoreStats - some failed to validate");
					// Fake up a callback here so that we re-load the values.
					//UserStatsReceived_t callback = new UserStatsReceived_t();
					//callback.m_eResult = EResult.k_EResultOK;
					//callback.m_nGameID = (ulong)m_GameID;
					//OnUserStatsReceived(callback);
				}
				else {
					Debug.Log("StoreStats - failed, " + pCallback.m_eResult);
				}
			}
		}

		//-----------------------------------------------------------------------------
		// Purpose: An achievement was stored
		//-----------------------------------------------------------------------------
		private void OnAchievementStored(UserAchievementStored_t pCallback)
		{
			// We may get callbacks for other games' stats arriving, ignore them
			if ((ulong)m_GameID == pCallback.m_nGameID)
			{
				if (0 == pCallback.m_nMaxProgress)
				{
					Debug.Log("Achievement '" + pCallback.m_rgchAchievementName + "' unlocked!");
				}
				else {
					Debug.Log("Achievement '" + pCallback.m_rgchAchievementName + "' progress callback, (" + pCallback.m_nCurProgress + "," + pCallback.m_nMaxProgress + ")");
				}
			}
		}
	}
}