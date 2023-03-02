using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockerDoor : MonoBehaviour
{
    private Animator anim = null;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void Open()
    {
        anim.SetBool("isOpen", true);
    }

    public void Close()
    {
        anim.SetBool("isOpen", false);
    }
}
