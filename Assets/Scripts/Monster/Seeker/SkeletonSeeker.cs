using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SkeletonSeeker : Seeker
{
    [SerializeField]
    private GameObject target;
    private GameObject canvas;
    private MonsterStatusController monsterStatusController;

    public Transform currentTarget;
    public Queue<Transform> targetQueue;

    public GameObject crystal;

    private void Awake()
    {
        canvas = GameObject.Find("Canvas");

    }

    private void OnEnable()
    {
        // ������ ���۵Ǹ� ���� ������Ʈ�� ������ NavMeshAgent ������Ʈ�� �����ͼ� ����
        Agent = this.GetComponent<NavMeshAgent>();
        monsterStatusController = canvas.transform.Find("MonsterStatus").GetComponent<MonsterStatusController>();
    }

    void Start()
    {
        crystal = GameObject.Find("Crystal");

        targetQueue = new Queue<Transform>();
        for (int i = 0; i < 5; i++)
        {
            targetQueue.Enqueue(crystal.transform.GetChild(i));
        }

        currentTarget = targetQueue.Dequeue();

        // ���� ������ �ش� ������ Face ī�޶� ���
        FaceCamera = transform.Find("FaceCamera").GetComponent<Camera>();
        CameraManagement.Camera.EnrollFaceCamera(FaceCamera);

        MonsterName = "SEEKER";
        MonsterWeak = "JOINT";
        IsAttacking = false;

        Animator = this.GetComponent<Animator>();
        Health = this.GetComponent<HealthSystem>();
        // Player = GameObject.Find("Robot Kyle").transform;

        Health.hitPoint = 5000f;
        Health.maxHitPoint = 5000f;
        Health.regenerate = false;
        Health.isDecrease = false;
        Health.GodMode = false;

        AttackValue = 30f;
        AttackRange = 6f;
        SpeedValue = 2f;

        DetectionTime = 1f;
        DetectionIntervalTime = 7f;

        Agent.speed = SpeedValue;

        Detection = DetectionRoutine();
        StartCoroutine(Detection);
    }

    // Update is called once per frame
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
                if ((detectObject.CompareTag("Crystal") || detectObject.CompareTag("Player")) && !IsAttacking)
                {
                    // Debug.Log("���� ����!!!");
                    // ShotTrigger �̺�Ʈ���� IsAttacking ���� ������ָ� ��¦ �ʰ� �����.
                    IsAttackingTrue();
                    // target = detectObject.gameObject;
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

    // �ش� �ִϸ��̼��� �������� ������ ���� ����
    private void AnimationCompleteToAction(string animationName, Action action)
    {
        if (Animator.GetCurrentAnimatorStateInfo(0).IsName(animationName) &&
                              Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.4)
        {
            action();
        }
    }

    // ������ �÷��̾� ���� ���� ��ƾ
    protected override IEnumerator DetectionRoutine()
    {
        // ���� �ʾ��� ���� �÷��̾� ����
        while (!IsDie)
        {
            if (!IsAttacking)
            {
                OnIdleStatus();
                yield return new WaitForSeconds(DetectionTime);
                OnRunStatus(currentTarget);
                yield return new WaitForSeconds(DetectionIntervalTime);
            }
            yield return null;
        }
    }

    // �Ʒ����� ������ �Լ�

    protected override void OnRunStatus(Transform target)
    {
        base.OnRunStatus(target);
    }

    protected override void OnIdleStatus()
    {
        base.OnIdleStatus();
    }

    protected override void OnHitStatus()
    {
        base.OnHitStatus();
    }

    protected override void Die()
    {
        base.Die();

        monsterStatusController.UnActiveMonsterStatus();
    }

    protected override void Attack()
    {
        // ���� ���� �� ĳ���� ��ġ�� ������.
        transform.rotation = Quaternion.LookRotation(currentTarget.transform.position);

        base.Attack();
    }
}
