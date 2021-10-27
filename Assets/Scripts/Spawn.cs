using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    private GameObject monsterGenerator;
    private MonsterGenerator generator;
    private Transform seekerPoint;
    private Transform chaserPoint;
    private List<Transform> chaserPointList;

    // Start is called before the first frame update
    void Start()
    {
        // 해당 오브젝트 자식으로 몬스터들 생성
        monsterGenerator = GameObject.Find("MonsterGenerator");
        generator = GameObject.Find("MonsterGenerator").GetComponent<MonsterGenerator>();
        seekerPoint = transform.GetChild(0);
        chaserPoint = transform.GetChild(1);

        StartCoroutine(SpawnRoutine());
        StartCoroutine(SeekerSpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        yield return new WaitForSeconds(10f);
        RandomSpawn();

        while (true)
        {
            yield return new WaitForSeconds(60f);
            RandomSpawn();
        }
    }

    IEnumerator SeekerSpawnRoutine()
    {
        // 영상찍기용 시작하자마자 나오는 수색꾼
        // Monster seeker = generator.Spawn(MONSTER_TYPE.Seeker, monsterGenerator.transform);
        // seeker.transform.position = seekerPoint.position;
        while (true)
        {
            yield return new WaitForSeconds(180f);
            var cloneSeeker = generator.Spawn(MONSTER_TYPE.Seeker, monsterGenerator.transform);
            cloneSeeker.transform.position = seekerPoint.position;
        }
    }


    private void RandomSpawn()
    {
        // 체이서(추격꾼) 생성 위치 리스트
        chaserPointList = GetChaserSpawnList();

        foreach (Transform transform in chaserPointList)
        {
            Vector3 position = transform.position;
            int monsterTypeIndex = Random.Range(0, 4);
            Monster monster = generator.Spawn((MONSTER_TYPE)monsterTypeIndex, monsterGenerator.transform);
            monster.transform.position = position;

            monster.GetComponent<Animator>().SetTrigger("SpawnTrigger");
        }
    }


    private List<Transform> GetChaserSpawnList()
    {
        List<Transform> resultList = new List<Transform>();

        int[] randomSpwanIndexArr = GetRandomInt(7, 0, 30);
        for (int i = 0; i < randomSpwanIndexArr.Length; i++)
        {
            resultList.Add(chaserPoint.GetChild(randomSpwanIndexArr[i]));
        }

        return resultList;
    }

    private int[] GetRandomInt(int length, int min, int max)
    {
        int[] randArray = new int[length];
        bool isSame;

        for (int i = 0; i < length; i++)
        {
            while (true)
            {
                randArray[i] = Random.Range(min, max);
                isSame = false;
                for (int j = 0; j < i; j++)
                {
                    if (randArray[j] == randArray[i])
                    {
                        isSame = true;
                        break;
                    }
                }
                if (!isSame) break;
            }
        }
        return randArray;
    }


}
