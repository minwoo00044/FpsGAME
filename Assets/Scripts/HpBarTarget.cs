using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//����: hpbar�� �չ����� Ÿ���� �� �������� ���Ѵ�.
//����Ӽ� : Ÿ��
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
