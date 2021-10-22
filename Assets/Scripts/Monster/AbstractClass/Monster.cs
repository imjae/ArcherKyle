using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Monster : Character
{
    private string _monsterName;
    private Animator _animator;
    private Transform _player;
    private NavMeshAgent _agent;
    private HealthSystem _healthSystem;
    private Camera _faceCamera;

    private float _detectionTime;
    private float _detectionIntervalTime;

    private bool _isAttacking;
    private bool _isDie;

    private IEnumerator _detection;

    public string MonsterName
    {
        get { return _monsterName; }
        set { _monsterName = value; }
    }
    protected Animator Animator
    {
        get { return _animator; }
        set { _animator = value; }
    }
    protected Transform Player
    {
        get { return _player; }
        set { _player = value; }
    }
    protected NavMeshAgent Agent
    {
        get { return _agent; }
        set { _agent = value; }
    }
    protected HealthSystem Health
    {
        get { return _healthSystem; }
        set { _healthSystem = value; }
    }

    protected float DetectionTime
    {
        get { return _detectionTime; }
        set { _detectionTime = value; }
    }

    protected float DetectionIntervalTime
    {
        get { return _detectionIntervalTime; }
        set { _detectionIntervalTime = value; }
    }

    protected bool IsAttacking
    {
        get { return _isAttacking; }
        set { _isAttacking = value; }
    }
    protected bool IsDie
    {
        get { return _isDie; }
        set { _isDie = value; }
    }
    protected Camera FaceCamera
    {
        get { return _faceCamera; }
        set { _faceCamera = value; }
    }
    protected IEnumerator Detection
    {
        get { return _detection; }
        set { _detection = value; }
    }

    // �߰� ���ǰ� �ʿ� ���� ��� virtual Ű���� ������ ����.
    protected void DetectionLocationTarget(Transform target)
    {
        _agent.SetDestination(target.position);
    }

    // TODO ���� ���� �ڵ�
    // ���Ϳ� ������ Range�� ���� �ȿ� �浹�Ͼ� ���� ��쿡 ���� �ൿ ����
    protected void DetectionInRange(float radius, Action<Collider> action)
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
        for (int i = 0; i < hitColliders.Length; i++)
        {
            Collider detectObject = hitColliders[i++];
            action(detectObject);
        }
    }

    // ������ �÷��̾� ���� ���� ��ƾ
    protected IEnumerator DetectionRoutine()
    {
        // ���� �ʾ��� ���� �÷��̾� ����
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

    // �޸��� �����϶� ����
    protected virtual void OnRunStatus()
    {
        Agent.enabled = true;
        DetectionLocationTarget(Player);
        Animator.SetTrigger("RunTrigger");
    }
    // IDLE �����϶� ����
    protected virtual void OnIdleStatus()
    {
        Animator.SetTrigger("IdleTrigger");
        Agent.enabled = false;
        Agent.velocity = Vector3.zero;
    }
    // IDLE �����϶� ����
    protected virtual void OnHitStatus()
    {
        Animator.SetTrigger("HitTrigger");
        Agent.enabled = false;
        Agent.velocity = Vector3.zero;
    }
    // ����
    protected virtual void Attack()
    {
        Agent.enabled = false;
        Agent.velocity = Vector3.zero;
        Animator.SetTrigger("AttackTrigger");
    }
    // ����
    protected virtual void Die()
    { // �������̴� �ִϸ��̼� Ʈ���� ���� ����
        Animator.ResetTrigger("AttackTrigger");
        Animator.ResetTrigger("RunTrigger");
        Animator.ResetTrigger("IdleTrigger");
        Animator.ResetTrigger("HitTrigger");
        Animator.SetTrigger("DieTrigger");

        IsDie = true;
        Agent.velocity = Vector3.zero;
        Agent.enabled = false;
        IsAttacking = false;

        StopCoroutine(Detection);
    }

    protected virtual void SelfDestroy()
    {
        Destroy(gameObject);
    }

    // �ǰݴ��� ����
    protected virtual Vector3 GetHitDiretion(Transform target)
    {
        return (transform.position - target.position).normalized;
    }

    // ���� �÷��� ���� TRUE
    public virtual void IsAttackingTrue()
    {
        IsAttacking = true;
    }
    // ���� �÷��� ���� FALSE
    public virtual void IsAttackingFalse()
    {
        IsAttacking = false;
    }
}
