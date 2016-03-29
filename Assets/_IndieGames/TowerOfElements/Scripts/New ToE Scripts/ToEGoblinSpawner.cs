using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TowerOfElements
{
	public class ToEGoblinSpawner : MonoBehaviour 
	{
		public List<GameObject> mEnemiesToSpawn;

		[SerializeField] private Transform mLeftSpawn;
		[SerializeField] private Transform mRightSpawn;

		public ToEManager mManager;
		public List<ToEGoblin> mGoblinsRight = new List<ToEGoblin>();
		public List<ToEGoblin> mGoblinsLeft = new List<ToEGoblin>();

		int mWaveNumber = 1;
		int mSpawnCounter = 0;
		int mWaveSpawns = 3;

		public bool mAllowToSpawn = true;

		public float mTimeThreshold = 0.5f;
		private float mTimer = 0;


		// Use this for initialization
		void Start () 
		{
			
		}//END of Start()
		
		// Update is called once per frame
		void Update () 
		{
			if (mAllowToSpawn) //Spawn stuff if it can
			{
				mTimer += Time.deltaTime; //Increment timer

				//Spawn stuff if time to spawn
				if (mTimer > mTimeThreshold)
				{
					//Figure out which enemy to spawn
					int indexOfEnemy = 0;
					//Start with green enemy
					if (mManager.mScore < 40)
					{
						indexOfEnemy = 0;
					}
					//Then add the blue enemy
					else if (mManager.mScore < 120)
					{
						indexOfEnemy = Random.Range(0, mEnemiesToSpawn.Count-1);
					}
					//Then add the red enemy
					else
					{
						indexOfEnemy = Random.Range(0, mEnemiesToSpawn.Count);
					}

					//Decide which side to spawn on
					int leftOrRight = Random.Range(0, 2);
					//Spawn an enemy on the Left
					if (leftOrRight == 0)
					{
						//Spawn and set up appropriate variables
						GameObject newEnemy = Instantiate(mEnemiesToSpawn[indexOfEnemy], mLeftSpawn.position, Quaternion.identity) as GameObject;

						var enemyComponent = newEnemy.GetComponent<ToEGoblin>();
						
						enemyComponent.mSpeed += mSpawnCounter / 120.0f;
						enemyComponent.mRightRope = false;
						enemyComponent.mGoblinSpawner = this;
						enemyComponent.mManager = mManager;

						mGoblinsLeft.Add(enemyComponent);

					}
					//Spawn an enemy on the Right
					else
					{
						
						//Spawn and set up appropriate variables
						GameObject newEnemy = Instantiate(mEnemiesToSpawn[indexOfEnemy], mRightSpawn.position, Quaternion.identity) as GameObject;

						var enemyComponent = newEnemy.GetComponent<ToEGoblin>();
						
						enemyComponent.mSpeed += mSpawnCounter / 120.0f;
						enemyComponent.mRightRope = true;
						enemyComponent.mGoblinSpawner = this;
						enemyComponent.mManager = mManager;

						mGoblinsRight.Add(enemyComponent);
						
					}

					//Reset timer and note that another thing was spawned
					mTimer = 0;
					mSpawnCounter++;

					//Decide whether or not to increase the spawn speed
					if (mSpawnCounter%mWaveSpawns == 0)
					{
						mTimeThreshold *= 4;
					}
					else
					{
						mTimeThreshold = 0.5f - mSpawnCounter / 1600.0f;
						if (mTimeThreshold < 0.1f)
						{
							mTimeThreshold = 0.1f;
						}
					}
					
					if (mSpawnCounter%(mWaveSpawns*mWaveSpawns)==0)
					{
						mWaveSpawns++;
					}
				}
			}
		}//END of Update()
	}
}
