using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseTest : MonoBehaviour {

    public float mouse_speedX = 3.0f;    //마우스 좌우
    public float mouse_speedY = 3.0f;    //마우스 상하
    float rotationY = 0f;

    // Use this for initialization
    void Start()
    {
        if (GetComponent<Rigidbody>())
            GetComponent<Rigidbody>().freezeRotation = true;     //지면에 붙어있어도 중심축을 기준으로 회전 가능하게 한다.
    }

    // Update is called once per frame
    void LateUpdate()
    {

        float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * mouse_speedX;      // 마우스 좌우 회전 시키는 이벤트

        //마우스 상하 움직이기
        rotationY -= Input.GetAxis("Mouse Y") * mouse_speedY;    // +=로 하면 마우스 반전 상하로 바뀌게됨
        rotationY = Mathf.Clamp(rotationY, -20.0f, 60.0f);       //상하 범위 제한 시키기, 왜냐하면 위로 향하는데 360도로 돌기때문.

        transform.localEulerAngles = new Vector3(rotationY, rotationX, 0);
    }
}
