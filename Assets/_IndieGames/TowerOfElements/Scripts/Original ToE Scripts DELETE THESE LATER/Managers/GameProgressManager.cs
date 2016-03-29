using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Managers
{
    public class GameProgressManager : MonoBehaviour
    {
        public static GameProgressManager instance;

        public Scrollbar MagesScrollbar;
        public Text MagesText;

        public Scrollbar ScoreScrollbar;
        public Text ScoreText;

        public Scrollbar TowerScrollbar;
        public Text TowerText;

        void Awake()
        {
            instance = this;
        }


        public void SetUpMagesProgress(float current, float max)
        {
            float procent = current/max;
            if (procent > 1) procent = 1;
            MagesScrollbar.size = procent;
            MagesText.text = "Mages " + (int)(procent * 100) + "%";
        }

        public void SetUpScoreProgress(float current, float max)
        {
            float procent = current / max;
            if (procent > 1) procent = 1;
            ScoreScrollbar.size = procent;
            ScoreText.text = "Score " + (int)(procent * 100) + "%";
        }

        public void SetUpTowerProgress(float current, float max)
        {
            float procent = current / max;
            if (procent > 1) procent = 1;
            TowerScrollbar.size = procent;
            TowerText.text = "Tower " + (int)(procent * 100) + "%";
        }
    }
}
