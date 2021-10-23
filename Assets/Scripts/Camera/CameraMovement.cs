using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public enum VIEW
    {
        ONE,
        THIRD,
        NONE
    }

    // 카메라가 따라가야할 오브젝트의 위치
    public Transform objectToFollow;
    // 현제 카메라 시점
    public VIEW curView;
    // 카메라가 따라가는 속도
    public float followSpeed = 10f;
    // 마우스 감도
    public float sensitivity = 100f;
    // 마우스 움직임 제한 각도
    public float clampAngle = 70f;

    // 마우스 인풋을 받을 변수
    private float rotX;
    private float rotY;
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

    // 실제 카메라 위치
    public Transform realCamera;


    void Start()
    {
        curView = VIEW.THIRD;
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

    void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, objectToFollow.position, followSpeed * Time.deltaTime);
        finalDirection = transform.TransformPoint(directionNomalized * maxDistance);

        RaycastHit hit;

        // Debug.DrawRay(transform.position, finalDirection * 100f, Color.red);
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

    public void TransCamersView()
    {
        if (curView.Equals(VIEW.ONE))
        {
            minDistance = 0f;
            maxDistance = 0f;
            transform.GetChild(0).localRotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else if (curView.Equals(VIEW.THIRD))
        {
            minDistance = 1f;
            maxDistance = 3f;
            transform.GetChild(0).localRotation = Quaternion.Euler(15.75f, 0f, 0f);
        }
    }
}
