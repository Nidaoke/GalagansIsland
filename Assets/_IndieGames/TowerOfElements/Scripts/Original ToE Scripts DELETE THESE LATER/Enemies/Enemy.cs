using UnityEngine;
using System.Collections;

public enum EnemyColor { red, green, blue}

public class Enemy : MonoBehaviour {

    public float life;
    private float lifeCurrent;
    public float speed;
    public EnemyColor EnemyColor;

    public GameObject DieParticleSystem;

    void Start()
    {
        lifeCurrent = life;
    }

    public Enemy(EnemyColor color)
    {
        EnemyColor = color;
    }

    public void Damage()
    {
        lifeCurrent--;
        if (lifeCurrent <= 0)
        {
//            TestGameManager.Instance.IncreaseScore();
            GameObject particleSystem = Instantiate(DieParticleSystem);
            particleSystem.transform.position = gameObject.transform.position;
            Destroy(gameObject);
        }
    }

    public void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    void Update()
    {
        gameObject.transform.Translate(0, speed * Time.deltaTime, 0);
        //QQ
        if (gameObject.transform.position.y > 3.5f)
        {
//            TestGameManager.Instance.FinishLevel();
        }
    }

    public float GetCurrentLife()
    {
        return lifeCurrent;
    }
}
