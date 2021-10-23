using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seeker : Monster
{
    public enum Type
    {
        Skeleton
    }

    // 달리기 상태일때 동작
    protected override void OnRunStatus(Transform target)
    {
        Agent.enabled = true;
        DetectionLocationTarget(target);
        Animator.SetTrigger("RunTrigger");
    }

    // 몬스터의 플레이어 감지 동작 루틴
    protected virtual IEnumerator DetectionRoutine(Transform target)
    {
        // 죽지 않았을 때만 플레이어 감지
        while (!IsDie)
        {
            if (!IsAttacking)
            {
                OnIdleStatus();
                yield return new WaitForSeconds(DetectionTime);
                OnRunStatus(target);
                yield return new WaitForSeconds(DetectionIntervalTime);
            }
            yield return null;
        }
    }


    // 공격
    protected override void Attack()
    {
        Agent.enabled = false;
        Agent.velocity = Vector3.zero;
        Animator.SetTrigger("Skill1Trigger");
    }

    protected virtual void Skill2()
    {
        Agent.enabled = false;
        Agent.velocity = Vector3.zero;
        Animator.SetTrigger("Skill2Trigger");
    }
}
