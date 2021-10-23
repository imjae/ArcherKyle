using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seeker : Monster
{
    public enum Type
    {
        Skeleton
    }

    // �޸��� �����϶� ����
    protected override void OnRunStatus(Transform target)
    {
        Agent.enabled = true;
        DetectionLocationTarget(target);
        Animator.SetTrigger("RunTrigger");
    }

    // ������ �÷��̾� ���� ���� ��ƾ
    protected virtual IEnumerator DetectionRoutine(Transform target)
    {
        // ���� �ʾ��� ���� �÷��̾� ����
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


    // ����
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
