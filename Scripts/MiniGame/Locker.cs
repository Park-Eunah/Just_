using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locker : MonoBehaviour
{
    public GameObject phone = null;
    LockerDoor door = null;

    Transform phonePos;

    Outline outline = null;

    public bool isOpen = false; //문이 열려있는지 확인하는 변수
    public bool isHiding = false; //해당 오브젝트에 폰이 숨겨져 있는지 확인하는 변수

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
        if (isHiding) //폰이 숨겨져 있는 라커를 열었을 경우 성공 
        {
            PlayerController_TreasureHunt.instance.MoveStop(true);
            SoundManager.instance.PhoneBellSoundStop();//벨소리종료
            Fade.instance.FadeInOut();
            StartCoroutine(FindFinishDialogue());
        }
    }

    public void Close()
    {
        isOpen = false;
        door.Close();
    }
    IEnumerator FindFinishDialogue()//위치값도 옮겨줄 예정
    {
        yield return new WaitForSeconds(2.0f);
        StoryManager.instance.Story(14);
    }
}
