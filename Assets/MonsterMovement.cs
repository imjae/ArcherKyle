using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterMovement : MonoBehaviour
{
    // 길을 찾아서 이동할 에이전트
    [SerializeField]
    NavMeshAgent agent;

    // 에이전트의 목적지
    [SerializeField]
    Transform target;

    private void Awake()
    {
        // 게임이 시작되면 게임 오브젝트에 부착된 NavMeshAgent 컴포넌트를 가져와서 저장
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        // 스페이스 키를 누르면 Target의 위치까지 이동하는 경로를 계산해서 이동
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 에이전트에게 목적지를 알려주는 함수
            agent.SetDestination(target.position);
        }
    }
}
