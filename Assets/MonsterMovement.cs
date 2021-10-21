using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterMovement : MonoBehaviour
{
    // ���� ã�Ƽ� �̵��� ������Ʈ
    [SerializeField]
    NavMeshAgent agent;

    // ������Ʈ�� ������
    [SerializeField]
    Transform target;

    private void Awake()
    {
        // ������ ���۵Ǹ� ���� ������Ʈ�� ������ NavMeshAgent ������Ʈ�� �����ͼ� ����
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        // �����̽� Ű�� ������ Target�� ��ġ���� �̵��ϴ� ��θ� ����ؼ� �̵�
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // ������Ʈ���� �������� �˷��ִ� �Լ�
            agent.SetDestination(target.position);
        }
    }
}
