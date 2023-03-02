using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryManager : MonoBehaviour
{
    public DialogueManager theDm;
    public static StoryManager instance = null;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    public void Story(int index)
    {
        switch (index)
        {
            case 0:
                theDm.ShowDialogue(DatabaseManager.instance.GetDialogue(1,1));//첫날 등교 첫대사(지각하겠다)
                break;
            case 1:
                theDm.ShowDialogue(DatabaseManager.instance.GetDialogue(6, 11));//첫날 복도입장씬
                break;
            case 2:
                theDm.ShowDialogue(DatabaseManager.instance.GetDialogue(12, 12));//내 자리는 저기였지
                break;
            case 3:
                theDm.ShowDialogue(DatabaseManager.instance.GetDialogue(27, 27));//교무실 선생님의 앉으렴
                break;
            case 4:
                theDm.ShowDialogue(DatabaseManager.instance.GetDialogue(29, 29));//주인공 1일차 하교 후 독백
                break;
            case 5:
                theDm.ShowDialogue(DatabaseManager.instance.GetDialogue(30, 32));//2일차 등교
                break;
            case 6:
                theDm.ShowDialogue(DatabaseManager.instance.GetDialogue(36, 43));//2일차 등교대화
                break;
            case 7:
                theDm.ShowDialogue(DatabaseManager.instance.GetDialogue(64, 68), "marketTimeover"); //빵셔틀 시간초과로 탈락시 대화
                break;
            case 8:
                theDm.ShowDialogue(DatabaseManager.instance.GetDialogue(70, 73), "marketMiss"); //빵셔틀 간식 잘못 선택해 탈락시 대화
                break;
            case 9:
                theDm.ShowDialogue(DatabaseManager.instance.GetDialogue(61, 63), "marketClear"); //빵셔틀 한 번에 성공시 대화
                break;
            case 10:
                theDm.ShowDialogue(DatabaseManager.instance.GetDialogue(74, 74), "marketClear"); //빵셔틀 재도전에 성공시 대화
                break;
            case 11:
                theDm.ShowDialogue(DatabaseManager.instance.GetDialogue(76, 77), "RingBell");//핸드폰 찾기이벤트 시작대화
                break;
            case 12:
                theDm.ShowDialogue(DatabaseManager.instance.GetDialogue(78, 80), "BackPack");//핸드폰 찾기이벤트 시작대화
                break;
            case 13:
                theDm.ShowDialogue(DatabaseManager.instance.GetDialogue(81, 82), "BackPack");//핸드폰 찾기이벤트 시작대화
                break;
            case 14:
                theDm.ShowDialogue(DatabaseManager.instance.GetDialogue(83, 94), "FindPhone");//핸드폰 찾기 성공 후 대화
                break;
            default:
                Debug.Log("스토리 인덱스 잘못설정함");
                break;
        }
                          
    }
    public void Story(RaycastHit hit)
    {
        switch (GameManager.sceneCount)
        {
            case 1: //1일차
                theDm.ShowDialogue(hit.transform.GetComponent<interactionEvent>().GetDialogue(), hit.transform.GetComponent<interactionEvent>().dialogue[0].name);
                break;
            case 2: //2일차
                //충돌체의 대화이벤트가 하나뿐이라면 첫번째 대화 출력, 아니라면 두번째 대화 출력.
                switch (hit.transform.GetComponent<interactionEvent>().dialogue.Length)
                {
                    case 1:
                        theDm.ShowDialogue(hit.transform.GetComponent<interactionEvent>().GetDialogue(), hit.transform.GetComponent<interactionEvent>().dialogue[0].name);
                        break;
                    default:
                        hit.transform.GetComponent<interactionEvent>().showed = true;
                        theDm.ShowDialogue(hit.transform.GetComponent<interactionEvent>().GetDialogue(), hit.transform.GetComponent<interactionEvent>().dialogue[1].name);                
                        break;
                }
                break;
        }
    }
}
