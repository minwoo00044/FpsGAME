using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//목적: hpbar의 앞방향이 타겟의 앞 방향으로 향한다.
//폴요속성 : 타겟
public class HpBarTarget : MonoBehaviour
{
    public Transform target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.forward = target.forward;
    }
}
