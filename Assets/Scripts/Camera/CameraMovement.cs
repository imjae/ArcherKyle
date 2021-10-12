using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    // 카메라가 따라가야할 오브젝트의 위치
    public Transform objectToFollow;
    // 카메라가 따라가는 속도
    public float followSpeed = 10f;
    // 마우스 감도
    public float sensitivity = 100f;
    // 마우스 움직임 제한 각도
    public float clampAngle = 70f;

    // 마우스 인풋을 받을 변수
    private float rotX;
    private float rotY;
    // 실제 카메라 위치
    public Transform realCamera;
    // 카메라 방향
    public Vector3 directionNomalized;
    // 최종적으로 셋팅될 카메라 방향
    public Vector3 finalDirection;

    // 최소, 최대 거리
    public float minDistance;
    public float maxDistance;
    // 최종 계산된 거리
    public float finalDistance;
    // 카메라 움직임의 부드러움 정도
    public float smoothness = 10f;


    void Start()
    {
        rotX = transform.localRotation.eulerAngles.x;
        rotY = transform.localRotation.eulerAngles.y;
        // Debug.Log($"{rotX} / {rotY}");

        directionNomalized = realCamera.localPosition.normalized;
        finalDistance = realCamera.localPosition.magnitude;
        // Debug.Log($"{directionNomalized} / {finalDistance}");

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // 카메라의 회전
        rotX += -(Input.GetAxis("Mouse Y")) * sensitivity * Time.deltaTime;
        rotY += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;

        rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);
        Quaternion rot = Quaternion.Euler(rotX, rotY, 0);
        transform.rotation = rot;
    }

    private void LateUpdate()
    {
        
    }

    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    void FixedUpdate()
    {
        // 카메라의 움직임
        transform.position = Vector3.MoveTowards(transform.position, objectToFollow.position, followSpeed * Time.deltaTime);

        finalDirection = transform.TransformPoint(directionNomalized * maxDistance);

        RaycastHit hit;

        if (Physics.Linecast(transform.position, finalDirection, out hit))
        {
            finalDistance = Mathf.Clamp(hit.distance, minDistance, maxDistance);
        }
        else
        {
            finalDistance = maxDistance;
        }
        realCamera.localPosition = Vector3.Lerp(realCamera.localPosition, directionNomalized * finalDistance, Time.deltaTime * smoothness);
    }
}
