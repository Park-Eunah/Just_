using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class RunGame : MonoBehaviour
{
    #region 필요한 변수들 선언
    [SerializeField]
    float Zspeed = 8f;
    [SerializeField]
    float Xspeed = 10f;

    public GameObject[] target;

    private float x;
    private float time = 5f;
    private float turnAngle = 0f;
    private float turnSpeed = 300f;
    private float turn = 0f;
    private float turnleft = -90;
    private float turnRight = 90;

    private bool isTurn = false;
    private bool isStart = false;

    private Vector3 targetAngle = Vector3.zero; //회전의 목표 방향

    RaycastHit hit;
    #endregion

    void Update()
    {
        Debug.Log(Zspeed);
        if (isStart)
        {
            Timer();
            if (time <= 0f)
            {
                transform.Translate(0, 0, Zspeed * Time.deltaTime);
                RayCast();
                Move();
            }
        }
    }
    void Move()
    {
        x = Input.GetAxis("Horizontal") * Time.deltaTime * Xspeed;
        transform.Translate(x, 0, 0);
    }

    void RayCast()
    {
        Debug.DrawRay(transform.position, transform.forward, Color.black, 2.2f);
        if (Physics.Raycast(transform.position, transform.forward, out hit, 2.2f)) //1. flag가 false 이면서 벽에 레이가 닿았을시 flag를 true로 바꿈
        {
            if (hit.transform.CompareTag("left"))
            {
                if (!isTurn)
                {
                    isTurn = true;
                    turnAngle = 0;
                    turn = 0;
                    TargetSetting(hit.transform.tag);
                    StartCoroutine(LeftTurn());
                }
            }
            else if(hit.transform.CompareTag("right"))
            {
                if (!isTurn)
                {
                    isTurn = true;
                    turnAngle = 0;
                    turn = 0;
                    TargetSetting(hit.transform.tag);
                    StartCoroutine(RightTurn());
                }
            }
        }
    }
    void Timer()
    {
        time -= Time.deltaTime;
    }

    private void TargetSetting(string tag) //마지막에 각도 정리해줄 회전값 설정.
    {
        switch (tag)
        {
            case "left":
                targetAngle = new Vector3(0, transform.rotation.eulerAngles.y + turnleft, 0);
                break;
            case "right":
                targetAngle = new Vector3(0, transform.rotation.eulerAngles.y + turnRight, 0);
                break;
        }
    }

    IEnumerator LeftTurn()
    {
        while (turnAngle <= 90)
        {
            turn = turnSpeed * Time.deltaTime;
            turnAngle += turn;
            yield return new WaitForSeconds(0.01f);
            transform.Rotate(0, -turn, 0);
        }
        Arrangement(); 
        isTurn = false;
    }

    IEnumerator RightTurn()
    {
        while (turnAngle <= 90)
        {
            turn = turnSpeed * Time.deltaTime;
            turnAngle += turn;
            yield return new WaitForSeconds(0.01f);
            transform.Rotate(0, turn, 0);
        }
        Arrangement();
        isTurn = false;
    }

    private void Arrangement() //각도 정리
    {
        transform.rotation = Quaternion.Euler(targetAngle);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Finish"))  //매점까지 완주 하면 매점으로 씬 이동.
        {
            Zspeed = 0f;
            Fade.instance.FadeToLevel(4);
        }
        else if (collision.collider.CompareTag("ground"))
        {
            Zspeed = 8f;
        }
    }

    public void RunStart()
    {
        UIManager.instance.RunStart(); //가이드 창 지워주기
        isStart = true; // 게임 시작
    }
}
