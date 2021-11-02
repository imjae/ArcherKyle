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
        // 몬스터 생성시 해당 몬스터의 Face 카메라 등록
        FaceCamera = transform.Find("FaceCamera").GetComponent<Camera>();
        CameraManagement.Camera.EnrollFaceCamera(FaceCamera);

        MonsterName = "Skeleton Grunt(해골 거인)";
        MonsterWeak = "CHEST";
        IsAttacking = false;

        Animator = this.GetComponent<Animator>();
        Health = this.GetComponent<HealthSystem>();
        Player = GameManager.Instance.robotKyle.transform;

        Health.hitPoint = 300f;
        Health.maxHitPoint = 300f;
        Health.regenerate = false;
        Health.isDecrease = false;
        Health.GodMode = false;

        AttackValue = 20f;
        AttackRange = 3f;
        SpeedValue = 5f;

        DetectionTime = 1f;
        DetectionIntervalTime = 5f;

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
                if (detectObject.CompareTag("Player") && !IsAttacking)
                {
                    // Debug.Log("감지 성공!!!");
                    // ShotTrigger 이벤트에서 IsAttacking 변수 토글해주면 살짝 늦게 실행됨.
                    IsAttackingTrue();
                    target = detectObject.gameObject;
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
                              Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            action();
        }
    }

    // 아래부터 재정의 함수

    protected override void OnRunStatus()
    {
        base.OnRunStatus();
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
        transform.LookAt(target.transform.position);

        base.Attack();
    }
}
