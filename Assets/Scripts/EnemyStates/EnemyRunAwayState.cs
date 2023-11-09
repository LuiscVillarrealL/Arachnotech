using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRunAwayState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager enemy, AINav ai, DetectScript detectScript)
    {
        ai.RunAway();
    }

    public override void UpdateState(EnemyStateManager enemy, AINav ai, DetectScript detectScript)
    {

        if (enemy.healthScript.Hp <= 0)
        {
            enemy.SwitchState(enemy.deadState);
        }

        if (detectScript.SawPickup())
        {
            enemy.SwitchState(enemy.runToWeapon);
        }

        if (enemy.enemyFireScript.ammo > 0 && detectScript.PlayerInRange())
        {
            enemy.SwitchState(enemy.attackState);
        }

        if (!detectScript.DetectedPlayer())

        {
            enemy.SwitchState(enemy.idleState);
        }

    }
}
