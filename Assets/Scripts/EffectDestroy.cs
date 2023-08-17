using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectDestroy : MonoBehaviour
{
    float timer = 0;

    private void Update()
    {
        timer += Time.deltaTime;
        if(timer >= GetComponent<ParticleSystem>().main.duration)
        {
            Destroy(gameObject);
        }
    }
}
