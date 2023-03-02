using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController_TargetShoot : MonoBehaviour
{
    private floorCollision floor;
    float x, y;
    float Xspeed = 50f;
    float Yspeed = 30f;
    public Camera cam = null;
    private bullet bull;
    private int targetCount = 5;//�Ӹ��� ��ü����
    public ParticleSystem particleObject = null;
    private void Start()
    {
        bull = FindObjectOfType<bullet>();
        floor = FindObjectOfType<floorCollision>();
    }
    // Start is called before the first frame update
    private void Update()
    {
        if (!bull.isShoot)
        {
            x = Input.GetAxis("Mouse Y") * -Xspeed;
            x = Mathf.Clamp(x, -45f, 45f);
            transform.eulerAngles += new Vector3(x, 0, 0) * Time.deltaTime;
            y = Input.GetAxis("Horizontal") * Yspeed;
            y = Mathf.Clamp(y, -45f, 45f);
            cam.transform.eulerAngles += new Vector3(0, y, 0) * Time.deltaTime;
        }
        if(floor.count == targetCount) //������ �Ӹ��� ����� ������ �ٴڿ� ������ ����� ������ ������ clear
        {
            MiniGameManager.instance.GameClear();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "friend")
        {
            MiniGameManager.instance.MinusHp();
        }
    }
}
