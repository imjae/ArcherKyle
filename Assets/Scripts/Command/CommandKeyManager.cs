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

        // ��ü(this)�� üũ�ؾ��� Ű ����Ʈ
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
        // interval�� ���� �ð��� �ʰ��ϸ� commandQueue Ŭ����
        if (this.keyDownIntervalTime > 0.3f && this.commandQueue.Count > 0)
        {
            // Debug.Log("ť �ʱ�ȭ");
            this.commandQueue.Clear();
        }
        // Ư�� Ű���� ������ �ð� ������ ������ �ش�.
        SetKeyDownIntervalTime();

        foreach (var dic in commandDictionary)
        {
            // dic.Key Ŀ�ǵ� ť�� �Ҵ�ȴ�.
            // dic.Value() �� �ش� Ŀ�ǵ� ť�� ��ġ�ϸ� ������ �Լ��̴�.
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
                // Debug.Log($"ť�� {key} �Է�");
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
