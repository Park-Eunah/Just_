using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public Transform bulletParent;
    Vector3 speed = new Vector3(0, 70, 700);
    Vector3 firstPos;
    Vector3 firstRot;
    public bool isShoot = false;
    private Rigidbody rigid;
    void Start()
    {
        firstPos = transform.position;
        firstRot = transform.eulerAngles;
        rigid = GetComponent<Rigidbody>();
        rigid.isKinematic = true;
    }

    // Update is called once per frame
    void Update()
    {
        Shoot();
    }
    void Shoot()
    {
        if (Input.GetMouseButtonDown(0) && !isShoot)
        {
            rigid.isKinematic = false;
            rigid.AddRelativeForce(speed);
            transform.parent = null;
            isShoot = true;
            StartCoroutine(ReturnBullet(this.gameObject));
            firstPos = transform.position;
            firstRot = transform.eulerAngles;
        }
    }
    IEnumerator ReturnBullet(GameObject bullet)
    {
        yield return new WaitForSeconds(2.0f);
        rigid.isKinematic = true;
        bullet.transform.parent = bulletParent;
        bullet.transform.position = firstPos;
        bullet.transform.eulerAngles = firstRot;
        isShoot = false;
    }


}
