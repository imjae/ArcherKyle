using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OperationKeyManager : MonoBehaviour
{
    Queue<KeyCode> commendQueue;

    Dictionary<Queue<KeyCode>, Action> commandDictionary;

    // Start is called before the first frame update
    void Start()
    {
        commendQueue = new Queue<KeyCode>();
        commandDictionary = new Dictionary<Queue<KeyCode>, Action>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            foreach (var dic in commandDictionary)
            {
                // dic.Key Ŀ�ǵ� ť�� �Ҵ�ȴ�.
                // dic.Value() �� �ش� Ŀ�ǵ� ť�� ��ġ�ϸ� ������ �Լ��̴�.
            }
        }
    }

    IEnumerator CommendRemover()
    {
        WaitForSeconds wait = new WaitForSeconds(2f);

        while (true)
        {
            yield return wait;
            if (this.commendQueue.Count > 0)
                this.commendQueue.Dequeue();
        }
    }
}
