using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private float yMin = -0.2f; //y의 최소값
    private float yMax = 0.2f; //y의 최대값
    private float y = 0f; //현재 y값
    private float speed = 0.5f;

    private bool isUp = true; //올라갈지 내려갈지 확인하는 불리언.

    private void OnEnable() //화살표가 활성화될 때 0,0,0으로 위치 초기화
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
