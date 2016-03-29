using UnityEngine;
using System.Collections;

public class EnemyDieParticleDestroy : MonoBehaviour {

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
