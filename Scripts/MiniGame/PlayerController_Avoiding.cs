using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController_Avoiding : MonoBehaviour
{
    private float xMin = -18.5f;
    private float xMax = 18.5f;
    private float x = 0;
    private float speed = 10;

    void Update()
    {
        InputKey();
        Move();
    }

    void InputKey()  //�¿� �̵��� �� �Է� �ޱ�
    {
        x = Input.GetAxisRaw("Horizontal") * Time.deltaTime * speed;
    }

    void Move()  //�Է� ���� ��ŭ �̵��ϱ�
    {
        transform.Translate(x, 0, 0);
        transform.localPosition = ClampPosition(transform.localPosition);
    }

    Vector3 ClampPosition(Vector3 position)  //�÷��̾� �¿� �̵����� �����ֱ�
    {
        return new Vector3(Mathf.Clamp(position.x, xMin, xMax), position.y, position.z);
    }
}
