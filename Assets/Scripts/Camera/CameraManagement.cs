using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManagement : MonoBehaviour
{
    private List<Camera> cameraList;
    private static CameraManagement _instance;

    // �̱��� ������ ����ϱ� ���� �ν��Ͻ� ����
    // �ν��Ͻ��� �����ϱ� ���� ������Ƽ
    public static CameraManagement Instance
    {
        get
        {
            // �ν��Ͻ��� ���� ��쿡 �����Ϸ� �ϸ� �ν��Ͻ��� �Ҵ�.
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(CameraManagement)) as CameraManagement;

                if (_instance == null)
                    Debug.Log("no Singleton obj");
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        // �ν��Ͻ��� �����ϴ� ��� ���λ���� �ν��Ͻ��� �����Ѵ�.
        else if (_instance != this)
            Destroy(gameObject);
    }

    private void Start()
    {
        cameraList = new List<Camera>();
    }

    // ���� ������ �� ī�޶� ���
    public void EnrollFaceCamera(Camera camera)
    {
        if (camera != null)
            cameraList.Add(camera);
    }

    public void RemoveCamera(Camera camera)
    {
        cameraList.Remove(camera);
    }

    // ��ü ��ȯ�Ͽ� ���Ͱ� �׾� ����� ī�޶� ����
    public void RefreshCameraList()
    {
        foreach (Camera camera in cameraList)
        {
            if (camera == null)
                cameraList.Remove(camera);
        }
    }

    public void AllFaceCameraOff()
    {
        foreach (Camera camera in cameraList)
        {
            if (camera != null)
                camera.enabled = false;
        }
    }
}
