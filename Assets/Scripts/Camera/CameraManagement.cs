using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManagement : MonoBehaviour
{
    private static List<Camera> _cameraList;
    private static CameraManagement _instance;

    // �̱��� ������ ����ϱ� ���� �ν��Ͻ� ����
    // �ν��Ͻ��� �����ϱ� ���� ������Ƽ
    public static CameraManagement Camera
    {
        get
        {
            // �ν��Ͻ��� ���� ��쿡 �����Ϸ� �ϸ� �ν��Ͻ��� �Ҵ�.
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(CameraManagement)) as CameraManagement;

                if (_instance == null)
                    Debug.Log("no Singleton Camera Obj");
            }
            return _instance;
        }
    }

    public static List<Camera> CameraList
    {
        get { return _cameraList; }
        set { _cameraList = value; }
    }

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        // �ν��Ͻ��� �����ϴ� ��� ���λ���� �ν��Ͻ��� �����Ѵ�.
        else if (_instance != this)
            Destroy(gameObject);

        if (_cameraList == null)
            _cameraList = new List<Camera>();
    }

    // ���� ������ �� ī�޶� ���
    public void EnrollFaceCamera(Camera camera)
    {
        if (camera != null)
            CameraList.Add(camera);

        Debug.Log($"ī�޶� ����Ʈ ������ : {CameraList.Count}");
    }

    public void RemoveCamera(Camera camera)
    {
        CameraList.Remove(camera);
    }

    // ��ü ��ȯ�Ͽ� ���Ͱ� �׾� ����� ī�޶� ����
    public void RefreshCameraList()
    {
        foreach (Camera camera in CameraList)
        {
            if (camera == null)
                CameraList.Remove(camera);
        }
    }

    public void AllFaceCameraOff()
    {
        foreach (Camera camera in CameraList)
        {
            if (camera != null)
                camera.enabled = false;
        }
    }
}
