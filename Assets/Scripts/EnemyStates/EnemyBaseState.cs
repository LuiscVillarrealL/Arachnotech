using System;
using UnityEngine;

public abstract class EnemyBaseState
{

  public abstract void EnterState(EnemyStateManager enemy, AINav ai, DetectScript detectScript);
   
  public abstract void UpdateState(EnemyStateManager enemy, AINav ai, DetectScript detectScript);

}
