using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController_RunMarket : MonoBehaviour
{
    public GameObject buyButton; //���Ź�ư
    public GameObject buyText; //���ſϷ� �ȳ� �ؽ�Ʈ
    public GameObject guideText; //�ȳ� �ؽ�Ʈ

    public Transform[] counterTransform; //ī���� ���� ���ڰ� �ö� ��ġ

    private float moveX = 0f, moveZ = 0f; 
    private float walkSpeed = 10.0f;
    private float runSpeed = 15.0f;
    private float applySpeed;
    private float mouseSensitivity=30f;
    private float cameraLimit = 40f;

    private bool isClicked = false; //���� ��ư�� ������ �ִ��� Ȯ��. ���ѷ��� ����

    private Vector3 dir = Vector3.zero;
    private Vector3 snackPos = Vector3.zero; //���ڰ� �ִ� ��ġ

    private GameObject selectedSnack = null; //���� �տ� ����ִ� ����
    private GameObject curSnack = null; //���� ���̿� ����ִ� ����
    private GameObject prevSnack = null; //���̿� ��Ҵ� ����

    private Transform handTransform = null; //�� ��ġ

    private Camera cam = null;

    private RaycastHit hit;

    void Start()
    {
        cam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        handTransform = transform.GetChild(0); //�� ��ġ ��������
    }

    void Update()
    {
        InputKey();
        Move();
        CharacterRotation();
        CameraRotation();
        RayCast();
    }

    private void InputKey() //�̵��� ���õ� Ű �Է¹ޱ�
    {
        moveX = Input.GetAxis("Horizontal");
        moveZ = Input.GetAxis("Vertical");

        dir = new Vector3(moveX, 0, moveZ);

        //if (Input.GetKey(KeyCode.LeftShift)) //���� ����Ʈ�� ������ �޸���
        //{
        //    applySpeed = runSpeed;
        //}
        //else
        //{ 
        //    applySpeed = walkSpeed;
        //}
    }

    private void Move() //�Է¹��� Ű ���� ���� �̵��ϱ�
    {
        transform.Translate(dir* walkSpeed * Time.deltaTime);
    }

    private void CharacterRotation() //ĳ����ȸ��
    {
        float RotationY = Input.GetAxis("Mouse X") * Time.deltaTime * mouseSensitivity;
        transform.Rotate(0, RotationY, 0);
    }

    private void CameraRotation() // X����ȸ���� Player�� �ƴ� ī�޶� ȸ��
    {
        float RotationX = Input.GetAxis("Mouse Y") * Time.deltaTime * mouseSensitivity;
        RotationX = Mathf.Clamp(RotationX, -cameraLimit, cameraLimit); //Clamp�� ������ �ּҰ�,�ִ밪 ����
        cam.transform.eulerAngles -= new Vector3(RotationX, 0, 0);
    }

    private void RayCast()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //���콺�� ����Ű�� ��ġ�� ���̸� ���.

        if(Physics.Raycast(ray,out hit))
        {
            if (hit.transform.CompareTag("snack")) //���̿� �浹�� ��ü�� �±װ� Snack�϶� ����.
            {
                if (!hit.transform.parent.name.Contains("counter") || hit.transform.parent == null) //ī���� ���� ���� ���� ������ ������ �� ����
                {
                    if (curSnack != null)
                    {
                        prevSnack = curSnack;
                    }

                    curSnack = hit.transform.gameObject;

                    if (prevSnack != curSnack || !curSnack.GetComponent<Outline>().enabled) //���� �����ӿ� �浹�� ���ڿ� ���� �浹���� ���ڰ� ��ġ���� �ʰų� ���� �浹���� ������ �ƿ������� �׷��� ���� ������ ����
                    {
                        if (prevSnack != null)
                        {
                            prevSnack.GetComponent<Outline>().enabled = false; //���� �浹������ ���� ��ü�� �ƿ����� �����ֱ�
                        }
                        curSnack.GetComponent<Outline>().enabled = true; //���� �浹���� ��ü�� �ƿ����� �׷��ֱ�
                    }

                    if (Input.GetMouseButtonDown(0)) //���콺 ��Ŭ����
                    {
                        SelectSnack(); //���� �����ϱ�
                    }
                }
            }
            else if (hit.transform.name.Contains("Counter")) //���̿� �浹�� ��ü�� ī������ �� ����.
            {
                if (selectedSnack != null && Input.GetMouseButtonDown(0)) //snack�� ������ ���¿��� ���콺 ��Ŭ���� ���� ��
                {
                    selectedSnack.transform.parent = null; //�÷��̾��� �տ��� �����
                    PutOnCounter(); //ī���Ϳ� �÷��α�
                }
            }
            else
            {
                if(curSnack!=null)
                {
                    curSnack.GetComponent<Outline>().enabled = false; //�浹������ ���� ��ü�� �ƿ����� �����ֱ�
                }
            }
        }
    }

    private void SelectSnack() //���� �����ϱ�
    {
        if (selectedSnack != null && snackPos != Vector3.zero)
        {
            selectedSnack.transform.parent = null; //�÷��̾��� �տ��� �����
            selectedSnack.transform.position = snackPos; //���� �ִ� �ڸ��� �̵�
        }

        selectedSnack = curSnack; //���� ���̿� �浹���� ���� �����ϱ�
        snackPos = curSnack.transform.position; //���ڰ� �ִ� ��ġ ����ֱ�
        selectedSnack.transform.position = handTransform.position; //�� ��ġ�� ���� ��ġ �Ű��ֱ�
        selectedSnack.transform.parent = handTransform; //�÷��̾��� �� ��ġ�� �ڽ����� ����.
        selectedSnack.transform.localPosition = new Vector3(0, 0, 0);
    }

    private void PutOnCounter() //���õ� ���� ī���� ���� �÷�����
    {
        for (int i = 0; i < counterTransform.Length; i++) //ī���Ϳ� �� �ڸ��� �ִٸ� �� �ڸ��� ���� �����ֱ�. �� �ڸ��� ������ ���� �� ����.
        {
            if (counterTransform[i].childCount == 0)
            {
                selectedSnack.transform.parent = counterTransform[i]; //�� ī���� �ڸ��� �θ�� ����.
                selectedSnack.transform.localPosition = new Vector3(0, 0, 0);
                selectedSnack = null;
                if (i == 2)
                {
                    buyButton.SetActive(true); //ī������ ������ �ڸ��� ä������ ���Ź�ư Ȱ��ȭ
                }
                return;
            }
        }
    }

    public void BuyButton() //���Ź�ư Ŭ����
    {
        if (!isClicked)
        {
            isClicked = true;
            for (int i = 0; i < counterTransform.Length; i++)
            {
                if (!counterTransform[i].GetChild(0).name.Contains("buy")) //�ϳ��� �߸� �������� ��
                {
                    MiniGameManager.instance.Failed(true); //����
                    return;
                }
            }
            UIManager.instance.MarketClear(); //���� ���ſ� �����ϸ� UI �������ְ�
            GetComponent<PlayerCtrl>().enabled = true; // PlayerCtrl ��Ʈ��Ʈ�� ���ְ�
            enabled = false; //�� ��ũ��Ʈ�� ���ش�.

            //���꽺ž�����a
            //MiniGameManager.instance.GameClear(); //����
        }
    }
}
