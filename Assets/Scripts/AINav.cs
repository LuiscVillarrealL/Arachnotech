using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.VR;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.WSA;
using static SoundManagerScript;

public class AINav : MonoBehaviour
{

    public Transform player;
    public bool hasWeapon;
    public bool isRotating;

    public bool hasTargetPos;

    private NavMeshAgent agent;

    public float wanderRadius;
    public float idleTimer;
    public float timer;
    public float closeDistance;
    public float wanderTimer;
    public float wanderTimerMax;


    private DetectScript detectScript;
    [SerializeField] public float displacementDist = 5f;

    public Vector3 newPos;

    public GameObject explosionParticle;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        detectScript = GetComponent<DetectScript>();
        hasWeapon = false;
        hasTargetPos = false;
        isRotating = false;
        newPos = new Vector3();
        timer = 0;
        wanderTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {


    }


    public void Wander()
    {
        hasTargetPos = true;
        newPos = RandomNavSphere(transform.position, wanderRadius, -1);
        agent.SetDestination(newPos);

    }

    public void Idling()
    {
        if (!isRotating)
        {
            isRotating = true;
            StartCoroutine(RotateIdle());
        }

    }

    public void RunAway()
    {

        Vector3 normDir = (player.position - transform.position).normalized;

        // normDir = Quaternion.AngleAxis(Random.Range(0, 179), Vector3.up) * normDir;

        agent.SetDestination(transform.position - (normDir * displacementDist));
    }

    public void lookAt(Vector3 lookPoint)
    {
        Vector3 _direction = (lookPoint - transform.position).normalized;

        //create the rotation we need to be in to look at the target
        Quaternion _lookRotation = Quaternion.LookRotation(_direction);
        transform.rotation = _lookRotation;
    }

    public void RunTo(Vector3 runToPoint)
    {
        lookAt(runToPoint);

        agent.SetDestination(runToPoint);
    }

    public void StopRotating()
    {
        StopCoroutine("RotateIdle");
    }
    IEnumerator RotateIdle()
    {

        //sine function parameters
        float a = 0.5f;
        float h = -3f;
        float b = 1f;
        float k = 0.5f;
        float timerPos = 0;
        float timeElapsed = 0;

        float insideSin = 0;


        Quaternion startRotation = transform.rotation;
        Quaternion targetRotationFirst = transform.rotation * Quaternion.Euler(0, 90, 0);
        Quaternion targetRotationLast = transform.rotation * Quaternion.Euler(0, -90, 0);
        while (timeElapsed < idleTimer)
        {
            insideSin = (timeElapsed - h) / b;
            timerPos = a * Mathf.Sin(insideSin) + k;
            if (timeElapsed < idleTimer)
            {
                isRotating = true;

                transform.rotation = Quaternion.Slerp(targetRotationFirst, targetRotationLast, timerPos);



            }

            timeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.rotation = startRotation;
        isRotating = false;
    }


    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }


    public Vector3 GetNewPos()
    {
        return newPos;
    }

    public void StartDeadAnim() {
        SoundManagerScript.instance.PlaySound(soundsEnum.explosion);
        StartCoroutine("DeadEnemyAnim");
    }

    IEnumerator DeadEnemyAnim()
    {

        Quaternion iniTorsorot = transform.rotation;
        float elapsedTime = 0.0f;

        Quaternion finalRot = new Quaternion(Random.Range(0f, 10f), Random.Range(0f, 20f), 0, 1);

        while (elapsedTime < 3f)
        {
            Instantiate(explosionParticle, transform.position, Quaternion.identity);
            transform.rotation = Quaternion.Slerp(iniTorsorot, finalRot, elapsedTime / 3f);
            
            elapsedTime += Time.deltaTime * 3f;
            yield return null;
        }

        this.gameObject.SetActive(false);
    }

}
