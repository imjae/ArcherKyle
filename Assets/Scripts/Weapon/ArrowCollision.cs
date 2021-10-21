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

    // Arrow Ŭ���� ��ӹ޴� �� ȭ���� ���ݷ��� �������� ���� ����
    private Arrow arrowScript;
    private float attackPoint;

    private GameObject canvas;
    private MonsterStatusController monsterStatusController;

    private void Start()
    {
        canvas = GameObject.Find("Canvas");

        rigid = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        arrowMovement = GetComponent<ArrowMovement>();


        arrowScript = GetComponent<Arrow>();
        monsterStatusController = canvas.transform.Find("MonsterStatus").GetComponent<MonsterStatusController>();
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
                Monster monster = other.GetComponent<Archer>();
                monsterStatusController.SetMonsterName(monster.monsterName);
            }

            if (other.CompareTag("MonsterCriticalZone"))
            {
                Debug.Log("ũ��Ƽ��!!");
                var monster = other.transform.root;
                var health = monster.gameObject.GetComponent<HealthSystem>();
                health.TakeDamage(health.maxHitPoint);
            }
            else
            {
                var monster = other.transform.root;
                var health = monster.gameObject.GetComponent<HealthSystem>();
                health.TakeDamage(arrowScript.attackPoint);
            }
        }
    }
}
