using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;
using Vector3 = UnityEngine.Vector3;

public class ProceduralSpiderMovement : MonoBehaviour
{
    //initial variables
    public Transform[] targets;
    public float stepSize = 1f;
    public int smoothness = 1;
    public float stepHeight = 0.1f;
    public float stepSpeed = 5f;
    public bool bodyOrientation = true;

    [SerializeField]
    private float raycastRange = 1f;
    [SerializeField]
    private Vector3[] defaultLegPositions;
    [SerializeField]
    private Vector3[] lastLegPositions;
    [SerializeField]
    private Vector3[] newLegPositions;
    private Vector3[] newLegPositionsHit;
    private Vector3 lastBodyUp;
    private Vector3[] directionLine;
    private bool[] legMoving;
    private bool[] moveTurn;
    private int nbLegs;
    private float[] lerp;

    private Vector3 velocity;
    private Vector3 lastVelocity;
    private Vector3 lastBodyPos;

    private float velocityMultiplier = 15f;

    public int groundLayerNum = 6;
    private LayerMask groundLayer;


    // Start is called before the first frame update
    void Start()
    {
        lastBodyUp = transform.up;

        nbLegs = targets.Length;
        defaultLegPositions = new Vector3[nbLegs];
        lastLegPositions = new Vector3[nbLegs];
        newLegPositions = new Vector3[nbLegs];
        directionLine = new Vector3[nbLegs];
        newLegPositionsHit = new Vector3[nbLegs];
        legMoving = new bool[nbLegs];
        moveTurn = new bool[nbLegs];
        lerp = new float[nbLegs];   


        for (int i = 0; i < nbLegs; ++i)
        {

            defaultLegPositions[i] = targets[i].localPosition;
            //defaultLegPositions[i] = targets[i].position;
            lastLegPositions[i] = targets[i].position;
            newLegPositions[i] = lastLegPositions[i];
            newLegPositionsHit[i] = newLegPositions[i];
            lerp[i] = 1;
            legMoving[i] = false;
            moveTurn[i] = false;
        }
        lastBodyPos = transform.position;

        groundLayer = 1 << groundLayerNum;

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        velocity = transform.position - lastBodyPos;
        velocity = (velocity + smoothness * lastVelocity) / (smoothness + 1f);

        if (velocity.magnitude < 0.000025f)
            velocity = lastVelocity;
        else
            lastVelocity = velocity;



        float maxDistance = stepSize;



        for (int i = 0; i < nbLegs; ++i)
        {
            var target = targets[i];
            Ray ray = new Ray(target.position + (Vector3.up * 5), Vector3.down);
            Debug.DrawRay(ray.origin, ray.direction * 10);



            if (Physics.Raycast(ray, out RaycastHit hit, 10, groundLayer))
            {

                newLegPositionsHit[i] = transform.TransformPoint(defaultLegPositions[i]);
                newLegPositionsHit[i].y = hit.point.y;
                newLegPositions[i] = newLegPositionsHit[i];

                //directionLine[i] = Vector3.ProjectOnPlane(desiredPositions[i] + velocity * velocityMultiplier - lastLegPositions[i], transform.up);

                float distance = Vector3.ProjectOnPlane(newLegPositions[i] + velocity * velocityMultiplier - lastLegPositions[i], transform.up).magnitude;

                if (distance > maxDistance && !legMoving[i])
                {
                    // maxDistance = distance;
                    // indexToMove = i;
                    legMoving[i] = true;
                    lerp[i] = 0f;
                    newLegPositions[i] = newLegPositionsHit[i];
                    //Debug.Log("Distance " + distance);
                    Debug.Log("Distance " + distance);
                }


                if (lerp[i] < 1)
                {
                    Vector3 targetPos = Vector3.Lerp(lastLegPositions[i], newLegPositions[i], lerp[i]);
                    targetPos.y += Mathf.Sin(lerp[i] * Mathf.PI) * stepHeight;
                    target.position = targetPos;

                    lerp[i] = lerp[i] += Time.deltaTime * stepSpeed;
                }
                else
                {
                    legMoving[i] = false;
                    lastLegPositions[i] = newLegPositions[i];
                    target.position = newLegPositions[i];
                }



                //if (i != indexToMove)
                //{
                //    target.position = lastLegPositions[i];
                    
                //}
                //else if(i == indexToMove && lerp[i] < 1)
                //{
                //    Vector3 targetpos = Vector3.Lerp(lastLegPositions[i], newLegPositions[i], lerp[i]);
                //    target.position = targetpos;

                //    lerp[i] = lerp[i] += Time.deltaTime * speed;
                //    Debug.Log("Lerp " + i + " " + lerp[i] );
                //    // lastLegPositions[i] = newLegPositions[i];
                //    //tLegPositions[i] = Vector3.Lerp(); hit.point;
                //}



                // 
                //Debug.Log("lastLegPositions " + i + " " + lastLegPositions[i]);
                //Debug.Log("hit.point " + i + " " + hit.point);
                //if (Vector3.Distance(newLegPositions[i], hit.point) > stepSize)
                //{
                //    indexToMove = i;
                //    lastLegPositions[i] = Vector3.Lerp(); hit.point;
                //}

               

                lastBodyPos = transform.position;
            }



            //target.position = defaultLegPositions[i];
        }
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < nbLegs; ++i)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(lastLegPositions[i], 0.5f);
            Gizmos.color = Color.green;
           // Debug.Log("TransformPoint " + i + " " + transform.TransformPoint(defaultLegPositions[i]));
            Gizmos.DrawWireSphere(transform.TransformPoint(defaultLegPositions[i]), stepSize);
            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(newLegPositions[i], 0.5f);

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(lastLegPositions[i], newLegPositions[i]);

        }
    }

}
