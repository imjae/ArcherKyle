using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mage : Chaser
{
    [SerializeField]
    private GameObject magic;
    private GameObject target;
    private GameObject canvas;
    private MonsterStatusController monsterStatusController;

    private IEnumerator detection;

    private void Awake()
    {
        // ������ ���۵Ǹ� ���� ������Ʈ�� ������ NavMeshAgent ������Ʈ�� �����ͼ� ����
        Agent = this.GetComponent<NavMeshAgent>();
        canvas = GameObject.Find("Canvas");
        monsterStatusController = canvas.transform.Find("MonsterStatus").GetComponent<MonsterStatusController>();

    }

    private void Start()
    {
        // ���� ������ �ش� ������ Face ī�޶� ���
        FaceCamera = transform.Find("FaceCamera").GetComponent<Camera>();
        CameraManagement.Camera.EnrollFaceCamera(FaceCamera);

        MonsterName = "Skeleton Mage(�ذ� ������)";
        IsAttacking = false;

        Animator = this.GetComponent<Animator>();
        Health = this.GetComponent<HealthSystem>();
        Player = GameObject.Find("Robot Kyle").transform;

        Health.hitPoint = 100f;
        Health.maxHitPoint = 100f;
        Health.regenerate = false;
        Health.isDecrease = false;
        Health.GodMode = false;

        AttackValue = 20f;
        AttackRange = 9f;
        SpeedValue = 5f;

        DetectionTime = 0f;
        DetectionIntervalTime = 2f;

        Agent.speed = SpeedValue;

        detection = DetectionRoutine();
        StartCoroutine(detection);
    }

    void Update()
    {
        if (Health.hitPoint <= 0 && !IsDie)
        {
            Die();
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
                    IsAttackingTrue();
                    target = detectObject.gameObject;
                    // Debug.Log("���� !");

                    Attack();
                }
            });
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

    private void OnDestroy()
    {
        CameraManagement.Camera.RemoveCamera(FaceCamera);
    }

    // �ش� �ִϸ��̼��� �������� ������ ���� ����
    private void AnimationCompleteToAction(string animationName, Action action)
    {
        if (Animator.GetCurrentAnimatorStateInfo(0).IsName(animationName) &&
                              Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.4)
        {
            action();
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
        var clone = Instantiate(magic);
        clone.transform.SetParent(transform.Find("arrow"));
        clone.transform.localPosition = new Vector3(0f, 0.7f, 0f);

        clone.transform.LookAt(ArrowTargetVertor());
    }

    private Vector3 ArrowTargetVertor()
    {
        Vector3 position = target.transform.position;
        return new Vector3(position.x, position.y + 1f, position.z);
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
        Animator.Play("Skeleton_Mage_Idle_Loop");
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
        base.Die();

        StopCoroutine(detection);
        monsterStatusController.UnActiveMonsterStatus();
    }

    protected override void Attack()
    {
        // ���� ���� �� ĳ���� ��ġ�� ������.
        transform.LookAt(ArrowTargetVertor());

        Agent.enabled = false;
        Agent.velocity = Vector3.zero;
        Animator.SetTrigger("AttackTrigger");
    }
}
