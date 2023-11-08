using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.WSA;
using System.Reflection;
using static SoundManagerScript;

public class PlayerController : MonoBehaviour
{

    //Object initialization
    private Rigidbody rb;

    //movement
    private float movementX;
    private float movementY;
    public float speed = 1f;
    public Vector2 turn;
    public float sensitivity = .5f;

    public bool isAlive;

    public Transform torso;

    public GameObject explosionParticle;


    // Start is called before the first frame update
    void Awake()
    {
        
        rb = GetComponent<Rigidbody>();
        isAlive = true;

    }

    private void Start()
    {
        
    }

    private void FixedUpdate()
    {

        //turn.y += Input.GetAxis("Mouse Y") * sensitivity;
        //turn.x += Input.GetAxis("Mouse X") * sensitivity;

        if (isAlive)
        {
            turn.y += Input.GetAxis("Mouse Y") * sensitivity;
            turn.x += movementX * sensitivity;
            transform.localRotation = Quaternion.Euler(0, turn.x, 0);

            if (rb.velocity.magnitude < speed)
            {
                Vector3 movement = new Vector3(0.0f, 0.0f, movementY);

                rb.AddRelativeForce(movement * speed * Time.fixedDeltaTime * 1000f);
            }

        }





    }

    public void died()
    {
        isAlive = false;


        //torso.rotation = new Quaternion(10f, 20f, 0, 1);
        //torso.position = new Vector3(0, -0.74f, 0);

        StartCoroutine(DeadAnimation());

    }



    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;
        movementY = movementVector.y;

    }


    IEnumerator DeadAnimation()
    {
        SoundManagerScript.instance.PlaySound(soundsEnum.explosion);
        //Vector3 iniTorsoPos = torso.position;
        Quaternion iniTorsorot = torso.rotation;
        float elapsedTime = 0.0f;

        Vector3 finalTorsoPos = new Vector3(0, -0.74f, 0);
        Quaternion finalTorsoRos = new Quaternion(10f, 20f, 0, 1);

        while (elapsedTime < 2) {
            Instantiate(explosionParticle, torso.transform.position, Quaternion.identity);
            torso.rotation = Quaternion.Slerp(iniTorsorot, finalTorsoRos, elapsedTime / 2);
            //torso.position = Vector3.Slerp(iniTorsoPos, finalTorsoPos, fracComplete);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }







}
