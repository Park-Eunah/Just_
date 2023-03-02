using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController_TreasureHunt : MonoBehaviour
{
    public static PlayerController_TreasureHunt instance = null;
    private float x = 0f; //x�� �̵��� ����� ����
    private float z = 0f; //z�� �̵��� ����� ����
    private float speed = 10f; //�̵� �ӷ�
    private float rotateY = 0f; //y�� ȸ���� ����� ����
    private float mouseSensitivity = 50f; //���콺 ����
    private static bool moveStop;

    private Vector3 dir = Vector3.zero; //���⺤��

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

    private void InputKey() //�̵�, ȸ���� ���� Ű �Է��� �޴� �޼���
    {
        x = Input.GetAxisRaw("Horizontal");
        z = Input.GetAxisRaw("Vertical");
        dir = new Vector3(x, 0, z).normalized; //�Է� ���� x, z������ ���⺤�͸� ����� �밢�� �̵��ÿ��� �ӵ��� �����ϰ� ������

        rotateY = Input.GetAxis("Mouse X");
    }
    public void Hide() //�̴ϰ���-�� ã�� ���� ���۵� �� ���� �� ��Ŀ ���ϴ� �޼��� 
    {
        lockers[Random.Range(0, lockers.Length)].GetComponent<Locker>().isHiding = true;       
    }

    private void Move() //�̵�
    {
        transform.Translate(dir * Time.deltaTime * speed);
    }

    private void RotateY() //Y�� ȸ��
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