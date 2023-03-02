using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private float yMin = -0.2f; //y�� �ּҰ�
    private float yMax = 0.2f; //y�� �ִ밪
    private float y = 0f; //���� y��
    private float speed = 0.5f;

    private bool isUp = true; //�ö��� �������� Ȯ���ϴ� �Ҹ���.

    private void OnEnable() //ȭ��ǥ�� Ȱ��ȭ�� �� 0,0,0���� ��ġ �ʱ�ȭ
    {
        y = 0f;
        transform.localPosition = new Vector3(0, y, 0); 
    }

    void Update()
    {
        if (isUp)
        {
            GoUp();
        }
        else
            GoDown();

        transform.localPosition = new Vector3(0, y, 0) * speed;
    }

    private void GoUp()
    {
        y += Time.deltaTime;

        if (y >= yMax)
            isUp = false;
    }

    private void GoDown()
    {
        y -= Time.deltaTime;

        if (y <= yMin)
            isUp = true;
    }
}
