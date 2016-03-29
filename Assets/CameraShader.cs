using UnityEngine;
using System.Collections;

public class CameraShader : MonoBehaviour 
{

	public CameraFilterPack_TV_Vcr shader1;
	public CameraFilterPack_TV_VHS shader2;
	public float mShaderTimer = 0f;
	public void Start()
	{

		shader1 = GetComponent<CameraFilterPack_TV_Vcr> ();
		shader2 = GetComponent<CameraFilterPack_TV_VHS> ();
	}

	void Update()
	{
		//Turn the shaders on while the timer is above 0 ~Adam
		if(mShaderTimer > 0f)
		{
			//Count down the timer ~Adam
			mShaderTimer -= Time.deltaTime;

			if(shader1 != null)
			{
				shader1.enabled = true;
			}
			if(shader2 != null)
			{
				shader2.enabled = true;
			}
		}
		//Otherwise keep them off ~Adam
		else
		{
			if(shader1 != null)
			{
				shader1.enabled = false;
			}
			if(shader2 != null)
			{
				shader2.enabled = false;
			}
		}
	}
}
