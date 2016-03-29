using UnityEngine;
using System.Collections;

public class ParticleController : MonoBehaviour {

    public EnemyDieParticleType type;

	// Use this for initialization
	void Start () {
	    switch (type)
        {
            case EnemyDieParticleType.MageNinja:
                gameObject.transform.RotateAround(new Vector3(0,0,1), Random.RandomRange(0.0f, 360.0f));
                break;
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
