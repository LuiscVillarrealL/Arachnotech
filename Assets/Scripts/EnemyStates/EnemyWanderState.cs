using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWanderState : EnemyBaseState
{


    public override void EnterState(EnemyStateManager enemy, AINav ai, DetectScript detectScript)
    {
        if (!ai.hasTargetPos)
        {
            ai.Wander();
        }
    }

    public override void UpdateState(EnemyStateManager enemy, AINav ai, DetectScript detectScript)
    {

        if (enemy.healthScript.Hp <= 0)
        {
            enemy.SwitchState(enemy.deadState);
        }

        if (detectScript.SawPickup() && enemy.enemyFireScript.ammo != enemy.enemyFireScript.reloadNum)
        {
            enemy.SwitchState(enemy.runToWeapon);
        }


        if (Vector3.Distance(ai.transform.position, ai.newPos) <= ai.closeDistance)
        {
            //Debug.Log("4");
            ai.timer = 0;
            ai.wanderTimer = 0;
            ai.hasTargetPos = false;
            enemy.SwitchState(enemy.idleState);
        }

        if (ai.wanderTimerMax <= ai.wanderTimer)
        {
            ai.wanderTimer = 0;
            ai.Wander();
        }
        else
        {
            ai.wanderTimer += Time.deltaTime;

        }

        if (detectScript.DetectedPlayer())
        {
            if (enemy.enemyFireScript.ammo <= 0)
            {
                ai.wanderTimer = 0;
                enemy.SwitchState(enemy.runAwayState);
            }
            else
            {
                ai.wanderTimer = 0;
                enemy.SwitchState(enemy.attackState);
            }

        }
    }
}
