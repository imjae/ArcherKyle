using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Monster : Character
{
    public string monsterName { get; set; }
    protected Animator _animator { get; set; }
    protected Transform player { get; set; }
    protected NavMeshAgent _agent { get; set; }

    protected virtual void DetectionLocationTarget(Transform target)
    {
        _agent.SetDestination(target.position);
    }

    // 몬스터에 설정된 Range값 범위 안에 충돌일어 났을 경우에 취할 행동 정의
    protected virtual void DetectionInRange(float radius, Action<Collider> action)
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
        for (int i = 0; i < hitColliders.Length; i++)
        {
            Collider detectObject = hitColliders[i++];
            action(detectObject);
        }
    }

    // 공격
    protected virtual void Attack()
    {

    }

    // 죽음. 델리게이트로 받은 동작 실행 후 스스로 Destory()
    protected virtual void Die()
    {
        Destroy(gameObject);
    }

    // 피격당한 방향
    protected virtual Vector3 GetHitDiretion(Transform target)
    {
        return (transform.position - target.position).normalized;
    }
}
