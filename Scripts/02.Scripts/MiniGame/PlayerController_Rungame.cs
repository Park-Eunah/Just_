using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController_Rungame : MonoBehaviour
{
    [SerializeField]
    float Zspeed = 3f;
    [SerializeField]
    float Xspeed = 10f;
    [SerializeField]
    float speed = 50f;
    RaycastHit hit;
    public GameObject[] target;
    public Camera cam;
    [SerializeField]
    float mouseSensitivity;
    int i = 0;
    float x;
    float cameraLimit = 45f;
    bool flag = false;
    private float time = 5f;
    

    void Update()
    {
        Timer();
        if (time <= 0f)
        {
            transform.Translate(0, 0, Zspeed * Time.deltaTime);
            RayCast();
            Move();
            CameraRotation();

            if (flag) //2.flag를 true로 바꾸면 턴이 일단 계속실행됨
            {
                Turn();
            }

        }
        
    }
    void BackTurn()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(0, 180, 0)),0);
    }
    void Move()
    {
        x = Input.GetAxis("Horizontal") * Time.deltaTime * Xspeed;
        transform.Translate(x, 0, 0);
    }

    void RayCast()
    {
        if(Physics.Raycast(transform.position, transform.forward, out hit, 1f)&&flag==false) //1. flag가 false 이면서 벽에 레이가 닿았을시 flag를 true로 바꿈
        {
            if (hit.transform.tag == "wall")
            {
                flag = true;
            }
        }
    }

    void CameraRotation()
    { 
        float RotationY = Input.GetAxis("Mouse X") * Time.deltaTime * mouseSensitivity;
        RotationY = Mathf.Clamp(RotationY, -cameraLimit, cameraLimit); //카메라 y축회전값 조정
        cam.transform.eulerAngles += new Vector3(0, RotationY, 0);
       // float RotationX = Input.GetAxis("Mouse Y") * Time.deltaTime * mouseSensitivity;
        //RotationX = Mathf.Clamp(RotationX, -cameraLimit, cameraLimit); //마찬가지로 Clamp로 각도의 최소값,최대값 설정
        //cam.transform.eulerAngles -= new Vector3(RotationX, 0, 0);
    }


    void Turn()//다음 벽이 있는 방향으로 돌아야되는데 lerp로 목표값까지 돌수있을때까지(일단0.8초로 설정) 계속 실행 (쓸데없이 호출하는 부분이 많아서 나중에 피드백받아서 수정예정)
    {
        Vector3 pos = target[i].transform.position - transform.position;
        transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(pos.normalized), Time.deltaTime * speed);
        StartCoroutine(TurnStop());
    }

    IEnumerator TurnStop()//회전이 1초뒤멈추게 안하면 위 Turn 함수가 계속실행됨
    {
        yield return new WaitForSeconds(0.8f);
        if (flag)
        {
            i++; //i++를 바깥에 두면 0.8초동안 계속실행되길래 조건문 안에 작성
            if (i == target.Length) //일단 무한루프맵만들기위해 마지막 타겟에 갔을시 타켓인덱스를 다시 첫 타켓으로갈수있게 0 으로바꿔줌
            {
                i = 0;
            }       
        }
        flag = false; //해당 구문을 0.8초동안 계속실행하는것도 나중에 수정예정  
    }

    void Timer()
    {
        time -= Time.deltaTime;
    }
}