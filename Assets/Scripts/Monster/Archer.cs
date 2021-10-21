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

    // ������ �÷���
    private bool isAttacking;
    private GameObject target;

    private void Awake()
    {
        // ������ ���۵Ǹ� ���� ������Ʈ�� ������ NavMeshAgent ������Ʈ�� �����ͼ� ����
        _agent = this.GetComponent<NavMeshAgent>();

    }

    private void Start()
    {
        isAttacking = false;

        _animator = this.GetComponent<Animator>();
        healthSystem = this.GetComponent<HealthSystem>();
        player = GameObject.Find("Robot Kyle").transform;

        healthSystem.hitPoint = 100f;
        healthSystem.maxHitPoint = 100f;
        healthSystem.regenerate = true;
        healthSystem.regen = 0.1f;
        healthSystem.regenUpdateInterval = 1f;
        healthSystem.GodMode = false;

        attackValue = 10f;
        attackRange = 7f;
        speedValue = 8f;

        _agent.speed = this.speedValue;


        StartCoroutine(DetectionRoutine());
    }

    void Update()
    {
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
                yield return new WaitForSeconds(1f);
                OnRunStatus();
                yield return new WaitForSeconds(6f);
            }
            yield return null;
        }
    }

    private void OnRunStatus()
    {
        _agent.enabled = true;
        DetectionLocationTarget(player);
        _animator.SetTrigger("RunTrigger");
    }

    private void OnIdleStatus()
    {
        _animator.Play("Skeleton_Crossbowman_Idle_Loop");
        _agent.enabled = false;
        _agent.velocity = Vector3.zero;
    }

    protected override void Attack()
    {
        // ShotTrigger �̺�Ʈ���� isAttacking ���� ������ָ� ��¦ �ʰ� �����.
        this.isAttacking = true;
        // ���� ���� �� ĳ���� ��ġ�� ������.
        this.transform.LookAt(ArrowTargetVertor());

        _agent.enabled = false;
        _agent.velocity = Vector3.zero;
        _animator.SetTrigger("ShotTrigger");
    }

    public void ToggleIsAttacking()
    {
        Debug.Log("�� : " + this.isAttacking);
        this.isAttacking = !this.isAttacking;
        Debug.Log("�� : " + this.isAttacking);
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
}
