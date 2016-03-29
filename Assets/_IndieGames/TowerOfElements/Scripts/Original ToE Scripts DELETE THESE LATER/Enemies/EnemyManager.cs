using UnityEngine;
using System.Collections.Generic;
using System;

public enum EnemyDieParticleType { Default, MageNinja, MageSkull}

[Serializable]
public class EnemyDieParticle
{
    public EnemyDieParticleType type;
    public GameObject greenGoblinParticleSystem;
    public GameObject redGoblinParticleSystem;
    public GameObject blueGoblinParticleSystem;
}

public class EnemyManager : MonoBehaviour {

    public List<EnemyDieParticle> EnemyDieParticleList;
    public EnemyDieParticleType CurrentEnemyDieType
    {
        set
        {
            foreach (EnemyDieParticle part in EnemyDieParticleList)
            {
                if (part.type == value)
                {
                    EnemySpawner.Instance.ChangeParticleSystems(part.redGoblinParticleSystem, part.greenGoblinParticleSystem, part.blueGoblinParticleSystem);
                    break;
                }
            }
        }
    }

    Queue<Enemy> LeftQueue;
    Queue<Enemy> RightQueue;
    private int LeftQueueLenght;
    private int RightQueueLenght;

    public static EnemyManager Instance;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        LeftQueueLenght = 0;
        RightQueueLenght = 0;
        LeftQueue = new Queue<Enemy>();
        RightQueue = new Queue<Enemy>();
    }
	
	public void AddEnemyToLeftQueue(Enemy enemy)
    {
        ++LeftQueueLenght;
        LeftQueue.Enqueue(enemy);
    }

    public void AddEnemyToRightQueue(Enemy enemy)
    {
        ++RightQueueLenght;
        RightQueue.Enqueue(enemy);
    }

    public Enemy PeekEnemyFromLeftQueue()
    {
        if (LeftQueueLenght != 0)
            return LeftQueue.Peek();
        else
            return null;
    }

    public Enemy PeekEnemyFromRightQueue()
    {
        if (RightQueueLenght != 0)
            return RightQueue.Peek();
        else
            return null;
    }

    public void DeleteEnemyFromLeftQueue()
    {
        --LeftQueueLenght;
        LeftQueue.Dequeue();
    }

    public void DeleteEnemyFromRightQueue()
    {
        --RightQueueLenght;
        RightQueue.Dequeue();
    }

    public void StopEnemies()
    {
        EnemySpawner.Instance.RestartSpawner();
        int tmpi;
        tmpi = LeftQueue.Count;
        for (int i = 0; i < tmpi; i++)
        {
            LeftQueue.Dequeue().DestroyEnemy();
        }
        tmpi = RightQueue.Count;
        for (int i = 0; i < tmpi; i++)
        {
            RightQueue.Dequeue().DestroyEnemy();
        }
    }

    public bool CheckLeftQueueForGoblins()
    {
        if (LeftQueueLenght==0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public bool CheckRightQueueForGoblins()
    {
        if (RightQueueLenght==0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

}
