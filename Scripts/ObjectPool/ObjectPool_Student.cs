using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool_Student : MonoBehaviour
{
    public GameObject []prefab;
    int maxCount = 18;
    Queue<GameObject> queue = new Queue<GameObject>();
    public static ObjectPool_Student objectPool = null;
    private void Awake()
    {
        if(objectPool == null)//�̱���
        {
            objectPool = this.GetComponent<ObjectPool_Student>();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    private void Start()
    {
        CreateStudent();
        InvokeRepeating("GetObject", 2f, Random.Range(3,4));//���ӽ���1���� 3�ʿ��� 5�ʻ��� �ѹ� �ش�޼ҵ� ����(�Ϸ��ߴµ� �μ��� ������
                                                          //�ȵȴٰ��ؼ� �ڷ�ƾ���� ���濹��

    }
    public void CreateStudent() //���ӽ���� �ѹ� ����Ʈ���л���ü maxCount ��ŭ����
    {
        for (int i = 0; i < maxCount; i++) 
        {
            GameObject temp = Instantiate(prefab[Random.Range(0, 2)]);
            temp.name = "student" + i;
            temp.transform.parent = transform; //������ƮǮObject�� �ڽ����� �־���
            temp.SetActive(false);
            queue.Enqueue(temp);//queue�� �־���
        }
    }
    public void GetObject()
    {
        foreach(GameObject item in queue) //foreach�� queue �ȿ� �� �����յ��� �ϳ��ϳ� ������
        {
            if(item.activeSelf == false) //�ϳ��� �������� setActive�� false�� �༮���� ������
            {
                GameObject obj = queue.Dequeue();
                obj.SetActive(true);//setActive�� true�� �������
                obj.transform.position = new Vector3(Random.Range(-3.0f,22f),0,-243.1f);//������ �������� ��ġ(x���� �������� �༭ x��ġ ���� �ٸ��� ����)
                return;//�Ѱ��������;ߵǱ⶧���� return���� ������
            }
        }
    }
    public void returnPool(GameObject obj)
    {
        obj.SetActive(false);//�޾ƿ� �л���ü active�� �ٽ� false�� ��ȯ������ GetPool�� �ٽù޾ƿü��ֵ��� ��
        queue.Enqueue(obj);
        return;
    }
}
