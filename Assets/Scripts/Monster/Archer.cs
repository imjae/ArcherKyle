using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Archer : Chaser
{
    // protected float healthPoint { get; set; }
    // protected float maxHealthPoint { get; set; }
    // protected float energyPoint { get; set; }
    // protected float maxEnergyPoint { get; set; }
    // protected float attackValue { get; set; }
    // protected float speedValue { get; set; }
    // protected Vector3 currentPosition { get; set; }

    // ���� ã�Ƽ� �̵��� ������Ʈ
    [SerializeField]
    NavMeshAgent _agent;

    // ������Ʈ�� ������
    [SerializeField]
    Transform player;

    Animator _animator;

    private void Awake()
    {
        // ������ ���۵Ǹ� ���� ������Ʈ�� ������ NavMeshAgent ������Ʈ�� �����ͼ� ����
        _agent = this.GetComponent<NavMeshAgent>();
        _animator = this.GetComponent<Animator>();

        StartCoroutine(DetectionRoutine());
    }

    private void Start()
    {

        healthSystem = gameObject.AddComponent<HealthSystem>();
        healthSystem.hitPoint = 100f;
        healthSystem.maxHitPoint = 100f;
        healthSystem.regenerate = true;
        healthSystem.regen = 0.1f;
        healthSystem.regenUpdateInterval = 1f;
        healthSystem.GodMode = false;

        attackValue = 10f;
        attackRange = 10f;
        speedValue = 10f;

        _agent = this.GetComponent<NavMeshAgent>();
        _agent.speed = this.speedValue;
    }












    IEnumerator DetectionRoutine()
    {
        while (true)
        {
            _animator.Play("Skeleton_Crossbowman_Idle_Loop");
            yield return new WaitForSeconds(2f);
            _animator.Play("Skeleton_Crossbowman_Run_Loop");
            TargetDetection(player);
            yield return new WaitForSeconds(5f);
        }
    }

    protected override void TargetDetection(Transform target)
    {
        Debug.Log(target.gameObject.name);
        _agent.SetDestination(target.position);
    }

}
