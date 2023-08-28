using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//목적 : 폭탄이 물체에 부딪히면 폭탄이 이펙트를 만들고 파괴된다.
public class BombAction : MonoBehaviour
{
    public GameObject bombEffect;
    public float explosionRadius = 5f;
    public int damage = 5;
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("FxTemporaire") || other.gameObject.CompareTag("Player"))
        {
            return;
        }
        Collider[] cols = Physics.OverlapSphere(transform.position, explosionRadius, 1 << 8);
        for(int i = 0; i < cols.Length; i++)
        {
            cols[i].GetComponent<EnemyFSM>().DamageAction(damage);
        }
        GameObject bombEffectGO = Instantiate(bombEffect);
        bombEffectGO.transform.position = transform.position;
        Destroy(gameObject);
    }
}
