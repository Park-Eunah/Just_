using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locker : MonoBehaviour
{
    public GameObject phone = null;
    LockerDoor door = null;

    Transform phonePos;

    Outline outline = null;

    public bool isOpen = false; //���� �����ִ��� Ȯ���ϴ� ����
    public bool isHiding = false; //�ش� ������Ʈ�� ���� ������ �ִ��� Ȯ���ϴ� ����

    private void Awake()
    {
        door = GetComponentInChildren<LockerDoor>();
        outline = GetComponent<Outline>(); 
    }

    private void Start()
    {
        if (outline != null)
        {
            outline.enabled = false;
        }
        if (isHiding)
        {
            Instantiate(phone,transform);
        }
    }

    public void Open()
    {
        isOpen = true;
        door.Open();
        if (isHiding) //���� ������ �ִ� ��Ŀ�� ������ ��� ���� 
        {
            PlayerController_TreasureHunt.instance.MoveStop(true);
            SoundManager.instance.PhoneBellSoundStop();//���Ҹ�����
            Fade.instance.FadeInOut();
            StartCoroutine(FindFinishDialogue());
        }
    }

    public void Close()
    {
        isOpen = false;
        door.Close();
    }
    IEnumerator FindFinishDialogue()//��ġ���� �Ű��� ����
    {
        yield return new WaitForSeconds(2.0f);
        StoryManager.instance.Story(14);
    }
}
