using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateManager : MonoBehaviour
{

    [SerializeField]
    public EnemyBaseState currentState;
    public EnemyAttackState attackState = new EnemyAttackState();
    public EnemyDeadState deadState = new EnemyDeadState();
    public EnemyIdleState idleState = new EnemyIdleState();
    public EnemyRunAwayState runAwayState = new EnemyRunAwayState();
    public EnemyWanderState wanderState = new EnemyWanderState();
    public EnemyRunToWeapon runToWeapon = new EnemyRunToWeapon();

    private AINav aiNav;
    private DetectScript detectScript;
    public EnemyFireScript enemyFireScript;
    public HealthScript healthScript;


    // Start is called before the first frame update
    void Start()
    {
        aiNav = GetComponent<AINav>();
        detectScript = GetComponent<DetectScript>();
        enemyFireScript = GetComponent<EnemyFireScript>();
        healthScript = GetComponent<HealthScript>();

        currentState = idleState;
        currentState.EnterState(this, aiNav, detectScript);

        
        
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this, aiNav, detectScript);
       // Debug.Log(currentState);
    }

   public void SwitchState(EnemyBaseState nextState)
    {
        currentState= nextState;
        nextState.EnterState(this, aiNav, detectScript);
    }
}
