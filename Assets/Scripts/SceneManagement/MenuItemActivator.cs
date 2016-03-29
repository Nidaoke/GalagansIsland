using UnityEngine;
using System.Collections;

public class MenuItemActivator : MonoBehaviour 
{
	public GameObject mMenuUIController;

	public Vector3 scale;

	public Transform goTo;

	public float mActivationTimer = 0f;

	public bool mainMenu;

	bool mUiActivated = false;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (mainMenu) {

			mActivationTimer += Time.deltaTime;
		}

		transform.position = Vector3.Lerp(transform.position, goTo.position, 0.01f);

		if(!Application.isMobilePlatform)
		{
			transform.localScale = Vector3.Lerp(transform.localScale, scale, 0.01f);
		}
		else
		{
			transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(6.9f,6.9f,6.9f), 0.01f);
		}

		if (mainMenu) {

			if(mActivationTimer > 8.25f && !mUiActivated)
			{
				mMenuUIController.SetActive(true);
				mUiActivated = true;
			}
		}
	}
}
