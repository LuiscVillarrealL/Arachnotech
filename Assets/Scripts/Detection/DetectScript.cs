using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEngine.TextCore.Text;
using System.Linq;

public class DetectScript : MonoBehaviour
{
    public float runTimeMax = 5;
    [SerializeField]
    private float runTime = 0;
    public int detectRadius = 5;
    [Range(0, 360)]
    public float viewAngle;

    public LayerMask playerLayer;
    private string playerTag;
    private string pickupTag;

    //public UnityEvent<bool> DetectedPlayer;

    [SerializeField]
    private bool detectedPlayer;
    [SerializeField]
    private bool inRangePlayer;
    [SerializeField]
    private bool isRunning;
    [SerializeField]
    private bool sawPickup;

    [HideInInspector]
    public Transform playerTransformPos;
    public List<Transform> visibleTargets = new List<Transform>();
    public List<Transform> visiblePickups = new List<Transform>();
    private HealthScript healthScript;

    public Renderer detectRender;
    public Material materialBLue;
    public Material materialRed;

    // Start is called before the first frame update
    void Start()
    {
        detectedPlayer = false;
        isRunning = false;
        sawPickup = false;
        inRangePlayer = false;
        playerTag = "Player";
        pickupTag = "Pickup";
        healthScript = GetComponent<HealthScript>();
    }

    // Update is called once per frame
    void Update()
    {

        if (detectedPlayer)
        {
            detectRender.material = materialRed;
        }
        else
        {
            detectRender.material = materialBLue;
        }


        if (visibleTargets.Count > 0 && !visibleTargets.ElementAt(0).gameObject.activeSelf)
        {
            visibleTargets.Clear();
        }
        //DetectedPlayer?.Invoke(detectedPlayer);
        if (!isRunning && !sawPickup)
        {
            inRangePlayer = false;
            detectedPlayer = false;
            visibleTargets.Clear();
            visiblePickups.Clear();
        }


        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectRadius);
        foreach (var hitCollider in hitColliders)
        {
            

            //if (hitCollider.gameObject.tag == playerTag)
            //{
            //    playerTransformPos = hitCollider.gameObject.transform;
            //    //inRangePlayer = true;
            //   // visibleTargets.Add(hitCollider.transform);
            //   // RunAwayCall();
            //}

            //if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
            //{

            if (hitCollider.gameObject.tag == playerTag || hitCollider.gameObject.tag == pickupTag)
            {
                Ray ray;
                Vector3 hitPosition = hitCollider.transform.position;
                float distance = Vector3.Distance(hitPosition, transform.position);
                Vector3 dirToTarget = (hitPosition - transform.position).normalized;

                ray = new Ray(transform.position, (hitPosition - transform.position));

                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, distance))
                {

                    if (hit.collider.gameObject.tag == pickupTag && !(visiblePickups.Count() > 0))
                    {
                        if (!visiblePickups.Contains(hit.transform))
                        {
                            visiblePickups.Add(hit.transform);
                        }

                        sawPickup = true;
                    }
                    else
                    if (hit.collider.gameObject.tag == playerTag && !(visibleTargets.Count() > 0))
                    {
                        playerTransformPos = hitCollider.gameObject.transform;
                        inRangePlayer = true;
                        visibleTargets.Add(hit.transform);
                        RunAwayCall();
                    }



                    //}

                }

            }

                
               // Debug.DrawRay(ray.origin, ray.direction * distance, Color.green);

        }


    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    public bool DetectedPlayer()
    {
        return detectedPlayer;
    }

    public bool SawPickup()
    {
        return sawPickup;
    }

    public bool PlayerInRange()
    {
        return inRangePlayer;
    }

    public void GotPickup()
    {
        sawPickup = false;
    }

    public Transform pickupTransform()
    {

        
        visiblePickups = visiblePickups.OrderBy(ch => (Vector3.Distance(transform.position, ch.position))).ToList();
        Debug.Log("Pickup: " + visiblePickups.FirstOrDefault().gameObject.name + " pos: " + visiblePickups.FirstOrDefault().position);
        return visiblePickups.FirstOrDefault();
    }

    public Transform playerTransform()
    {
        if (inRangePlayer)
        {
            return playerTransformPos;
        }
        else
        {
            visibleTargets = visibleTargets.OrderBy(ch => (Vector3.Distance(transform.position, ch.position))).ToList();
            //Debug.Log("Pickup: " + visiblePickups.FirstOrDefault().gameObject.name + " pos: " + visiblePickups.FirstOrDefault().position);
            return visibleTargets.FirstOrDefault();
        }


    }


    void OnDrawGizmos()
    {

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, detectRadius);
        Gizmos.color = Color.white;

        foreach (var target in visibleTargets)
        {
            Gizmos.DrawLine(transform.position, target.transform.position);
        }

        //Gizmos.DrawWireArc
    }


    public void RunAwayCall()
    {
        Debug.Log("runawaycall");
        runTime = 0;
        StartCoroutine(RunAway());
    }

    public void isHit()
    {
        detectedPlayer = true;
    }

    IEnumerator RunAway()
    {
        

        while (runTime < runTimeMax)
        {
            isRunning = true;
            detectedPlayer = true;
            runTime += Time.deltaTime;
            yield return null;
        }
        
        inRangePlayer = false;
        isRunning = false;
        detectedPlayer = false;

    }
}
