using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeadState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager enemy, AINav ai, DetectScript detectScript)
    {
        ai.StartDeadAnim();

    }

    public override void UpdateState(EnemyStateManager enemy, AINav ai, DetectScript detectScript)
    {
        
    }

}
