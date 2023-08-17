using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//목적 : 폭탄이 물체에 부딪히면 폭탄이 이펙트를 만들고 파괴된다.
public class BombAction : MonoBehaviour
{
    public GameObject bombEffect;
    private void OnCollisionEnter(Collision other)
    {
        GameObject bombEffectGO = Instantiate(bombEffect);
        bombEffectGO.transform.position = transform.position;
        Destroy(gameObject);
    }
}
