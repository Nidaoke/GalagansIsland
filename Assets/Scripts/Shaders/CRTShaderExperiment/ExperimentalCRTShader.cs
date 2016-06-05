//This was made following a tutorial at http://www.gamasutra.com/blogs/SvyatoslavCherkasov/20140531/218753/Shader_tutorial_CRT_emulation.php
using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]

public class ExperimentalCRTShader : MonoBehaviour 
{
	public Shader mShader;
	private Material mMaterial;

	[Range(0,1)] [SerializeField] private float mVerts_Force = 0f;
	[Range(0,1)] [SerializeField] private float mVerts_Force2 = 0f;
	[Range(-3,20)] [SerializeField] private float mContrast = 0f;
	[Range(-200,200)] [SerializeField] private float mBrightness = 0f;
	[Range(0,1)] [SerializeField] private float mScanStrength = 0f;



	protected Material material
	{
		get
		{
			if (mMaterial == null)
			{
				mMaterial = new Material(mShader);
				mMaterial.hideFlags = HideFlags.HideAndDontSave;
			}
			return mMaterial;
		}
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (mShader == null) return;
		Material mat = material;
		mat.SetFloat("_VertsColor", 1f-mVerts_Force);
		mat.SetFloat("_VertsColor2", 1f-mVerts_Force2);
		mat.SetFloat("_Contrast", mContrast);
		mat.SetFloat("_Brightness", mBrightness);
		mat.SetFloat("_ScanStrength", 1f-mScanStrength);

		Graphics.Blit(source, destination, mat);
	}

	void OnDisable()
	{
		if (mMaterial)
		{
			DestroyImmediate(mMaterial);
		}
	}
}