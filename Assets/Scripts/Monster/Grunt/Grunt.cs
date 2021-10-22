using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Grunt : Chaser
{
    private HealthSystem healthSystem;
    private GameObject target;

    private GameObject canvas;
    private MonsterStatusController monsterStatusController;

    private void Awake()
    {
        // 게임이 시작되면 게임 오브젝트에 부착된 NavMeshAgent 컴포넌트를 가져와서 저장
        _agent = this.GetComponent<NavMeshAgent>();

    }

    void Start()
    {
        canvas = GameObject.Find("Canvas");
        monsterStatusController = canvas.transform.Find("MonsterStatus").GetComponent<MonsterStatusController>();

        monsterName = "Skeleton Grunt(해골 거인)";
        isAttacking = false;

        _animator = this.GetComponent<Animator>();
        healthSystem = this.GetComponent<HealthSystem>();
        player = GameObject.Find("Robot Kyle").transform;

        healthSystem.hitPoint = 300f;
        healthSystem.maxHitPoint = 300f;
        healthSystem.regenerate = true;
        healthSystem.regen = 2f;
        healthSystem.isDecrease = false;
        healthSystem.regenUpdateInterval = 1f;
        healthSystem.GodMode = false;

        attackValue = 20f;
        attackRange = 3f;
        speedValue = 5.5f;

        _agent.speed = this.speedValue;

        StartCoroutine(DetectionRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        if (healthSystem.hitPoint <= 0)
        {
            isDie = true;
            _animator.SetTrigger("DieTrigger");
        }

        DetectionInRange(attackRange, (detectObject) =>
        {
            if (detectObject.CompareTag("Player") && !isAttacking)
            {
                target = detectObject.gameObject;
                Debug.Log("멈춤 !");
                Attack();
            }
        });
    }

    IEnumerator DetectionRoutine()
    {
        while (true)
        {
            if (!isAttacking)
            {
                OnIdleStatus();
                yield return new WaitForSeconds(0.5f);
                OnRunStatus();
                yield return new WaitForSeconds(3f);
            }
            yield return null;
        }
    }

    protected override void OnRunStatus()
    {
        _agent.enabled = true;
        DetectionLocationTarget(player);
        _animator.SetTrigger("RunTrigger");
    }

    protected override void OnIdleStatus()
    {
        _animator.SetTrigger("IdleTrigger");
        _agent.enabled = false;
        _agent.velocity = Vector3.zero;
    }

    protected override void OnHitStatus()
    {
        _animator.SetTrigger("HitTrigger");
        _agent.enabled = false;
        _agent.velocity = Vector3.zero;
    }


    protected override void Die()
    {
        monsterStatusController.UnActiveMonsterStatus();
        base.Die();
    }

    protected override void Attack()
    {
        // ShotTrigger 이벤트에서 isAttacking 변수 토글해주면 살짝 늦게 실행됨.
        isAttacking = true;
        // 공격 실행 후 캐릭터 위치를 보게함.
        // transform.LookAt(ArrowTargetVertor());

        _agent.enabled = false;
        _agent.velocity = Vector3.zero;
        _animator.SetTrigger("AttackTrigger");
    }
}
