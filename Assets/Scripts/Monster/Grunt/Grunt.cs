using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Grunt : Chaser
{
    private GameObject target;

    private GameObject canvas;
    private MonsterStatusController monsterStatusController;

    private void Awake()
    {
        // ������ ���۵Ǹ� ���� ������Ʈ�� ������ NavMeshAgent ������Ʈ�� �����ͼ� ����
        Agent = this.GetComponent<NavMeshAgent>();

    }

    void Start()
    {
        canvas = GameObject.Find("Canvas");
        monsterStatusController = canvas.transform.Find("MonsterStatus").GetComponent<MonsterStatusController>();

        MonsterName = "Skeleton Grunt(�ذ� ����)";
        IsAttacking = false;

        Animator = this.GetComponent<Animator>();
        Health = this.GetComponent<HealthSystem>();
        Player = GameObject.Find("Robot Kyle").transform;

        Health.hitPoint = 300f;
        Health.maxHitPoint = 300f;
        Health.regenerate = false;
        Health.isDecrease = false;
        Health.GodMode = false;

        AttackValue = 20f;
        AttackRange = 3f;
        SpeedValue = 5f;

        DetectionTime = 1f;
        DetectionIntervalTime = 3f;

        Agent.speed = SpeedValue;

        StartCoroutine(DetectionRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        if (Health.hitPoint <= 0 && !IsDie)
        {
            if (Animator.GetCurrentAnimatorStateInfo(0).IsName("Skeleton_Grunt_Hit_Back") &&
                Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            {
                IsDie = true;
                Animator.SetTrigger("DieTrigger");
            }
        }

        if (!IsDie)
        {
            DetectionInRange(AttackRange, (detectObject) =>
            {
                if (detectObject.CompareTag("Player") && !IsAttacking)
                {
                    // ShotTrigger �̺�Ʈ���� IsAttacking ���� ������ָ� ��¦ �ʰ� �����.
                    IsAttacking = true;
                    target = detectObject.gameObject;
                    Debug.Log("���� !");
                    Attack();
                }
            });
        }
    }

    private void LateUpdate()
    {

    }

    IEnumerator DetectionRoutine()
    {
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerArrow"))
        {
            // var a = Vector3.Scale(GetHitDiretion(other.transform), new Vector3(1, 0, 1));

            // TODO ���Ͱ� ȭ�쿡 �¾����� ���� ����
            if (!IsDie)
                OnHitStatus();

        }
    }



    // �Ʒ����� ������ �Լ�

    protected override void OnRunStatus()
    {
        Agent.enabled = true;
        DetectionLocationTarget(Player);
        Animator.SetTrigger("RunTrigger");
    }

    protected override void OnIdleStatus()
    {
        Animator.SetTrigger("IdleTrigger");
        Agent.enabled = false;
        Agent.velocity = Vector3.zero;
    }

    protected override void OnHitStatus()
    {
        Animator.SetTrigger("HitTrigger");
        Agent.enabled = false;
        Agent.velocity = Vector3.zero;
    }


    protected override void Die()
    {
        monsterStatusController.UnActiveMonsterStatus();
        base.Die();
    }

    protected override void Attack()
    {
        // ���� ���� �� ĳ���� ��ġ�� ������.
        // transform.LookAt(ArrowTargetVertor());

        Agent.enabled = false;
        Agent.velocity = Vector3.zero;
        Animator.SetTrigger("AttackTrigger");
    }
}
