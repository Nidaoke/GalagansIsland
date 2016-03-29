using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace TowerOfElements
{
	public class ToEManager : MonoBehaviour 
	{
		public bool mGameOver = false;
		[SerializeField] private GameObject mGameOverScreen;

		public ToEMage mMage;
		public ToEGoblinSpawner mGoblinSpawner;

		public int mScore = 0;
		public int mCombo = 0;
		public int mMultiplier = 1;
		[SerializeField] private Text mScoreText;
		[SerializeField] private Text mComboText;
		[SerializeField] private Text mToEHighScoreText;

		[SerializeField] private Image mMultiplierImage;
		[SerializeField] private Sprite[] mMultiplierSprites;

		float mReturnTimer = 5f;
		// Use this for initialization
		void Start () 
		{
		
		}//END of Start()
		
		// Update is called once per frame
		void Update () 
		{
			if(mGameOver)
			{
				mReturnTimer -= Time.deltaTime;

				if(mReturnTimer <= 0f)
				{
					//Do stuff to retun to the main GI game ~Adam
				}

				mGoblinSpawner.gameObject.SetActive (false);
				mGameOverScreen.SetActive (true);

			}

			//Set the score UI stuff
			mScoreText.text = "Score: " + mScore.ToString();
			mComboText.text = "Combo: " + mCombo.ToString();
			mMultiplierImage.sprite = mMultiplierSprites[mMultiplier-1];
			if(mScore > PlayerPrefs.GetInt ("ToEHighScore"))
			{
				PlayerPrefs.SetInt ("ToEHighScore", mScore);
			}
			mToEHighScoreText.text = "High Score: " + PlayerPrefs.GetInt ("ToEHighScore");

		}//END of Update()


		public void IncreaseCombo()
		{
			mCombo++;
			if(mCombo < 5)
			{
				mMultiplier = 1;
			}
			if(mCombo >= 5)
			{
				mMultiplier = 2;
			}
			if(mCombo >= 10)
			{
				mMultiplier = 3;
			}
			if(mCombo >= 20)
			{
				mMultiplier = 4;
			}
			if(mCombo >= 30)
			{
				mMultiplier = 5;
			}

		}//END of IncreaseCombo()
	}
}
