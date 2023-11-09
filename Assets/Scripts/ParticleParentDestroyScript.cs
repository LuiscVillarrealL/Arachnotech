using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ParticleParentDestroyScript : MonoBehaviour
{
    float runTime = 0;

    public void Start()
    {
        StartCoroutine(DestroyParticles());
    }


    IEnumerator DestroyParticles()
    {
        
        while (runTime < 2)
        {

            runTime += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }


    }
