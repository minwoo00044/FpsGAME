using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���콺 �Է� �޾Ƽ� ī�޶� ȸ��
//���콺�Է� x,y, �ӵ�
//���� 1 : ������� ���콺 �Է��� �޴´�.
//���� 2 : ���콺 �Է¿� ���� ȸ�� ������ �����Ѵ�.
//���� 3 : ��ü�� ȸ����Ų��.
public class CamRotate : MonoBehaviour
{
    //���콺�Է� x,y, �ӵ�
    public float speed = 200f;
    float mx = 0;
    float my = 0;
    void Update()
    {
        if (GameManager.Instance.gameState != GameManager.GameState.Start)
            return;
        //���� 1 : ������� ���콺 �Է�(x,y)�� �޴´�.
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        mx += mouseX * speed * Time.deltaTime;
        my += mouseY * speed * Time.deltaTime;

        my = Mathf.Clamp(my, -90, 90);
        //���� 2 : ���콺 �Է¿� ���� ȸ�� ������ �����Ѵ�.
        Vector3 dir = new Vector3(-my, mx, 0);
        //���� 3 : ��ü�� ȸ����Ų��.
        // r = r0 + vt

        transform.eulerAngles = dir;
    }
}
