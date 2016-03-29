using UnityEngine;
using System.Collections;

public class SocialMediaBox : MonoBehaviour 
{
	[SerializeField] private SwarmGrid mOutlineGrid;
	[SerializeField] private GameObject mMediaComponents;
	bool mShowSocialMedia = false;

	#region Variables for showing text ~Adam
	[SerializeField] private TextMesh mMediaText;
	[SerializeField] private string mFullText = "";
	[SerializeField] private float mWordsPerSecond = 2f; // speed of typewriter
	private float mTextTimeElapsed = 0f;   
	#endregion

	[SerializeField] private GameObject mShieldKiller;
	bool mHasActivated = false;

	// Use this for initialization
	void Start () 
	{
		mFullText = mMediaText.text;
		mMediaText.text = "";
		StartCoroutine(UpdateMediaWait());
	}

	void Update()
	{
		if(mShowSocialMedia)
		{
			mTextTimeElapsed += Time.deltaTime;
			mMediaText.text = GetWords(mFullText, mTextTimeElapsed * mWordsPerSecond);
		}
	}

	private string GetWords(string text, float wordCount)
	{
		int words = Mathf.FloorToInt( wordCount);
		// loop through each character in text
		for (int i = 0; i < text.Length; i++)
		{ 
			if (text[i] == ' ')
			{
				words--;
			}
			if (words <= 0)
			{
				return text.Substring(0, i);
			}
		}
		return text;
	}

	void UpdateMediaVisibility()
	{
		mShowSocialMedia = false;
		foreach(SwarmGridSlot gridSlot in mOutlineGrid.mGridSlots)
		{
			if(gridSlot.mOccupied)
			{
				mShowSocialMedia = gridSlot.mOccupied;
			}
		}
		if(mShowSocialMedia)
		{
			mShowSocialMedia = (mOutlineGrid.GetFormationNumber() == 0);
		}
		mMediaComponents.SetActive(mShowSocialMedia);
		StartCoroutine(UpdateMediaWait());
		if(mShowSocialMedia)
		{
			mHasActivated = true;
		}
		else if(mHasActivated)
		{
			mShieldKiller.SetActive(true);
		}

	}

	IEnumerator UpdateMediaWait()
	{
		yield return new WaitForSeconds(3.3f);
		UpdateMediaVisibility();
	}

	

}
