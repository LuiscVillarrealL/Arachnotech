using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using static SoundManagerScript;

public class TurretScript : MonoBehaviour
{

    Camera cam;
    public float zRange = 100f;

    Vector3 mousePos;

    [SerializeField]
    public GameObject laserPrefab;
    public GameObject shootingPoint;
    //private LineRenderer lineRenderer;
    private Vector3[] startingLineRendererPoints;
    [SerializeField]
    private Vector3[] newLineRendererPoints;
    private GameObject spawnedLaser;
    private Ray ray;
    public float maxLaserRange;
    public int laserOffsetRange = 11;

    private bool isShooting;
    public int damage = 5;

    public LayerMask ignoreLayers;

    private PlayerController playerController;

    public GameObject sparksPrefab;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        mousePos = new Vector3();

        spawnedLaser = Instantiate(laserPrefab, shootingPoint.transform) as GameObject;
        spawnedLaser.SetActive(false);
        LineRenderer lineRenderer = spawnedLaser.GetComponentInChildren<LineRenderer>();
        Debug.Log(lineRenderer);
        
        
        
        startingLineRendererPoints = new Vector3[2];
        newLineRendererPoints = new Vector3[2];
        lineRenderer.GetPositions(startingLineRendererPoints);
        lineRenderer.GetPositions(newLineRendererPoints);
        //maxLaserRange = 100;
        isShooting = false;


        //ignores layer
        ignoreLayers = ~ignoreLayers;

        playerController = GetComponentInParent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {

        if (playerController.isAlive)
        {
            mousePos = Input.mousePosition;
            mousePos.z = zRange;
            ray = Camera.main.ScreenPointToRay(mousePos);
            mousePos = cam.ScreenToWorldPoint(mousePos);
            Debug.DrawRay(transform.position, mousePos - transform.position, Color.red);

            transform.LookAt(mousePos);






            if (Input.GetMouseButtonDown(0))
            {
                EnableLaser();
            }

            if (Input.GetMouseButton(0))
            {
                UpdateLaser();
            }

            if (Input.GetMouseButtonUp(0))
            {
                DisableLaser();
            }

            LaserHit();

        }


    }

    private void LaserHit()
    {
        if (isShooting)
        {
            SoundManagerScript.instance.PlaySound(soundsEnum.laser);

            LineRenderer lineRenderer = spawnedLaser.GetComponentInChildren<LineRenderer>();
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, maxLaserRange, ignoreLayers))
            {
                Debug.Log("something hit " + hit.collider.gameObject.name);
                newLineRendererPoints[1] = new Vector3(newLineRendererPoints[1].x,
                    newLineRendererPoints[1].y, hit.distance - laserOffsetRange);

                if (hit.collider.tag == "Enemy")
                {
                    Debug.Log("enemy hit ");
                    if (hit.collider.gameObject.TryGetComponent<HealthScript>(out var health))
                    {
                        Instantiate(sparksPrefab, hit.collider.transform.position, Quaternion.identity);
                        if (health.Hp >0)
                        {
                            health.Damage(damage);
                        }
                        
                    }
                    
                }
            }
            else
            {

                newLineRendererPoints[1] = new Vector3(newLineRendererPoints[1].x,
                    newLineRendererPoints[1].y, maxLaserRange);
            }
            lineRenderer.SetPositions(newLineRendererPoints);
        }
        SoundManagerScript.instance.StopSound(soundsEnum.laser);
    }

    private void DisableLaser()
    {
        isShooting = false;
        spawnedLaser.SetActive(false);
    }

    private void UpdateLaser()
    {
        if (shootingPoint != null)
        {
            spawnedLaser.transform.position = shootingPoint.transform.position;
            
        }
    }

    private void EnableLaser()
    {
        isShooting = true;
        spawnedLaser.SetActive(true);

    }
}
