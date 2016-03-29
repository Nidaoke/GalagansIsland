using UnityEngine;
using System.Collections;

namespace TowerOfElements
{
	public class ToEParticleDeath : MonoBehaviour 
	{
		// Use this for initialization
		void Start () {
			StartCoroutine(WaitToDestroy());
		}
		
		IEnumerator WaitToDestroy()
		{
			yield return new WaitForSeconds(1);
			Destroy(gameObject);
		}
	}	
}
