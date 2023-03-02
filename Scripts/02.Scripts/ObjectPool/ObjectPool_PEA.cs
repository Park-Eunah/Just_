using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool_PEA : MonoBehaviour
{
    private float spawnRateMin = 0.01f;
    private float spawnRateMax = 0.5f;
    private float spawnRate = 0f; //스폰 주기
    private float timeAfterSpawn = 0f;
    private float spawnPosXMin = -10f;
    private float spawnPosXMax = 10f;
    private float spawnPosX = 0f; //스폰할 위치값 X축
    private float spawnPosY = 1.5f; //스폰할 위치값 Y축
    private float spawnPosZ = -17f; //스폰할 위치값 Z축

    private int allocateCount = 25;  //미리 만들어 둘 오브젝트 수
    private int objNum = 0; //스폰할 오브젝트 번호(배열 인덱스)
    private int index = 0; //풀에서 꺼내줄 오브젝트 인덱스(리스트 인덱스)

    private bool isChanging = false; //날아갈 오브젝트 변경중인지 확인(자연스러운 난이도 상승을 위해 시용)

    private List<GameObject> poolList = new List<GameObject>();

    public GameObject[] poolObj = null;

    public static ObjectPool_PEA instance = null;

    private void Awake()
    {
        if (instance)  //싱글턴
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    void Start()
    {
        Allocate();
        RandomSpawnRate();
        RandomSpawnPosX();
        InvokeRepeating("TimeCheck", 5f, 1f); //TimeCheck메서드 5초후부터 2초마다 반복 실행
    }

    void Update()
    {
        SpawnTrash();
    }

    void RandomSpawnRate()  //다음 오브젝트 스폰 시간 정하는 메서드
    {
        spawnRate = Random.Range(spawnRateMin, spawnRateMax);
    }

    void RandomSpawnPosX()  //다음 오브젝트 스폰할 위치 정하는 메서드
    {
        spawnPosX = Random.Range(spawnPosXMin, spawnPosXMax);
    }

    void SpawnTrash()  //랜덤으로 정해진 시간, 위치에 맞춰 오브젝트 풀에서 꺼내주기
    {
        timeAfterSpawn += Time.deltaTime;

        if (timeAfterSpawn >= spawnRate)
        {
            timeAfterSpawn = 0f;
            GameObject obj = PopQueue();
            obj.transform.position = new Vector3(spawnPosX, spawnPosY, spawnPosZ);
            RandomSpawnRate();
            RandomSpawnPosX();
        }
    }

    void Allocate()  //오브젝트들 미리 생성, 비활성화, 리스트에 넣어주기
    {
        foreach(GameObject o  in poolObj)
        {
            for (int i = 0; i < allocateCount ; i++)
            {
                GameObject obj = Instantiate(o, this.gameObject.transform);
                obj.SetActive(false);
                poolList.Add(obj);
            }
        }
    }

    public GameObject PopQueue() //사용할 오브젝트 리스트에서 꺼내주기
    {
        ChooseObject();
        GameObject obj = poolList[index];
        poolList.RemoveAt(index);
        obj.gameObject.SetActive(true);
        obj.GetComponent<Trash>().SetSpeedRange(objNum);
        return obj;
    }

    public void PushQueue(GameObject obj) //사용한 오브젝트를 다시 리스트에 넣어주기
    {
        obj.gameObject.SetActive(false);
        poolList.Insert(Random.Range(0,poolList.Count),obj); //리스트 안에 랜덤으로 반환
    }

    private void ChooseObject() //스폰할 오브젝트 선택
    {
        index = 0;
        foreach (GameObject obj in poolList) //오브젝트 이름으로 찾는다.
        {
            if (isChanging) 
            {
                if(obj.name.Contains(poolObj[objNum].name) || obj.name.Contains(poolObj[objNum + 1].name))
                {
                    return;
                }
            }
            else
            {
                if (obj.name.Contains(poolObj[objNum].name))
                {
                    return;
                }
            }
            index++;
        }
    }

    private void TimeCheck()
    {
        float time = Time.time - 5f;
        if (time <= 8f) //처음 시작할 때 카운트 세는 시간을 빼고 계산
        {
            objNum = 0;
            isChanging = false;
        }
        else if (8f < time && time <= 10.5f)
        {
            objNum = 0;
            isChanging = true;
        }
        else if (10.5f < time && time <= 18.5f)
        {
            objNum = 1;
            isChanging = false;
        }
        else if (18.5f < time && time <= 21f)
        {
            objNum = 1;
            isChanging = true;
        }
        else if (21f < time && time <= 29f)
        {
            objNum = 2;
            isChanging = false;
        }
        else if (29f < time && time <= 31.5f)
        {
            objNum = 2;
            isChanging = true;
        }
        else if (31.5f < time && time <= 39.5f)
        {
            objNum = 3;
            isChanging = false;
        }
        else if (39.5f < time && time <= 42f)
        {
            objNum = 3;
            isChanging = true;
        }
        else if (42f < time && time <= 50f)
        {
            objNum = 4;
            isChanging = false;
        }
        else if (50f < time && time <= 52.5f)
        {
            objNum = 4;
            isChanging = true;
        }
        else if (52.5f < time && time <= 60f)
        {
            objNum = 5;
            isChanging = false;
        }
    }
}