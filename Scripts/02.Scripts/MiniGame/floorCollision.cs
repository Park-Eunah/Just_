using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floorCollision : MonoBehaviour
{
    public int count = 0;
    public void OnCollisionEnter(Collision collision) //�ٴڿ� ��ü�� ������ ī��Ʈ1��
    {
        if(collision.gameObject.tag == "target")
        {
            Debug.Log("count" + count);
            count++;
            collision.gameObject.tag = "apple";//Ÿ���� �ٴڿ� �ι��浹ó�� �����ʰ�
        }
    }
}
