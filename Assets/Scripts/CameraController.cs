using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public GameObject player;
    private Vector3 offset;

    public Vector2 turn;

    public float pLerp = .02f;
    public float rLerp = .01f;


    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - player.transform.position;
    }

    
    void LateUpdate()
    {
        transform.position = player.transform.position + offset;
       // transform.rotation = Quaternion.Lerp(transform.rotation, player.transform.rotation, rLerp);
    }
}
