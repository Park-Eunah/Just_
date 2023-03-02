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

    public void CameraRotation() // X���� ȸ���� Player�� �ƴ� ī�޶� ȸ��
    {
        rotateX = Input.GetAxis("Mouse Y") * Time.deltaTime * mouseSensitivity;
        rotateX = Mathf.Clamp(rotateX, -cameraLimit, cameraLimit); //Clamp�� ������ �ּҰ�,�ִ밪 ����
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
            else if (hit.collider.tag == "interactObj")//����� ��ȣ�ۿ�
            {
                backPack = hit.collider.gameObject;
                backPack.GetComponent<Outline>().enabled = true;
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    StoryManager.instance.Story(12);//����Ŭ����ȭ
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
                    backPack.GetComponent<Outline>().enabled = false;//��������
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
            openCount++;//�繰�� �� Ƚ��
            if (openCount == 2)
            {
                StoryManager.instance.Story(13);//�ι��̻��ã���� ���� ��ã�İ� ������
            }
            //2�� �Ѿ�� ��ã���� ����� �� �ؾߵɵ�
        }
        else
        {
            locker.GetComponent<Locker>().Close();
        }
    }
}
