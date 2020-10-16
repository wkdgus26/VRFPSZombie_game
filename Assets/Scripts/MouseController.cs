using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour {
    [SerializeField]
    private float lookSens;

    [SerializeField]
    private float cameraRotationLimit;

    private float currentCamera = 0;
    private float currentCameraY = 0;
    
    private Rigidbody myRigid;
    void Start () {
        myRigid = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }
	
	// Update is called once per frame
	void Update () {
        CameraRotation();
    }

    private void CameraRotation()
    {
        float xRotation = Input.GetAxis("Mouse Y"); // x축회전 y축방향 보기
        float yRotation = Input.GetAxis("Mouse X"); // y축회전 x축방형보기
        float cameraRotationX = xRotation * lookSens;
        float cameraRotationY = yRotation * lookSens;
        currentCamera -= cameraRotationX;
        currentCamera = Mathf.Clamp(currentCamera, -cameraRotationLimit, cameraRotationLimit);
        currentCameraY += cameraRotationY;
        transform.localEulerAngles = new Vector3(currentCamera, currentCameraY, 0f);
        
    }
}
