using UnityEngine;

namespace Assets.Scripts.Achievements
{
    public class AchievementStatFloat : MonoBehaviour
    {
        [Tooltip("Steam platform API ID, needs to be the same as in Achievement Manager")]
        public string AssosiatedAchievementId;
        [Tooltip("If Stat is constant, it will be saved in PlayerPrefs and loaded on start. Use for snowball achievements")]
        public bool IsConstant;
        [Tooltip("Value that is required to unlock Achievement")]
        public float ValueToUnlock;

        private float currentValue;

        void Start()
        {
            if (IsConstant)
            {
                if (PlayerPrefs.HasKey("STAT_" + AssosiatedAchievementId))
                {
                    currentValue = PlayerPrefs.GetFloat("STAT_" + AssosiatedAchievementId);
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

        public void IncreseValue(float amount)
        {
            currentValue += amount;
            CheckProgress();
            if (IsConstant) SaveProgress();
        }

        private void SaveProgress()
        {
            PlayerPrefs.SetFloat("STAT_" + AssosiatedAchievementId, currentValue);
        }

        private void CheckProgress()
        {
            if (currentValue >= ValueToUnlock)
                AchievementManager.instance.PostAchievement(AssosiatedAchievementId);
        }

    }
}