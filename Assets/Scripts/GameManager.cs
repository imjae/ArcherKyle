using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private float _playtime;
    private static GameManager _instance;
    public float PlayTime { get { return _playtime; } set { _playtime = value; } }


    // 게임메니저에서 가지고 있을 최상위 계층 오브젝트들 나열
    public GameObject canvas;
    public GameObject robotKyle;
    public GameObject warningPanel;
    public GameObject warningHit;

    public Text timeText;

    float _sec;
    int _min;

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

        PlayTime = 0f;

        canvas = GameObject.Find("Canvas").gameObject;
        robotKyle = GameObject.Find("Robot Kyle").gameObject;

        warningPanel = canvas.transform.Find("WarningPanel").gameObject;
        warningHit = canvas.transform.Find("WarningHit").gameObject;
        timeText = canvas.transform.Find("Time").GetChild(0).GetComponent<Text>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(SceneManager.GetActiveScene().name.Equals("Map_v1"))
        {
            PlayTime += Time.deltaTime;
            Timer();
        }
    }


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

    public IEnumerator BlinkWarningPanel()
    {
        int count = 0;
        while (count < 4)
        {
            this.warningPanel.SetActive(false);
            yield return new WaitForSeconds(.5f);
            this.warningPanel.SetActive(true);
            yield return new WaitForSeconds(.5f);
            count++;
        }
        this.warningPanel.SetActive(false);
    }
    public IEnumerator BlinkWarningHit()
    {
        int count = 0;
        while (count < 1)
        {
            this.warningHit.SetActive(true);
            yield return new WaitForSeconds(.3f);
            this.warningHit.SetActive(false);
            count++;
        }
    }

    public void SetTimeScale(float time)
    {
        Time.timeScale = time;
        Time.fixedDeltaTime = 0.02f * time;
    }
}
