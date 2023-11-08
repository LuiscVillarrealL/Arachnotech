using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager enemy, AINav ai, DetectScript detectScript)
    {
        if (detectScript.PlayerInRange())
        {
            ai.lookAt(detectScript.playerTransform().position);
            enemy.enemyFireScript.shoot(detectScript.playerTransform());
        }
        else
        {

        }
        
    }

    public override void UpdateState(EnemyStateManager enemy, AINav ai, DetectScript detectScript)
    {

        if (enemy.healthScript.Hp <= 0)
        {
            enemy.SwitchState(enemy.deadState);
        }

        if (enemy.enemyFireScript.ammo > 0 && detectScript.PlayerInRange())
        {
            enemy.enemyFireScript.shoot(detectScript.playerTransform());
        }
        else
        {
            enemy.SwitchState(enemy.runAwayState);
        }
    }
}
