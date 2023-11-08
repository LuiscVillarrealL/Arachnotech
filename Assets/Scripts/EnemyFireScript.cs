using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemyFireScript : MonoBehaviour
{
    public int ammo;
    public int reloadNum = 3;
    public Transform shootingSpot;
    public Renderer shootRenderer;
    public Rigidbody missile;
    public float reloadTime = 2f;
    private float reloadTimer;
    public float shotSpeed = 5f;

    public bool shootButton;
    public bool reload;
    public Transform player;

    public Material materialBLue;
    public Material materialRed;

    // Start is called before the first frame update
    void Start()
    {
        ammo = 0;
        reloadTimer = 0f;
       

        

        Debug.Log(shootRenderer);
    }

    // Update is called once per frame
    void Update()
    {
        reloadTimer += Time.deltaTime;

        //for debugging
        if (shootButton)
        {
            shoot(player);
            shootButton = false;
        }

        if (reload)
        {
            Reload();
            reload = false;
        }

        if (ammo > 0)
        {
            shootRenderer.material = materialBLue;
        }
        else
        {
            shootRenderer.material = materialRed;
        }
    }

    public void shoot(Transform target)
    {
        if(ammo > 0 && reloadTimer >= reloadTime)
        {
            reloadTimer = 0f;
            ammo--;
            Rigidbody newMissile = Instantiate(missile, shootingSpot.transform.position, missile.transform.rotation);

            Vector3 _direction = (target.position - transform.position).normalized;
            newMissile.AddForce(_direction * shotSpeed, ForceMode.Impulse);
            


        }
    }

    public void Reload()
    {
        ammo = reloadNum;
    }


}
