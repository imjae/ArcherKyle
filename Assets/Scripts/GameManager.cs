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


    public GameObject warningPanel;
    public GameObject warningHit;

    public Text timeText;

    float _sec;
    int _min;

    // �̱��� ������ ����ϱ� ���� �ν��Ͻ� ����
    // �ν��Ͻ��� �����ϱ� ���� ������Ƽ
    public static GameManager Instance
    {
        get
        {
            // �ν��Ͻ��� ���� ��쿡 �����Ϸ� �ϸ� �ν��Ͻ��� �Ҵ�.
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
        // �ν��Ͻ��� �����ϴ� ��� ���λ���� �ν��Ͻ��� �����Ѵ�.
        else if (_instance != this)
            Destroy(gameObject);
        // �Ʒ��� �Լ��� ����Ͽ� ���� ��ȯ�Ǵ��� ����Ǿ��� �ν��Ͻ��� �ı����� �ʴ´�.
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
