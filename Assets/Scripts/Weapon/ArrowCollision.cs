using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowCollision : MonoBehaviour
{

    Rigidbody rigid;
    Collider col;

    ArrowMovement arrowMovement;

    [SerializeField]
    private GameObject arrowExplosion;

    // Arrow 클래스 상속받는 각 화살의 공격력을 가져오기 위한 변수
    private Arrow arrowScript;
    private float attackPoint;

    private GameObject canvas;
    private MonsterStatusController monsterStatusController;

    private void Awake()
    {
        canvas = GameObject.Find("Canvas");
        monsterStatusController = canvas.transform.Find("MonsterStatus").GetComponent<MonsterStatusController>();
    }

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        arrowMovement = GetComponent<ArrowMovement>();
        arrowScript = GetComponent<Arrow>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            arrowMovement.isMovement = false;
            rigid.velocity = Vector3.zero;

            Destroy(rigid);
            Destroy(col);

            transform.SetParent(other.transform);

            Instantiate(arrowExplosion, transform.position, Quaternion.identity);

            if (other.CompareTag("Monster"))
            {
                monsterStatusController.ActiveMonsterStatus();

                // 몬스터 카메라 전부 OFF
                CameraManagement.Instance.AllFaceCameraOff();
                other.transform.Find("FaceCamera").GetComponent<Camera>().enabled = true;

                Monster monster = other.GetComponent<Monster>();
                monsterStatusController.SetMonsterName(monster.MonsterName);
            }

            if (other.CompareTag("MonsterCriticalZone"))
            {
                var monster = other.transform.root;
                var health = monster.gameObject.GetComponent<HealthSystem>();
                Debug.Log("크리티컬!! : " + health.maxHitPoint);
                health.TakeDamage(health.maxHitPoint);
            }
            else if (other.CompareTag("Monster"))
            {
                Debug.Log("몸통! : " + arrowScript.attackPoint);
                var monster = other.transform.root;
                var health = monster.gameObject.GetComponent<HealthSystem>();
                health.TakeDamage(arrowScript.attackPoint);
            }
        }
    }
}
