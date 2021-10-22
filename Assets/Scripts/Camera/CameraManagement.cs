using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManagement : MonoBehaviour
{
    private static List<Camera> _cameraList;
    private static CameraManagement _instance;

    // 싱글톤 패턴을 사용하기 위한 인스턴스 변수
    // 인스턴스에 접근하기 위한 프로퍼티
    public static CameraManagement Camera
    {
        get
        {
            // 인스턴스가 없는 경우에 접근하려 하면 인스턴스를 할당.
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
        // 인스턴스가 존재하는 경우 새로생기는 인스턴스를 삭제한다.
        else if (_instance != this)
            Destroy(gameObject);

        if (_cameraList == null)
            _cameraList = new List<Camera>();
    }

    // 몬스터 리스폰 시 카메라 등록
    public void EnrollFaceCamera(Camera camera)
    {
        if (camera != null)
            CameraList.Add(camera);

        Debug.Log($"카메라 리스트 사이즈 : {CameraList.Count}");
    }

    public void RemoveCamera(Camera camera)
    {
        CameraList.Remove(camera);
    }

    // 전체 순환하여 몬스터가 죽어 사라진 카메라 삭제
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
