using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam_TreasureHunt : MonoBehaviour
{ 
    private float mouseSensitivity = 45f;
    private float cameraLimit = 40f;
    private float rotateX = 0f;
    private float rayDistance = 5f;
    public static bool isInteracting;

    private RaycastHit hit;

    private GameObject locker = null;
    private GameObject prevLocker = null;
    private GameObject backPack = null;

    private int openCount = 0;
    private bool openBackPack = false;

    private void Start()
    {
        transform.localRotation = (Quaternion.Euler(new Vector3(0, 0, 0)));
        isInteracting = true;
    }

    void Update()
    { 
        if (!isInteracting)
        {
            SearchObject();
            CameraRotation();
        }
    }

    public void CameraRotation() // X축의 회전은 Player가 아닌 카메라만 회전
    {
        rotateX = Input.GetAxis("Mouse Y") * Time.deltaTime * mouseSensitivity;
        rotateX = Mathf.Clamp(rotateX, -cameraLimit, cameraLimit); //Clamp로 각도의 최소값,최대값 설정
        transform.eulerAngles -= new Vector3(rotateX, 0, 0);
    }

    public void SearchObject()
    {
        Debug.DrawRay(transform.position, transform.forward * rayDistance, Color.blue, 0.2f);
        if (Physics.Raycast(transform.position, transform.forward, out hit, rayDistance))
        {
            if (openBackPack)
            {
                if (hit.transform.CompareTag("locker"))
                {
                    if (locker != null)
                    {
                        prevLocker = locker;
                        prevLocker.GetComponent<Outline>().enabled = false;
                    }
                    locker = hit.collider.gameObject;
                    locker.GetComponent<Outline>().enabled = true;
                    InputKey();
                }
            }         
            else if (hit.collider.tag == "interactObj")//가방과 상호작용
            {
                backPack = hit.collider.gameObject;
                backPack.GetComponent<Outline>().enabled = true;
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    StoryManager.instance.Story(12);//가방클릭대화
                    openBackPack = true;
                }
            }
            else
            {
                if (prevLocker != null)
                {
                    prevLocker = locker;
                    prevLocker.GetComponent<Outline>().enabled = false;
                }
                if(backPack != null)
                {
                    backPack.GetComponent<Outline>().enabled = false;//수정예정
                }
            }
        }
    }

    public void InputKey()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Locker();
        }
    }

    public void Locker()
    {
        if (locker.GetComponent<Locker>().isOpen == false)
        {
            locker.GetComponent<Locker>().Open();
            openCount++;//사물함 연 횟수
            if (openCount == 2)
            {
                StoryManager.instance.Story(13);//두번이상못찾을시 왜케 못찾냐고 꾸지람
            }
            //2번 넘어가도 못찾으면 뭐라고 더 해야될듯
        }
        else
        {
            locker.GetComponent<Locker>().Close();
        }
    }
}
