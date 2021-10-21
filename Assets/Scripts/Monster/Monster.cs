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

    // ���Ϳ� ������ Range�� ���� �ȿ� �浹�Ͼ� ���� ��쿡 ���� �ൿ ����
    protected virtual void DetectionInRange(float radius, Action<Collider> action)
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
        for (int i = 0; i < hitColliders.Length; i++)
        {
            Collider detectObject = hitColliders[i++];
            action(detectObject);
        }
    }

    protected virtual void Attack()
    {

    }
}
