using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandKeyManager : MonoBehaviour
{
    Queue<KeyCode> commandQueue;

    Dictionary<Queue<KeyCode>, Action> commandDictionary;

    // Start is called before the first frame update
    void Start()
    {
        commandQueue = new Queue<KeyCode>();
        commandDictionary = new Dictionary<Queue<KeyCode>, Action>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            foreach (var dic in commandDictionary)
            {
                // dic.Key 커맨드 큐가 할당된다.
                // dic.Value() 가 해당 커맨드 큐가 일치하면 동작할 함수이다.
            }
        }
    }

    IEnumerator CommandRemover()
    {
        WaitForSeconds wait = new WaitForSeconds(2f);

        while (true)
        {
            yield return wait;
            if (this.commandQueue.Count > 0)
                this.commandQueue.Dequeue();
        }
    }
}
