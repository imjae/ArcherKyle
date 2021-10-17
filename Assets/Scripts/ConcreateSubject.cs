using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConcreateSubject : MonoBehaviour
{
    // List<Observer> observers = new List<Observer>();
    private int myNum;

    delegate void NotiHandler(int num);
    NotiHandler _notiHandler;

    // �������� ���������� ����
    public void Notify()
    {
        _notiHandler(myNum);
    }

    // // ������ ������ ���
    // public void AddObserver(Observer observer)
    // {
    //     observers.Add(observer);
    // }

    // // �������� ������ ����
    // public void RemoveObserver(Observer observer)
    // {
    //     if (observers.IndexOf(observer) > 0) observers.Remove(observer);
    // }

    private void Start()
    {
        myNum = 10;

        Observer obj1 = new ConcreateObserver1(this.gameObject);
        Observer obj2 = new ConcreateObserver2(this.gameObject);

        _notiHandler += new NotiHandler(obj1.OnNotify);
        _notiHandler += new NotiHandler(obj2.OnNotify);

        _notiHandler(3);
    }

    public int getNum()
    {
        return myNum;
    }
}
