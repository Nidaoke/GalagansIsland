using UnityEngine;

namespace Assets.Scripts.Achievements
{
    public class AchievementStatInt : MonoBehaviour
    {
        [Tooltip("Steam platform API ID, needs to be the same as in Achievement Manager")]
        public string AssosiatedAchievementId;
        [Tooltip("If Stat is constant, it will be saved in PlayerPrefs and loaded on start. Use for snowball achievements")]
        public bool IsConstant;
        [Tooltip("Value that is required to unlock Achievement")]
        public int ValueToUnlock;

        private int currentValue;

        void Start()
        {
            if (IsConstant)
            {
                if (PlayerPrefs.HasKey("STAT_" + AssosiatedAchievementId))
                {
                    currentValue = PlayerPrefs.GetInt("STAT_" + AssosiatedAchievementId);
                }
            }
        }

        public void ResetValue()
        {
            currentValue = 0;
            if (IsConstant) SaveProgress();
        }

        public void IncreseValue()
        {
            currentValue++;
            CheckProgress();
            if (IsConstant) SaveProgress();
        }

        public void IncreseValue(int amount)
        {
            currentValue += amount;
            CheckProgress();
            if (IsConstant) SaveProgress();
        }

        private void SaveProgress()
        {
            PlayerPrefs.SetInt("STAT_" + AssosiatedAchievementId, currentValue);
        }

        private void CheckProgress()
        {
            if (currentValue >= ValueToUnlock)
                AchievementManager.instance.PostAchievement(AssosiatedAchievementId);
        }

    }
}
