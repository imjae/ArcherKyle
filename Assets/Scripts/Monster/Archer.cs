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
        _agent = this.GetComponent<NavMeshAgent>();

    }

    private void Start()
    {
        canvas = GameObject.Find("Canvas");
        monsterStatusController = canvas.transform.Find("MonsterStatus").GetComponent<MonsterStatusController>();

        monsterName = "Skeleton Archer";
        isAttacking = false;

        _animator = this.GetComponent<Animator>();
        healthSystem = this.GetComponent<HealthSystem>();
        player = GameObject.Find("Robot Kyle").transform;

        healthSystem.hitPoint = 200f;
        healthSystem.maxHitPoint = 200f;
        healthSystem.regenerate = true;
        healthSystem.regen = 0.5f;
        healthSystem.isDecrease = false;
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
        if (healthSystem.hitPoint <= 0)
        {
            _animator.SetTrigger("DieTrigger");
        }
        Debug.DrawLine(transform.position, transform.forward * 5f, Color.red);
        // Debug.DrawLine(transform.position, Vector3.forward * 5f, Color.red);
        // Debug.DrawLine(transform.position, Vector3.back * 5f, Color.cyan);
        // Debug.DrawLine(transform.position, Vector3.left * 5f, Color.blue);
        // Debug.DrawLine(transform.position, Vector3.right * 5f, Color.green);


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


            // var b = transform.forward;

            // float cos = Vector3.Dot(a, b) / (a.magnitude * b.magnitude);
            // float cos_to_angles = Mathf.Acos(cos) * Mathf.Rad2Deg;

            // var angle = Vector3.Angle(a, transform.forward);
            // Debug.Log(angle + " / " + cos_to_angles);
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
        _animator.Play("Skeleton_Crossbowman_Idle_Loop");
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
        this.isAttacking = true;
        // ���� ���� �� ĳ���� ��ġ�� ������.
        this.transform.LookAt(ArrowTargetVertor());

        _agent.enabled = false;
        _agent.velocity = Vector3.zero;
        _animator.SetTrigger("ShotTrigger");
    }
}
