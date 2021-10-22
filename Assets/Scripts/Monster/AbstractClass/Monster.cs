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

    // �޸��� �����϶� ����
    protected virtual void OnRunStatus() { }
    // IDLE �����϶� ����
    protected virtual void OnIdleStatus() { }
    // IDLE �����϶� ����
    protected virtual void OnHitStatus() { }
    // ����
    protected virtual void Attack() { }

    // ����. ��������Ʈ�� ���� ���� ���� �� ������ Destory()
    protected virtual void Die()
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
