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

            if (flag) //2.flag�� true�� �ٲٸ� ���� �ϴ� ��ӽ����
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
        if(Physics.Raycast(transform.position, transform.forward, out hit, 1f)&&flag==false) //1. flag�� false �̸鼭 ���� ���̰� ������� flag�� true�� �ٲ�
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
        RotationY = Mathf.Clamp(RotationY, -cameraLimit, cameraLimit); //ī�޶� y��ȸ���� ����
        cam.transform.eulerAngles += new Vector3(0, RotationY, 0);
       // float RotationX = Input.GetAxis("Mouse Y") * Time.deltaTime * mouseSensitivity;
        //RotationX = Mathf.Clamp(RotationX, -cameraLimit, cameraLimit); //���������� Clamp�� ������ �ּҰ�,�ִ밪 ����
        //cam.transform.eulerAngles -= new Vector3(RotationX, 0, 0);
    }


    void Turn()//���� ���� �ִ� �������� ���ƾߵǴµ� lerp�� ��ǥ������ ��������������(�ϴ�0.8�ʷ� ����) ��� ���� (�������� ȣ���ϴ� �κ��� ���Ƽ� ���߿� �ǵ��޾Ƽ� ��������)
    {
        Vector3 pos = target[i].transform.position - transform.position;
        transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(pos.normalized), Time.deltaTime * speed);
        StartCoroutine(TurnStop());
    }

    IEnumerator TurnStop()//ȸ���� 1�ʵڸ��߰� ���ϸ� �� Turn �Լ��� ��ӽ����
    {
        yield return new WaitForSeconds(0.8f);
        if (flag)
        {
            i++; //i++�� �ٱ��� �θ� 0.8�ʵ��� ��ӽ���Ǳ淡 ���ǹ� �ȿ� �ۼ�
            if (i == target.Length) //�ϴ� ���ѷ����ʸ�������� ������ Ÿ�ٿ� ������ Ÿ���ε����� �ٽ� ù Ÿ�����ΰ����ְ� 0 ���ιٲ���
            {
                i = 0;
            }       
        }
        flag = false; //�ش� ������ 0.8�ʵ��� ��ӽ����ϴ°͵� ���߿� ��������  
    }

    void Timer()
    {
        time -= Time.deltaTime;
    }
}