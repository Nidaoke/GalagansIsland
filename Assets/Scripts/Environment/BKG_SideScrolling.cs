using UnityEngine;
using System.Collections;

public class BKG_SideScrolling : MonoBehaviour 
{

	float m_fSpeed;

	GameObject mPlayerAvatar;
	Vector3 mDefaultPosition;

	Vector3 mPlayerMoveDirection = Vector3.zero;
	[SerializeField] private Vector3 desiredPosition;
	//float mPlayerMoveDistance = 0f;
	float mRenderOffest = 0f;

	//For making the scrolling persist between levels ~Adam
	[SerializeField] private ScoreManager mScoreManager;
	[SerializeField] private bool mCheckLastOffset = true;




	// Use this for initialization
	void Start()
	{
		mPlayerAvatar = GameObject.FindGameObjectWithTag("Player");
		mDefaultPosition = transform.position;

		//transform.position = new Vector3 (transform.position.x, transform.position.y, 50f);

		//Deal with potential duplicate score managers and only use the oldest one ~Adam
		ScoreManager[] existingScoreManagers = FindObjectsOfType<ScoreManager>();
		foreach(ScoreManager scoreManager in existingScoreManagers)
		{
			if(mScoreManager == null)
			{
				mScoreManager = scoreManager;
			}
			else
			{
				if(scoreManager.mOriginalLevel < mScoreManager.mOriginalLevel)
				{
					mScoreManager = scoreManager;
				}
			}
		}
		//mScoreManager = GameObject.FindObjectOfType<ScoreManager>();
		m_fSpeed = 0.04f;


	}

	// Update is called once per frame
	void Update()
	{
		if(mScoreManager == null)
		{
			mScoreManager = FindObjectOfType<ScoreManager>();
		}

		if(Time.timeScale != 0f)
		{
			

			if(mPlayerAvatar != null)
			{

				mPlayerMoveDirection = mPlayerAvatar.GetComponent<PlayerShipController>().mMoveDir;

			}
			if(mPlayerMoveDirection.x > 0f)
			{
				mRenderOffest += (Time.deltaTime * m_fSpeed) + (Time.deltaTime*m_fSpeed*mPlayerMoveDirection.x) ;
			}
			else
			{
				mRenderOffest += (Time.deltaTime * m_fSpeed) + (Time.deltaTime*m_fSpeed*mPlayerMoveDirection.x*-0.1f) ;
			}

			GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(0, mRenderOffest));
		}
	}


}
