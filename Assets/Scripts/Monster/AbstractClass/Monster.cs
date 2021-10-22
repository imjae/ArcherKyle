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

    // 달리기 상태일때 동작
    protected virtual void OnRunStatus() { }
    // IDLE 상태일때 동작
    protected virtual void OnIdleStatus() { }
    // IDLE 상태일때 동작
    protected virtual void OnHitStatus() { }
    // 공격
    protected virtual void Attack() { }

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

    // 공격 플래그 변수 TRUE
    public virtual void IsAttackingTrue()
    {
        IsAttacking = true;
    }
    // 공격 플래그 변수 FALSE
    public virtual void IsAttackingFalse()
    {
        IsAttacking = false;
    }
}
