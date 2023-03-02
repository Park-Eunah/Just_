using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trash : MonoBehaviour
{
    private float speedMin = 0f;
    private float speedMax = 0f;
    private float speed = 0f; //�ÿ��̾ �ⷡ ���ư� �ӵ�
    private float rotateSpeedMin = 50f;
    private float rotateSpeedMax = 150f;
    private float rotateSpeed = 0f; //���ư��鼭 ȸ���� �ӵ�

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

    void Move()  //������ �ӵ��� ������ �����̱�
    {
        transform.Translate(0, 0, speed * Time.deltaTime, Space.World);
    }

    void Rotate()
    {
        transform.Rotate(new Vector3(rotateSpeed, rotateSpeed, rotateSpeed)*Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {

        if(other.CompareTag("Player"))  //�÷��̾�� �浹�� ����
        {
            MiniGameManager.instance.MinusHp();
            return;
        }

        //trash������ �浹�� layer �������� ó������ �ʵ��� ��.
        ObjectPool_PEA.instance.PushQueue(gameObject); //�÷��̾ �ƴ� ������Ʈ�� �浹�� �ٽ� Ǯ�� ��ȯ   
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
