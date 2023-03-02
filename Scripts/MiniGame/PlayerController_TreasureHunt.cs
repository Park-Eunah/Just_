using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController_TreasureHunt : MonoBehaviour
{
    public static PlayerController_TreasureHunt instance = null;
    private float x = 0f; //x축 이동에 사용할 변수
    private float z = 0f; //z축 이동에 사용할 변수
    private float speed = 10f; //이동 속력
    private float rotateY = 0f; //y축 회전에 사용할 변수
    private float mouseSensitivity = 50f; //마우스 감도
    private static bool moveStop;

    private Vector3 dir = Vector3.zero; //방향벡터

    public GameObject cam;

    public GameObject[] lockers;



    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        Hide();
    }
    void Start()
    {
        SoundManager.instance.MainBgmOff();
        moveStop = true;
        SoundManager.instance.BreakTimeBellSound();
        StartCoroutine(TreasureStartDialogue());
    }

    void Update()
    {
        if (!moveStop)
        {
            InputKey();
            Move();
            RotateY();
        }
    }

    private void InputKey() //이동, 회전에 관한 키 입력을 받는 메서드
    {
        x = Input.GetAxisRaw("Horizontal");
        z = Input.GetAxisRaw("Vertical");
        dir = new Vector3(x, 0, z).normalized; //입력 받은 x, z값으로 방향벡터를 만들어 대각선 이동시에도 속도를 일정하게 맞춰줌

        rotateY = Input.GetAxis("Mouse X");
    }
    public void Hide() //미니게임-폰 찾기 맵이 시작될 때 폰이 들어갈 라커 정하는 메서드 
    {
        lockers[Random.Range(0, lockers.Length)].GetComponent<Locker>().isHiding = true;       
    }

    private void Move() //이동
    {
        transform.Translate(dir * Time.deltaTime * speed);
    }

    private void RotateY() //Y축 회전
    {
        transform.Rotate(new Vector3(0, rotateY * Time.deltaTime * mouseSensitivity, 0));
    }
    public IEnumerator TreasureStartDialogue()
    {
        yield return new WaitForSeconds(8.0f);
        SoundManager.instance.PhoneBellSound();
        yield return new WaitForSeconds(1.5f);
        StoryManager.instance.Story(11);
    }
    public void MoveStop(bool flag)
    {
        moveStop = flag;
        Cam_TreasureHunt.isInteracting = flag;
    }
}