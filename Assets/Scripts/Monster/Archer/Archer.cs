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
        // 게임이 시작되면 게임 오브젝트에 부착된 NavMeshAgent 컴포넌트를 가져와서 저장
        Agent = this.GetComponent<NavMeshAgent>();
        canvas = GameObject.Find("Canvas");
        monsterStatusController = canvas.transform.Find("MonsterStatus").GetComponent<MonsterStatusController>();
    }

    private void Start()
    {

        MonsterName = "Skeleton Archer(해골 궁수)";
        IsAttacking = false;

        Animator = this.GetComponent<Animator>();
        Health = this.GetComponent<HealthSystem>();
        Player = GameObject.Find("Robot Kyle").transform;

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

        StartCoroutine(DetectionRoutine());
    }

    void Update()
    {
        if (Health.hitPoint <= 0 && !IsDie)
        {
            AnimationCompleteToAction("Skeleton_Crossbowman_Hit_Back", () =>
            {
                IsDie = true;
                Animator.SetTrigger("DieTrigger");
            });
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
                    // ShotTrigger 이벤트에서 IsAttacking 변수 토글해주면 살짝 늦게 실행됨.
                    IsAttackingTrue();
                    target = detectObject.gameObject;
                    Debug.Log("멈춤 !");

                    Attack();

                }
            });
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

    IEnumerator DetectionRoutine()
    {
        // 죽지 않았을 때만 플레이어 감지
        while (true)
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

    // 아래부터 재정의 함수

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
        // 공격 실행 후 캐릭터 위치를 보게함.
        transform.LookAt(ArrowTargetVertor());

        Agent.enabled = false;
        Agent.velocity = Vector3.zero;
        Animator.SetTrigger("ShotTrigger");
    }
}
