using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Archer : Chaser
{
    private HealthSystem healthSystem;
    [SerializeField]
    private GameObject arrow;

    private GameObject target;

    private GameObject canvas;
    private MonsterStatusController monsterStatusController;

    private void Awake()
    {
        // ������ ���۵Ǹ� ���� ������Ʈ�� ������ NavMeshAgent ������Ʈ�� �����ͼ� ����
        Agent = this.GetComponent<NavMeshAgent>();

    }

    private void Start()
    {
        canvas = GameObject.Find("Canvas");
        monsterStatusController = canvas.transform.Find("MonsterStatus").GetComponent<MonsterStatusController>();

        MonsterName = "Skeleton Archer(�ذ� �ü�)";
        IsAttacking = false;

        Animator = this.GetComponent<Animator>();
        healthSystem = this.GetComponent<HealthSystem>();
        Player = GameObject.Find("Robot Kyle").transform;

        healthSystem.hitPoint = 150f;
        healthSystem.maxHitPoint = 150f;
        healthSystem.regenerate = true;
        healthSystem.regen = 0.5f;
        healthSystem.isDecrease = false;
        healthSystem.regenUpdateInterval = 1f;
        healthSystem.GodMode = false;

        AttackValue = 10f;
        AttackRange = 7f;
        SpeedValue = 8f;

        DetectionTime = 0.5f;
        DetectionIntervalTime = 4f;

        Agent.speed = SpeedValue;

        StartCoroutine(DetectionRoutine());
    }

    void Update()
    {
        if (healthSystem.hitPoint <= 0 && !IsDie)
        {
            IsDie = true;
            Animator.SetTrigger("DieTrigger");
        }
    }

    private void LateUpdate()
    {
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

    IEnumerator DetectionRoutine()
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

    private void CloneArrow()
    {
        // ������ġ
        // arrow ������Ʈ �ڽ�����
        // position : 0, 0.7, 0
        var clone = Instantiate(arrow);
        clone.transform.SetParent(transform.Find("arrow"));
        clone.transform.localPosition = new Vector3(0f, 0.7f, 0f);

        clone.transform.LookAt(ArrowTargetVertor());
    }

    private Vector3 ArrowTargetVertor()
    {
        Vector3 position = target.transform.position;
        return new Vector3(position.x, position.y + 1f, position.z);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerArrow"))
        {
            var a = Vector3.Scale(GetHitDiretion(other.transform), new Vector3(1, 0, 1));

            // TODO ���Ͱ� ȭ�쿡 �¾����� ���� ����
            // if (!IsDie)
            //     OnHitStatus();


            // var b = transform.forward;

            // float cos = Vector3.Dot(a, b) / (a.magnitude * b.magnitude);
            // float cos_to_angles = Mathf.Acos(cos) * Mathf.Rad2Deg;

            // var angle = Vector3.Angle(transform.forward, a);

            // if (angle < 90)
            // {

            //     Debug.Log(angle);
            //     Debug.Log("��");
            // }
            // else if (angle < 180)
            // {

            //     Debug.Log(angle);
            //     Debug.Log("��");
            // }
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
        Animator.Play("Skeleton_Crossbowman_Idle_Loop");
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
        transform.LookAt(ArrowTargetVertor());

        Agent.enabled = false;
        Agent.velocity = Vector3.zero;
        Animator.SetTrigger("ShotTrigger");
    }
}
