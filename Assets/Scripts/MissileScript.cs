using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SoundManagerScript;

public class MissileScript : MonoBehaviour
{
    public bool isShot;
    Rigidbody rb;
    public Transform target;
    private string playerTag;
    public int damage = 25;
    // Start is called before the first frame update
    void Start()
    {
        isShot = false;
        rb = GetComponent<Rigidbody>();
        playerTag = "Player";
    }


    public void setTarget(Transform shootTarget)
    {
        target = shootTarget;
        isShot = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        SoundManagerScript.instance.PlaySound(soundsEnum.explosion);
        if (collision.gameObject.CompareTag(playerTag))
        {
            
            if (collision.gameObject.TryGetComponent<HealthScript>(out var health))
            {
                health.Damage(damage);
            }
          //      this.gameObject.SetActive(false);

        }

        if (!collision.gameObject.CompareTag("Enemy"))
        {

            Debug.Log(collision.gameObject.name);
              //    this.gameObject.SetActive(false);
            Destroy(gameObject);

        }

        
    }
}
