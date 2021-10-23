using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어를 쫓는 추격꾼
public class Chaser : Monster
{
    public enum Type
    {
        Archer,
        Swordsman,
        Mage,
        Grunt,
        King
    }

    // 몬스터의 플레이어 감지 동작 루틴
    protected virtual IEnumerator DetectionRoutine()
    {
        // 죽지 않았을 때만 플레이어 감지
        while (!IsDie)
        {
            if (!IsAttacking)
            {
                OnIdleStatus();
                yield return new WaitForSeconds(DetectionTime);
                OnRunStatus();
                yield return new WaitForSeconds(DetectionIntervalTime);
            }
            yield return null;
        }
    }
}
