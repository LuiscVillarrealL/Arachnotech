using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PickupScript : MonoBehaviour
{
    string userTag;
    // Start is called before the first frame update
    void Start()
    {
        userTag = "Enemy";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.tag);
        if (other.gameObject.CompareTag(userTag))
        {

            if (other.gameObject.TryGetComponent(out EnemyFireScript enemyFireScript))
            {
                enemyFireScript.Reload();
            }
            if (other.gameObject.TryGetComponent(out DetectScript detectScript))
            {
                detectScript.GotPickup();
                this.gameObject.SetActive(false);
            }



        }
    }


}
