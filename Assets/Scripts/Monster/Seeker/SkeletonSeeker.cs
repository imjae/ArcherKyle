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
        // 게임이 시작되면 게임 오브젝트에 부착된 NavMeshAgent 컴포넌트를 가져와서 저장
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

        // 몬스터 생성시 해당 몬스터의 Face 카메라 등록
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
                    // Debug.Log("감지 성공!!!");
                    // ShotTrigger 이벤트에서 IsAttacking 변수 토글해주면 살짝 늦게 실행됨.
                    IsAttackingTrue();
                    // target = detectObject.gameObject;
                    // Debug.Log("멈춤 !");

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

            // TODO 몬스터가 화살에 맞았을때 동작 정의
            if (!IsDie)
                OnHitStatus();
        }
    }

    // 해당 애니메이션이 끝나고나서 동작할 행위 정의
    private void AnimationCompleteToAction(string animationName, Action action)
    {
        if (Animator.GetCurrentAnimatorStateInfo(0).IsName(animationName) &&
                              Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.4)
        {
            action();
        }
    }

    // 몬스터의 플레이어 감지 동작 루틴
    protected override IEnumerator DetectionRoutine()
    {
        // 죽지 않았을 때만 플레이어 감지
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

    // 아래부터 재정의 함수

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
        // 공격 실행 후 캐릭터 위치를 보게함.
        transform.rotation = Quaternion.LookRotation(currentTarget.transform.position);

        base.Attack();
    }
}
