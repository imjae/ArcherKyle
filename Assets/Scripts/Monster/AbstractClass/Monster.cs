using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Monster : Character
{
    private string _monsterName;
    private string _monsterWeak;

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
    public string MonsterWeak
    {
        get { return _monsterWeak; }
        set { _monsterWeak = value; }
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
    public HealthSystem Health
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

    // 추가 정의가 필요 없는 경우 virtual 키워드 붙이지 않음.
    protected void DetectionLocationTarget(Transform target)
    {
        _agent.SetDestination(target.position);
    }

    // TODO 영상 삽입 코드
    // 몬스터에 설정된 Range값 범위 안에 충돌일어 났을 경우에 취할 행동 정의
    protected void DetectionInRange(float radius, Action<Collider> action)
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
        for (int i = 0; i < hitColliders.Length; i++)
        {
            Collider detectObject = hitColliders[i++];
            action(detectObject);
        }
    }

    // 달리기 상태일때 동작
    protected virtual void OnRunStatus()
    {
        Agent.enabled = true;
        DetectionLocationTarget(Player);
        Animator.SetTrigger("RunTrigger");
    }
    // 달리기 상태일때 동작
    protected virtual void OnRunStatus(Transform target)
    {
        Agent.enabled = true;
        DetectionLocationTarget(target);
        Animator.SetTrigger("RunTrigger");
    }
    // IDLE 상태일때 동작
    protected virtual void OnIdleStatus()
    {
        Animator.SetTrigger("IdleTrigger");
        Agent.enabled = false;
        Agent.velocity = Vector3.zero;
    }
    // IDLE 상태일때 동작
    protected virtual void OnHitStatus()
    {
        Animator.SetTrigger("HitTrigger");
        Agent.enabled = false;
        Agent.velocity = Vector3.zero;
    }
    // 공격
    protected virtual void Attack()
    {
        Agent.enabled = false;
        Agent.velocity = Vector3.zero;
        Animator.SetTrigger("AttackTrigger");
    }
    // 죽음
    protected virtual void Die()
    {
        // 실행중이던 애니메이션 트리거 전부 종료
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

        // Face Camera 리스크에서 제거
        CameraManagement.Camera.RemoveCamera(FaceCamera);
    }

    protected virtual void SelfDestroy()
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

    private void OnParticleCollision(GameObject other)
    {
        // if (other.name.Equals("FireballExplosionRed(Clone)"))
        // {
        //     Debug.Log(name);
        //     if (!IsDie)
        //     {
        //         OnHitStatus();
        //         Health.TakeDamage(70);
        //     }
        // }

        // if (other.name.Equals("IceBigExplosion(Clone)"))
        // {
        //     Vector3 originVelocity = Agent.velocity;
        //     Debug.Log(name);
        //     if (!IsDie)
        //     {
        //         Health.TakeDamage(50);
        //         Agent.enabled = false;
        //         Agent.velocity = Vector3.zero;
        //         StartCoroutine(SwitchDelay(originVelocity));
        //     }
        // }
    }

    private IEnumerator SwitchDelay(Vector3 originSpeed)
    {
        yield return new WaitForSeconds(3f);
        Agent.enabled = true;
        Agent.velocity = originSpeed;
    }
}
