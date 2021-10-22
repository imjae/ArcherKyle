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
        // 게임이 시작되면 게임 오브젝트에 부착된 NavMeshAgent 컴포넌트를 가져와서 저장
        Agent = this.GetComponent<NavMeshAgent>();

    }

    void Start()
    {
        canvas = GameObject.Find("Canvas");
        monsterStatusController = canvas.transform.Find("MonsterStatus").GetComponent<MonsterStatusController>();

        MonsterName = "Skeleton Grunt(해골 거인)";
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
        DetectionIntervalTime = 5f;

        Agent.speed = SpeedValue;

        StartCoroutine(DetectionRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        if (Health.hitPoint <= 0 && !IsDie)
        {
            AnimationCompleteToAction("Skeleton_Grunt_Hit_Back", () =>
            {
                IsDie = true;
                Agent.enabled = false;
                Animator.SetTrigger("DieTrigger");
            });
        }

        if (!IsDie)
        {
            DetectionInRange(AttackRange, (detectObject) =>
            {
                if (detectObject.CompareTag("Player") && !IsAttacking)
                {
                    // ShotTrigger 이벤트에서 IsAttacking 변수 토글해주면 살짝 늦게 실행됨.
                    IsAttackingTrue();
                    target = detectObject.gameObject;
                    // Debug.Log("멈춤 !");

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
        // 공격 실행 후 캐릭터 위치를 보게함.
        // transform.LookAt(ArrowTargetVertor());

        Agent.enabled = false;
        Agent.velocity = Vector3.zero;
        Animator.SetTrigger("AttackTrigger");
    }
}
