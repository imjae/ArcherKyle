using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Archer : Chaser
{
    [SerializeField]
    private GameObject arrow;
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

    private void Start()
    {
        // 몬스터 생성시 해당 몬스터의 Face 카메라 등록
        FaceCamera = transform.Find("FaceCamera").GetComponent<Camera>();
        CameraManagement.Camera.EnrollFaceCamera(FaceCamera);

        MonsterName = "Skeleton Archer(해골 궁수)";
        MonsterWeak = "HAT";
        IsAttacking = false;

        Animator = this.GetComponent<Animator>();
        Health = this.GetComponent<HealthSystem>();
        Player = GameManager.Instance.robotKyle.transform;

        Health.hitPoint = 150f;
        Health.maxHitPoint = 150f;
        Health.regenerate = false;
        Health.isDecrease = false;
        Health.GodMode = false;

        AttackValue = 10f;
        AttackRange = 7f;
        SpeedValue = 7f;

        DetectionTime = 0.5f;
        DetectionIntervalTime = 4f;

        Agent.speed = SpeedValue;

        Detection = DetectionRoutine();
        StartCoroutine(Detection);
    }

    void Update()
    {
        if (Health.hitPoint <= 0 && !IsDie)
        {
            Die();
        }
    }

    // TODO 범위 안에 지정 태그가 들어왔을떄 동작 정의할 수 있다.
    private void LateUpdate()
    {
        if (!IsDie)
        {
            DetectionInRange(AttackRange, (detectObject) =>
             {
                 if (detectObject.CompareTag("Player") && !IsAttacking)
                 {
                     IsAttackingTrue();
                     target = detectObject.gameObject;

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

    private void CloneArrow()
    {
        // 생성위치
        // arrow 오브젝트 자식으로
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
        transform.LookAt(ArrowTargetVertor());

        base.Attack();
    }
}