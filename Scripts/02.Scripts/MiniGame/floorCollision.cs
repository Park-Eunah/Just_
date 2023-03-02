using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floorCollision : MonoBehaviour
{
    public int count = 0;
    public void OnCollisionEnter(Collision collision) //바닥에 물체가 닿으면 카운트1업
    {
        if(collision.gameObject.tag == "target")
        {
            Debug.Log("count" + count);
            count++;
            collision.gameObject.tag = "apple";//타겟이 바닥에 두번충돌처리 되지않게
        }
    }
}
