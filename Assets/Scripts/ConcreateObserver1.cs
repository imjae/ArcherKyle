using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConcreateObserver1 : Observer
{
    GameObject obj;

    public ConcreateObserver1(GameObject obj)
    {
        this.obj = obj;
    }


    // ���Ÿ���� Ŭ�������� �̸޼ҵ带 �����Ŵ
    public override void OnNotify(int num)
    {
        int num2 = obj.gameObject.GetComponent<ConcreateSubject>().getNum();

        Debug.Log("������ Ŭ������ �޼��� ���� #1");
        Debug.Log("�޼����� �Ķ��Ʈ : " + num);
        Debug.Log("��ü ������ ���� ���� : " + num2);
    }
}
