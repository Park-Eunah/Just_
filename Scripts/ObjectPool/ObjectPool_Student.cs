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
        if(objectPool == null)//싱글톤
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
        InvokeRepeating("GetObject", 2f, Random.Range(3,4));//게임실행1초후 3초에서 5초사이 한번 해당메소드 실행(하려했는데 인수로 랜덤값
                                                          //안된다고해서 코루틴으로 변경예정

    }
    public void CreateStudent() //게임실행시 한번 엑스트라학생객체 maxCount 만큼생성
    {
        for (int i = 0; i < maxCount; i++) 
        {
            GameObject temp = Instantiate(prefab[Random.Range(0, 2)]);
            temp.name = "student" + i;
            temp.transform.parent = transform; //오브젝트풀Object의 자식으로 넣어줌
            temp.SetActive(false);
            queue.Enqueue(temp);//queue에 넣어줌
        }
    }
    public void GetObject()
    {
        foreach(GameObject item in queue) //foreach로 queue 안에 들어간 프리팹들을 하나하나 꺼내봄
        {
            if(item.activeSelf == false) //하나씩 꺼내지만 setActive가 false인 녀석부터 꺼내줌
            {
                GameObject obj = queue.Dequeue();
                obj.SetActive(true);//setActive를 true로 만들어줌
                obj.transform.position = new Vector3(Random.Range(-3.0f,22f),0,-243.1f);//생성된 프리팹의 위치(x값은 랜덤으로 줘서 x위치 각각 다르게 설정)
                return;//한개씩꺼내와야되기때문에 return으로 끊어줌
            }
        }
    }
    public void returnPool(GameObject obj)
    {
        obj.SetActive(false);//받아온 학생객체 active를 다시 false로 변환함으로 GetPool로 다시받아올수있도록 함
        queue.Enqueue(obj);
        return;
    }
}
