using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    // ī�޶� ���󰡾��� ������Ʈ�� ��ġ
    public Transform objectToFollow;
    // ī�޶� ���󰡴� �ӵ�
    public float followSpeed = 10f;
    // ���콺 ����
    public float sensitivity = 100f;
    // ���콺 ������ ���� ����
    public float clampAngle = 70f;

    // ���콺 ��ǲ�� ���� ����
    private float rotX;
    private float rotY;
    // ���� ī�޶� ��ġ
    public Transform realCamera;
    // ī�޶� ����
    public Vector3 directionNomalized;
    // ���������� ���õ� ī�޶� ����
    public Vector3 finalDirection;

    // �ּ�, �ִ� �Ÿ�
    public float minDistance;
    public float maxDixtance;
    // ���� ���� �Ÿ�
    public float finalDistance;
    // ī�޶� �������� �ε巯�� ����
    public float smoothness = 10f;


    void Start()
    {
        rotX = transform.localRotation.eulerAngles.x;
        rotY = transform.localRotation.eulerAngles.y;

        directionNomalized = realCamera.localPosition.normalized;
        finalDistance = realCamera.localPosition.magnitude;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        rotX += -(Input.GetAxis("Mouse Y")) * sensitivity * Time.deltaTime;
        rotY += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;

        rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);
        Quaternion rot = Quaternion.Euler(rotX, rotY, 0);
        transform.rotation = rot;
    }

    private void LateUpdate()
    {
        // ī�޶��� ������
        transform.position = Vector3.MoveTowards(transform.position, objectToFollow.position, followSpeed * Time.deltaTime);

        finalDirection = transform.TransformPoint(directionNomalized * maxDixtance);

        RaycastHit hit;

        if(Physics.Linecast(transform.position, finalDirection, out hit))
        {
            finalDistance = Mathf.Clamp(hit.distance, minDistance, maxDixtance);
        }
        else
        {
            finalDistance = maxDixtance;
        }
        realCamera.localPosition = Vector3.Lerp(realCamera.localPosition, directionNomalized * finalDistance, Time.deltaTime * smoothness);
    }
}