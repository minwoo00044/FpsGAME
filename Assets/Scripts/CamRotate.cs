using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//마우스 입력 받아서 카메라를 회전
//마우스입력 x,y, 속도
//순서 1 : 사용자의 마우스 입력을 받는다.
//순서 2 : 마우스 입력에 따라 회전 방향을 설정한다.
//순서 3 : 물체를 회전시킨다.
public class CamRotate : MonoBehaviour
{
    //마우스입력 x,y, 속도
    public float speed = 200f;
    float mx = 0;
    float my = 0;
    void Update()
    {
        if (GameManager.Instance.gameState != GameManager.GameState.Start)
            return;
        //순서 1 : 사용자의 마우스 입력(x,y)을 받는다.
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        mx += mouseX * speed * Time.deltaTime;
        my += mouseY * speed * Time.deltaTime;

        my = Mathf.Clamp(my, -90, 90);
        //순서 2 : 마우스 입력에 따라 회전 방향을 설정한다.
        Vector3 dir = new Vector3(-my, mx, 0);
        //순서 3 : 물체를 회전시킨다.
        // r = r0 + vt

        transform.eulerAngles = dir;
    }
}
