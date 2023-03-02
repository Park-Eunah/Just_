using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool_PEA : MonoBehaviour
{
    private float spawnRateMin = 0.01f;
    private float spawnRateMax = 0.5f;
    private float spawnRate = 0f; //���� �ֱ�
    private float timeAfterSpawn = 0f;
    private float spawnPosXMin = -10f;
    private float spawnPosXMax = 10f;
    private float spawnPosX = 0f; //������ ��ġ�� X��
    private float spawnPosY = 1.5f; //������ ��ġ�� Y��
    private float spawnPosZ = -17f; //������ ��ġ�� Z��

    private int allocateCount = 25;  //�̸� ����� �� ������Ʈ ��
    private int objNum = 0; //������ ������Ʈ ��ȣ(�迭 �ε���)
    private int index = 0; //Ǯ���� ������ ������Ʈ �ε���(����Ʈ �ε���)

    private bool isChanging = false; //���ư� ������Ʈ ���������� Ȯ��(�ڿ������� ���̵� ����� ���� �ÿ�)

    private List<GameObject> poolList = new List<GameObject>();

    public GameObject[] poolObj = null;

    public static ObjectPool_PEA instance = null;

    private void Awake()
    {
        if (instance)  //�̱���
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
        InvokeRepeating("TimeCheck", 5f, 1f); //TimeCheck�޼��� 5���ĺ��� 2�ʸ��� �ݺ� ����
    }

    void Update()
    {
        SpawnTrash();
    }

    void RandomSpawnRate()  //���� ������Ʈ ���� �ð� ���ϴ� �޼���
    {
        spawnRate = Random.Range(spawnRateMin, spawnRateMax);
    }

    void RandomSpawnPosX()  //���� ������Ʈ ������ ��ġ ���ϴ� �޼���
    {
        spawnPosX = Random.Range(spawnPosXMin, spawnPosXMax);
    }

    void SpawnTrash()  //�������� ������ �ð�, ��ġ�� ���� ������Ʈ Ǯ���� �����ֱ�
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

    void Allocate()  //������Ʈ�� �̸� ����, ��Ȱ��ȭ, ����Ʈ�� �־��ֱ�
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

    public GameObject PopQueue() //����� ������Ʈ ����Ʈ���� �����ֱ�
    {
        ChooseObject();
        GameObject obj = poolList[index];
        poolList.RemoveAt(index);
        obj.gameObject.SetActive(true);
        obj.GetComponent<Trash>().SetSpeedRange(objNum);
        return obj;
    }

    public void PushQueue(GameObject obj) //����� ������Ʈ�� �ٽ� ����Ʈ�� �־��ֱ�
    {
        obj.gameObject.SetActive(false);
        poolList.Insert(Random.Range(0,poolList.Count),obj); //����Ʈ �ȿ� �������� ��ȯ
    }

    private void ChooseObject() //������ ������Ʈ ����
    {
        index = 0;
        foreach (GameObject obj in poolList) //������Ʈ �̸����� ã�´�.
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
        if (time <= 8f) //ó�� ������ �� ī��Ʈ ���� �ð��� ���� ���
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