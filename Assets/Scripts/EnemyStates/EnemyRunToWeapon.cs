using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyRunToWeapon : EnemyBaseState
{

    public override void EnterState(EnemyStateManager enemy, AINav ai, DetectScript detectScript)
    {

        Vector3 pickup = new Vector3();
        pickup = detectScript.pickupTransform().transform.position;
        Debug.Log("Pickup: " + detectScript.visiblePickups.FirstOrDefault().gameObject.name + " pos: " + pickup);
        ai.RunTo(pickup);
    }

    public override void UpdateState(EnemyStateManager enemy, AINav ai, DetectScript detectScript)
    {
        if (enemy.healthScript.Hp <= 0)
        {
            enemy.SwitchState(enemy.deadState);
        }


        if (!detectScript.SawPickup())
        {
            if (detectScript.DetectedPlayer())
            {
                if (enemy.enemyFireScript.ammo > 0)
                {
                    enemy.SwitchState(enemy.attackState);
                }
                else
                {
                    enemy.SwitchState(enemy.runAwayState);
                }
                
            }
            else
            {
                enemy.SwitchState(enemy.idleState);
            }
        }
            
            
        
    }
}
