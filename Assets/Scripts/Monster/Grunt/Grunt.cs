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
        // ������ ���۵Ǹ� ���� ������Ʈ�� ������ NavMeshAgent ������Ʈ�� �����ͼ� ����
        _agent = this.GetComponent<NavMeshAgent>();

    }

    void Start()
    {
        canvas = GameObject.Find("Canvas");
        monsterStatusController = canvas.transform.Find("MonsterStatus").GetComponent<MonsterStatusController>();

        monsterName = "Skeleton Grunt(�ذ� ����)";
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
                Debug.Log("���� !");
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
        // ShotTrigger �̺�Ʈ���� isAttacking ���� ������ָ� ��¦ �ʰ� �����.
        isAttacking = true;
        // ���� ���� �� ĳ���� ��ġ�� ������.
        // transform.LookAt(ArrowTargetVertor());

        _agent.enabled = false;
        _agent.velocity = Vector3.zero;
        _animator.SetTrigger("AttackTrigger");
    }
}
