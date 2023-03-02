using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trash : MonoBehaviour
{
    private float speedMin = 0f;
    private float speedMax = 0f;
    private float speed = 0f; //플에이어를 향래 날아갈 속도
    private float rotateSpeedMin = 50f;
    private float rotateSpeedMax = 150f;
    private float rotateSpeed = 0f; //날아가면서 회전할 속도

    private void Start()
    {
        speed = Random.Range(speedMin, speedMax);
        rotateSpeed = Random.Range(rotateSpeedMin, rotateSpeedMax);
        Debug.Log(speed + ", " + speedMin + ", " + speedMax);
    }

    void Update()
    {
        Move();
        Rotate();
    }

    void Move()  //일정한 속도로 앞으로 움직이기
    {
        transform.Translate(0, 0, speed * Time.deltaTime, Space.World);
    }

    void Rotate()
    {
        transform.Rotate(new Vector3(rotateSpeed, rotateSpeed, rotateSpeed)*Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {

        if(other.CompareTag("Player"))  //플레이어와 충돌시 실패
        {
            MiniGameManager.instance.MinusHp();
            return;
        }

        //trash끼리의 충돌은 layer 설정으로 처리하지 않도록 함.
        ObjectPool_PEA.instance.PushQueue(gameObject); //플레이어가 아닌 오브젝트와 충돌시 다시 풀에 반환   
    }

    public void SetSpeedRange(int objNum)
    {
        switch (objNum)
        {
            case 0:
                speedMin = 7f;
                speedMax = 10f;
                break;
            case 1:
                speedMin = 11f;
                speedMax = 15f;
                break;
            case 2:
                speedMin = 14f;
                speedMax = 20f;
                break;
            case 3:
                speedMin = 17f;
                speedMax = 25f;
                break;
            case 4:
                speedMin = 20f;
                speedMax = 30f;
                break;
            case 5:
                speedMin = 20f;
                speedMax = 35f;
                break;
        }
    }
}
