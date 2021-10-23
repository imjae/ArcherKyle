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

    // ī�޶� ���󰡾��� ������Ʈ�� ��ġ
    public Transform objectToFollow;
    // ���� ī�޶� ����
    public VIEW curView;
    // ī�޶� ���󰡴� �ӵ�
    public float followSpeed = 10f;
    // ���콺 ����
    public float sensitivity = 100f;
    // ���콺 ������ ���� ����
    public float clampAngle = 70f;

    // ���콺 ��ǲ�� ���� ����
    private float rotX;
    private float rotY;
    // ī�޶� ����
    public Vector3 directionNomalized;
    // ���������� ���õ� ī�޶� ����
    public Vector3 finalDirection;

    // �ּ�, �ִ� �Ÿ�
    public float minDistance;
    public float maxDistance;
    // ���� ���� �Ÿ�
    public float finalDistance;
    // ī�޶� �������� �ε巯�� ����
    public float smoothness = 10f;

    // ���� ī�޶� ��ġ
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
        // ī�޶��� ȸ��
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
