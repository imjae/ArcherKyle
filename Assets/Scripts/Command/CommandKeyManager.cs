using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandKeyManager : MonoBehaviour
{
    List<KeyCode> checkKeyList;
    Queue<Dictionary<KeyCode, float>> commandQueue;

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

        commandQueue = new Queue<Dictionary<KeyCode, float>>();
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
                var dict = new Dictionary<KeyCode, float> { { key, GameManager.Instance.playeTime } };
                commandQueue.Enqueue(dict);
            }
        }
    }

    private void PrintQueue(Queue<Dictionary<KeyCode, float>> q)
    {
        // string text = "";
        foreach (Dictionary<KeyCode, float> dict in q)
        {

            // text += (key + ",");
        }
    }
}
