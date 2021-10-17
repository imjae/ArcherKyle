using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 옵저버 구현 클래스
public class ConcreateObserver2 : Observer
{
    GameObject obj;

    public ConcreateObserver2(GameObject obj)
    {
        this.obj = obj;
    }


    // 대상타입의 클래스에서 이 메소드를 실행 시킴
    public override void OnNotify(int num)
    {
        int num2 = obj.gameObject.GetComponent<ConcreateSubject>().getNum();

        Debug.Log("옵저버 클래스의 메서드 실행 #2");
        Debug.Log("메서드의 파라미트 : " + num);
        Debug.Log("객체 변수를 통한 접근 : " + num2);
    }
}
