using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private float _playtime;
    private static GameManager _instance;
    public float PlayeTime { get { return _playtime; } set { _playtime = value; } }

    public Text timeText;
    // 싱글톤 패턴을 사용하기 위한 인스턴스 변수
    // 인스턴스에 접근하기 위한 프로퍼티
    public static GameManager Instance
    {
        get
        {
            // 인스턴스가 없는 경우에 접근하려 하면 인스턴스를 할당.
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(GameManager)) as GameManager;

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
        // 인스턴스가 존재하는 경우 새로생기는 인스턴스를 삭제한다.
        else if (_instance != this)
            Destroy(gameObject);
        // 아래의 함수를 사용하여 씬이 전환되더라도 선언되었던 인스턴스가 파괴되지 않는다.
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        PlayeTime += Time.deltaTime;
        Timer();
    }

    float _sec;
    int _min;

    private void Timer()
    {
        _sec += Time.deltaTime;
        timeText.text = string.Format("{0:D2}:{1:D2}", _min, (int)_sec);
        if ((int)_sec > 59)
        {
            _sec = 0;
            _min++;
        }
    }
    private void OnGUI()
    {
        string timeString;
        timeString = "" + PlayeTime.ToString("00.00");
        timeString = timeString.Replace(".", ":");
        timeText.text = timeString;
    }



}
