using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager instance;

    public string csv_FileName; //csv파일네임을 해당 스크립트의 인스펙터창에서 입력받는다.

    Dictionary<int, Dialogue> dialogueDic = new Dictionary<int, Dialogue>(); 

    public static bool isFinish = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DialogueParser theParser = GetComponent<DialogueParser>();
            Dialogue[] dialogues = theParser.Parse(csv_FileName); //실질적으로 DialogueParser에 들어갈 csv_Filename을 넣어주는부분이다.
            for(int i =0; i< dialogues.Length; i++)
            {
                dialogueDic.Add(i + 1,dialogues[i]); //키값으로 숫자를 주면 그 줄에 맞는 대사들이 찾아와진다.
            }
            isFinish = true;
        }
        else //해봄
        {
            Destroy(this);
        }
    }

    public Dialogue[] GetDialogue(int _StartNum,int _EndNum)
    {
        List<Dialogue> dialogueList = new List<Dialogue>();
        for(int i=0;i<=_EndNum - _StartNum; i++) //1번째줄과 3번째줄까지 총3번 대사를 꺼내오고싶으면 3-1+1만큼 반복문 돌려야되기때문에 해당범위 지정
        {
            dialogueList.Add(dialogueDic[_StartNum + i]);
        }
        return dialogueList.ToArray();
    }

}
