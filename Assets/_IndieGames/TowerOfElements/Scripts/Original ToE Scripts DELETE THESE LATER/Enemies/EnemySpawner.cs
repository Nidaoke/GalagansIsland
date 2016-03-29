using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour {

    public List<GameObject> EnemiesToSpawn;
    public Transform LeftSpawnPoint;
    public Transform RightSpawnPoint;

    public float timeThreshold = 0.5f;
    private int unitsInWave = 3;
    private float timer = 0;
    private int enemySpawnCounter;

    private GameObject CurrentGreenParticleSystem;
    private GameObject CurrentRedParticleSystem;
    private GameObject CurrentBlueParticleSystem;

    public bool AllowToSpawn = false;

    public static EnemySpawner Instance;

    void Awake()
    {
        Instance = this;
    }

    public void RestartSpawner()
    {
        timeThreshold = 0.5f;
        unitsInWave = 3;
        timer = 0;
        enemySpawnCounter = 0;
        AllowToSpawn = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (AllowToSpawn)
        {
            timer += Time.deltaTime;
            if (timer > timeThreshold)
            {
                int indexOfEnemy = 0;
				//Need to replace TestGameManager with another script
//                if (TestGameManager.Instance.Score < 10)
//                {
//                    indexOfEnemy = 0;
//                    
//                }
//                else if (TestGameManager.Instance.Score < 40)
//                {
//                    indexOfEnemy = Random.Range(0, EnemiesToSpawn.Count-1);
//                }
//                else
//                {
//                    indexOfEnemy = Random.Range(0, EnemiesToSpawn.Count);
//                }
                int leftOrRight = Random.Range(0, 2);
                if (leftOrRight == 0)
                {
                    
                    GameObject newEnemy = Instantiate(EnemiesToSpawn[indexOfEnemy]) as GameObject;
                    newEnemy.transform.position = LeftSpawnPoint.position;

                    var enemyComponent = newEnemy.GetComponent<Enemy>();

                    enemyComponent.speed += enemySpawnCounter / 30.0f;

                    if (enemyComponent.EnemyColor == EnemyColor.red)
                    {
                        enemyComponent.DieParticleSystem = CurrentRedParticleSystem;
                    }
                    else if (enemyComponent.EnemyColor == EnemyColor.green)
                    {
                        enemyComponent.DieParticleSystem = CurrentGreenParticleSystem;
                    }
                    else
                    {
                        enemyComponent.DieParticleSystem = CurrentBlueParticleSystem;
                    }

                    EnemyManager.Instance.AddEnemyToLeftQueue(enemyComponent);

                }
                else
                {
                    GameObject newEnemy = Instantiate(EnemiesToSpawn[indexOfEnemy]) as GameObject;
                    newEnemy.transform.position = RightSpawnPoint.position;

                    var enemyComponent = newEnemy.GetComponent<Enemy>();

                    enemyComponent.speed += enemySpawnCounter / 30.0f;

                    if (enemyComponent.EnemyColor == EnemyColor.red)
                    {
                        enemyComponent.DieParticleSystem = CurrentRedParticleSystem;
                    }
                    else if (enemyComponent.EnemyColor == EnemyColor.green)
                    {
                        enemyComponent.DieParticleSystem = CurrentGreenParticleSystem;
                    }
                    else
                    {
                        enemyComponent.DieParticleSystem = CurrentBlueParticleSystem;
                    }

                    EnemyManager.Instance.AddEnemyToRightQueue(enemyComponent);
                }
                timer = 0;
                enemySpawnCounter++;
                if (enemySpawnCounter%unitsInWave == 0)
                {
                    timeThreshold *= 4;
                }
                else
                {
                    timeThreshold = 0.5f - enemySpawnCounter / 1000.0f;
                    if (timeThreshold < 0.1f)
                    {
                        timeThreshold = 0.1f;
                    }
                }

                if (enemySpawnCounter%(unitsInWave*unitsInWave)==0)
                {
                    unitsInWave++;
                }
            }
        }
	}

    public void ChangeParticleSystems(GameObject red, GameObject green, GameObject blue)
    {
        CurrentRedParticleSystem = red;
        CurrentGreenParticleSystem = green;
        CurrentBlueParticleSystem = blue;
    }
}
