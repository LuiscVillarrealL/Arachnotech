using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{


    public override void EnterState(EnemyStateManager enemy, AINav ai, DetectScript detectScript)
    {

        if (!ai.isRotating)
        {
            ai.Idling();
        }
    }

    public override void UpdateState(EnemyStateManager enemy, AINav ai, DetectScript detectScript)
    {
        ai.timer += Time.deltaTime;

        if (enemy.healthScript.Hp <= 0)
        {
            enemy.SwitchState(enemy.deadState);
        }

        if (detectScript.SawPickup() && enemy.enemyFireScript.ammo != enemy.enemyFireScript.reloadNum)
        {
            ai.StopRotating();
            enemy.SwitchState(enemy.runToWeapon);
        }
        if (ai.timer >= ai.idleTimer)
        {
            enemy.SwitchState(enemy.wanderState);
        }

        if (detectScript.DetectedPlayer())
        {
            if (enemy.enemyFireScript.ammo <= 0)
            {
                ai.StopRotating();
                enemy.SwitchState(enemy.runAwayState);
            }
            else
            {
                ai.StopRotating();
                enemy.SwitchState(enemy.attackState);
            }
            
        }


    }
}
