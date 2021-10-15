using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandKeyManager : MonoBehaviour
{
    List<KeyCode> checkKeyList;
    Queue<KeyCode> commandQueue;

    Dictionary<Queue<KeyCode>, Action> commandDictionary;

    private bool isFirstKeyDown = true;
    private float keyDownTime = 0f;
    private float keyUpTime = 0f;
    private float keyDownIntervalTime = 0f;

    // Start is called before the first frame update
    void Start()
    {

        // 객체(this)가 체크해야할 키 리스트
        checkKeyList = new List<KeyCode>(){
            KeyCode.W,
            KeyCode.A,
            KeyCode.S,
            KeyCode.D,
            KeyCode.Space,
            KeyCode.LeftShift
        };

        commandQueue = new Queue<KeyCode>();
        commandDictionary = new Dictionary<Queue<KeyCode>, Action>();
    }

    // Update is called once per frame
    void Update()
    {
        // interval이 기준 시간을 초과하면 commandQueue 클리어
        if (this.keyDownIntervalTime > 0.3f && this.commandQueue.Count > 0)
        {
            // Debug.Log("큐 초기화");
            this.commandQueue.Clear();
        }
        // 특정 키들이 눌릴때 시간 간격을 셋팅해 준다.
        SetKeyDownIntervalTime();

        foreach (var dic in commandDictionary)
        {
            // dic.Key 커맨드 큐가 할당된다.
            // dic.Value() 가 해당 커맨드 큐가 일치하면 동작할 함수이다.
        }
    }

    void FixedUpdate()
    {
        
    }

    private void SetKeyDownIntervalTime()
    {
        foreach (KeyCode key in this.checkKeyList)
        {
            if (Input.GetKeyDown(key))
            {
                // Debug.Log($"큐에 {key} 입력");
                commandQueue.Enqueue(key);
                PrintQueue(commandQueue);

                this.keyDownTime = GameManager.Instance.playeTime;
                this.keyDownIntervalTime = keyDownTime - keyUpTime;
            }

            if (Input.GetKeyUp(key))
            {
                this.keyUpTime = GameManager.Instance.playeTime;
            }
        }
    }

    private void PrintQueue(Queue<KeyCode> q)
    {
        string text = "";
        foreach (KeyCode key in q)
        {
            text += (key + ",");
        }

        // Debug.Log(text);
    }
}
