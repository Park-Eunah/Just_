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

    void InputKey()  //좌우 이동할 값 입력 받기
    {
        x = Input.GetAxisRaw("Horizontal") * Time.deltaTime * speed;
    }

    void Move()  //입력 받은 만큼 이동하기
    {
        transform.Translate(x, 0, 0);
        transform.localPosition = ClampPosition(transform.localPosition);
    }

    Vector3 ClampPosition(Vector3 position)  //플레이어 좌우 이동값에 제한주기
    {
        return new Vector3(Mathf.Clamp(position.x, xMin, xMax), position.y, position.z);
    }
}
