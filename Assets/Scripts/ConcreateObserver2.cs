using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������ ���� Ŭ����
public class ConcreateObserver2 : Observer
{
    GameObject obj;

    public ConcreateObserver2(GameObject obj)
    {
        this.obj = obj;
    }


    // ���Ÿ���� Ŭ�������� �� �޼ҵ带 ���� ��Ŵ
    public override void OnNotify(int num)
    {
        int num2 = obj.gameObject.GetComponent<ConcreateSubject>().getNum();

        Debug.Log("������ Ŭ������ �޼��� ���� #2");
        Debug.Log("�޼����� �Ķ��Ʈ : " + num);
        Debug.Log("��ü ������ ���� ���� : " + num2);
    }
}
