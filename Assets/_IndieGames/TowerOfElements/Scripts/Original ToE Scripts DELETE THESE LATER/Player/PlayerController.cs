using UnityEngine;
using System.Collections;

public enum ActionType { EarthActionLeft = 0, EarthActionRight = 1, LightActionLeft = 2, LightActionRight = 3, FireActionLeft = 4, FireActionRight = 5 }

public class PlayerController : MonoBehaviour {

    protected Animator mageAnimator;
    private float actionTimer;
    private float PositionLeft = -3.35f;
    private float PositionRight = 3.35f;

    protected virtual void Start()
    {
        mageAnimator = GetComponent<Animator>();
        if (mageAnimator == null) 
            Debug.LogError("Current Character is not correctly set up!");
    }

    protected virtual void Update()
    {
        ReturnMageToStartingPositionAfterTime();
    }

    public void PerformAction(ActionType action)
    {
        RelocateMageOnAction(action);

        ResetTrigger();

        StartAnimationOnAction(action);

        AffectEnemiesOnAction(action);
    }

    private void ResetTrigger()
    {
        mageAnimator.SetTrigger("StopActions");
        mageAnimator.ResetTrigger("EarthAction");
        mageAnimator.ResetTrigger("FireAction");
        mageAnimator.ResetTrigger("LightAction");
        mageAnimator.ResetTrigger("StopActions");
    }

    protected virtual void RelocateMageOnAction(ActionType action)
    {
        actionTimer = 0;

        if (action == ActionType.EarthActionLeft || action == ActionType.FireActionLeft || action == ActionType.LightActionLeft)
            transform.position = new Vector3(PositionLeft, transform.position.y, transform.position.z);
        else
            transform.position = new Vector3(PositionRight, transform.position.y, transform.position.z);
    }

    protected virtual void StartAnimationOnAction(ActionType action)
    {
        if (action == ActionType.LightActionLeft || action == ActionType.LightActionRight)
        {
            mageAnimator.SetTrigger("LightAction");
            AudioManager.Instance.PlayLightSound();
//            GUIManager.Instance.BlueAbilityBlink.PlayOne();
        }
        else if (action == ActionType.FireActionLeft || action == ActionType.FireActionRight)
        {
            mageAnimator.SetTrigger("FireAction");
            AudioManager.Instance.PlayFireSound();
        }
        else
        {
            mageAnimator.SetTrigger("EarthAction");
            AudioManager.Instance.PlayEarthSound();
            Camera.main.GetComponent<ShakeCamera>().Shake();
        }
    }

    protected virtual void AffectEnemiesOnAction(ActionType action)
    {
        EnemyColor enemyColor;
        if (action == ActionType.EarthActionLeft || action == ActionType.FireActionLeft || action == ActionType.LightActionLeft)
        {
            if (EnemyManager.Instance.CheckLeftQueueForGoblins())
                enemyColor = EnemyManager.Instance.PeekEnemyFromLeftQueue().EnemyColor;
            else
                return;

            if ((action == ActionType.EarthActionLeft && enemyColor == EnemyColor.green) || (action == ActionType.FireActionLeft && enemyColor == EnemyColor.red) || (action == ActionType.LightActionLeft && enemyColor == EnemyColor.blue))
            {
                if (EnemyManager.Instance.PeekEnemyFromLeftQueue().GetCurrentLife() == 1)
                {
                    EnemyManager.Instance.PeekEnemyFromLeftQueue().Damage();
                    EnemyManager.Instance.DeleteEnemyFromLeftQueue();
                }
                else
                {
                    EnemyManager.Instance.PeekEnemyFromLeftQueue().Damage();
                }
            }
            else
            {
//                TestGameManager.Instance.DecreaseMultiplier();
            }
        }
        else
        {
            if (EnemyManager.Instance.CheckRightQueueForGoblins())
                enemyColor = EnemyManager.Instance.PeekEnemyFromRightQueue().EnemyColor;
            else
                return;

            if ((action == ActionType.EarthActionRight && enemyColor == EnemyColor.green) || (action == ActionType.FireActionRight && enemyColor == EnemyColor.red) || (action == ActionType.LightActionRight && enemyColor == EnemyColor.blue))
            {
                if (EnemyManager.Instance.PeekEnemyFromRightQueue().GetCurrentLife() == 1)
                {
                    EnemyManager.Instance.PeekEnemyFromRightQueue().Damage();
                    EnemyManager.Instance.DeleteEnemyFromRightQueue();
                }
                else
                {
                    EnemyManager.Instance.PeekEnemyFromRightQueue().Damage();
                }
            }
            else
            {
//                TestGameManager.Instance.DecreaseMultiplier();
            }
        }
    }

    private void ReturnMageToStartingPositionAfterTime()
    {
        actionTimer += Time.deltaTime;
        if (actionTimer > 1 && transform.position.x != 0)
        {
            transform.position = new Vector3(0, transform.position.y, transform.position.z);
        }
    }
}
