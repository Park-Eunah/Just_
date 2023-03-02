using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController_RunMarket : MonoBehaviour
{
    public GameObject buyButton; //구매버튼
    public GameObject buyText; //구매완료 안내 텍스트
    public GameObject guideText; //안내 텍스트

    public Transform[] counterTransform; //카운터 위에 과자가 올라갈 위치

    private float moveX = 0f, moveZ = 0f; 
    private float walkSpeed = 10.0f;
    private float runSpeed = 15.0f;
    private float applySpeed;
    private float mouseSensitivity=30f;
    private float cameraLimit = 40f;

    private bool isClicked = false; //구매 버튼이 눌려져 있는지 확인. 무한루프 방지

    private Vector3 dir = Vector3.zero;
    private Vector3 snackPos = Vector3.zero; //과자가 있던 위치

    private GameObject selectedSnack = null; //현재 손에 들려있는 과자
    private GameObject curSnack = null; //현재 레이에 닿아있는 과자
    private GameObject prevSnack = null; //레이에 닿았던 과자

    private Transform handTransform = null; //손 위치

    private Camera cam = null;

    private RaycastHit hit;

    void Start()
    {
        cam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        handTransform = transform.GetChild(0); //손 위치 가져오기
    }

    void Update()
    {
        InputKey();
        Move();
        CharacterRotation();
        CameraRotation();
        RayCast();
    }

    private void InputKey() //이동에 관련된 키 입력받기
    {
        moveX = Input.GetAxis("Horizontal");
        moveZ = Input.GetAxis("Vertical");

        dir = new Vector3(moveX, 0, moveZ);

        //if (Input.GetKey(KeyCode.LeftShift)) //왼쪽 쉬프트를 누르면 달리기
        //{
        //    applySpeed = runSpeed;
        //}
        //else
        //{ 
        //    applySpeed = walkSpeed;
        //}
    }

    private void Move() //입력받은 키 값에 따라 이동하기
    {
        transform.Translate(dir* walkSpeed * Time.deltaTime);
    }

    private void CharacterRotation() //캐릭터회전
    {
        float RotationY = Input.GetAxis("Mouse X") * Time.deltaTime * mouseSensitivity;
        transform.Rotate(0, RotationY, 0);
    }

    private void CameraRotation() // X축의회전은 Player가 아닌 카메라만 회전
    {
        float RotationX = Input.GetAxis("Mouse Y") * Time.deltaTime * mouseSensitivity;
        RotationX = Mathf.Clamp(RotationX, -cameraLimit, cameraLimit); //Clamp로 각도의 최소값,최대값 설정
        cam.transform.eulerAngles -= new Vector3(RotationX, 0, 0);
    }

    private void RayCast()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //마우스가 가리키는 위치로 레이를 쏜다.

        if(Physics.Raycast(ray,out hit))
        {
            if (hit.transform.CompareTag("snack")) //레이와 충돌한 물체의 태그가 Snack일때 실행.
            {
                if (!hit.transform.parent.name.Contains("counter") || hit.transform.parent == null) //카운터 위에 있지 않은 상태의 과자일 때 실행
                {
                    if (curSnack != null)
                    {
                        prevSnack = curSnack;
                    }

                    curSnack = hit.transform.gameObject;

                    if (prevSnack != curSnack || !curSnack.GetComponent<Outline>().enabled) //이전 프레임에 충돌한 과자와 현재 충돌중인 과자가 일치하지 않거나 현재 충돌중인 과자의 아웃라인이 그려져 있지 않으면 실행
                    {
                        if (prevSnack != null)
                        {
                            prevSnack.GetComponent<Outline>().enabled = false; //현재 충돌중이지 않은 물체의 아웃라인 지워주기
                        }
                        curSnack.GetComponent<Outline>().enabled = true; //현재 충돌중인 물체의 아웃라인 그려주기
                    }

                    if (Input.GetMouseButtonDown(0)) //마우스 좌클릭시
                    {
                        SelectSnack(); //과자 선택하기
                    }
                }
            }
            else if (hit.transform.name.Contains("Counter")) //레이와 충돌한 물체가 카운터일 때 실행.
            {
                if (selectedSnack != null && Input.GetMouseButtonDown(0)) //snack을 선택한 상태에서 마우스 좌클릭을 했을 때
                {
                    selectedSnack.transform.parent = null; //플레이어의 손에서 벗어나기
                    PutOnCounter(); //카운터에 올려두기
                }
            }
            else
            {
                if(curSnack!=null)
                {
                    curSnack.GetComponent<Outline>().enabled = false; //충돌중이지 않은 물체의 아웃라인 지워주기
                }
            }
        }
    }

    private void SelectSnack() //과자 선택하기
    {
        if (selectedSnack != null && snackPos != Vector3.zero)
        {
            selectedSnack.transform.parent = null; //플레이어의 손에서 벗어나기
            selectedSnack.transform.position = snackPos; //원래 있던 자리로 이동
        }

        selectedSnack = curSnack; //현재 레이와 충돌중인 과자 선택하기
        snackPos = curSnack.transform.position; //과자가 있던 위치 담아주기
        selectedSnack.transform.position = handTransform.position; //손 위치로 과자 위치 옮겨주기
        selectedSnack.transform.parent = handTransform; //플레이어의 손 위치의 자식으로 지정.
        selectedSnack.transform.localPosition = new Vector3(0, 0, 0);
    }

    private void PutOnCounter() //선택된 과자 카운터 위에 올려놓기
    {
        for (int i = 0; i < counterTransform.Length; i++) //카운터에 빈 자리가 있다면 빈 자리에 과자 놓아주기. 빈 자리가 업으면 놓을 수 없음.
        {
            if (counterTransform[i].childCount == 0)
            {
                selectedSnack.transform.parent = counterTransform[i]; //빈 카운터 자리를 부모로 설정.
                selectedSnack.transform.localPosition = new Vector3(0, 0, 0);
                selectedSnack = null;
                if (i == 2)
                {
                    buyButton.SetActive(true); //카운터의 마지막 자리가 채워지면 구매버튼 활성화
                }
                return;
            }
        }
    }

    public void BuyButton() //구매버튼 클릭시
    {
        if (!isClicked)
        {
            isClicked = true;
            for (int i = 0; i < counterTransform.Length; i++)
            {
                if (!counterTransform[i].GetChild(0).name.Contains("buy")) //하나라도 잘못 선택했을 시
                {
                    MiniGameManager.instance.Failed(true); //실패
                    return;
                }
            }
            UIManager.instance.MarketClear(); //과자 구매에 성공하면 UI 정리해주고
            GetComponent<PlayerCtrl>().enabled = true; // PlayerCtrl 스트립트를 켜주고
            enabled = false; //이 스크립트는 꺼준다.

            //무브스탑만들어줭
            //MiniGameManager.instance.GameClear(); //성공
        }
    }
}
