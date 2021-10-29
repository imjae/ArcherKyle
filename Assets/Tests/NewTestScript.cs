using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.TestTools;

public class NewTestScript
{
    // A Test behaves as an ordinary method
    // 테스트는 일반적인 방법으로 동작한다.
    [Test]
    public void NewTestScriptSimplePasses()
    {
        // Use the Assert class to test conditions
        // Assert 클래스를 사용하여 조건테스트
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    // UnityTest는 플레이 모드에서 코 루틴처럼 동작합니다. 편집 모드에서 사용할 수 있습니다.
    // 프레임을 건너 뛰려면 'yeild return null' 사용용
    [UnityTest]
    public IEnumerator 전기화살데미지테스트()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        // Assert 클래스를 사용하여 조건을 테스트
        // 프레임을 건너 뛰려면 'yeild return null' 사용

        var Obj = new GameObject();
        Obj.AddComponent<Text>();

        yield return null;

        var arrowScript = Obj.AddComponent<Arrow>();
        Debug.Log(arrowScript.attackPoint);
    }
}
