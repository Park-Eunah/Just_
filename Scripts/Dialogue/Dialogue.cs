using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Dialogue //해당 스크립트로 유니티 내에서 직접 대사들을 입력할수도 있지만
                      //구현은 어렵지만
                      //관리하기 쉽게
                      //하기위해 Csv파일을 Parse해서 사용하기로 결정
{
    [Tooltip("캐릭터 이름")]
    public string name;

    [Tooltip("대사 내용")]
    public string[] contexts;

    /*[Tooltip("이벤트 번호")]
    public string[] number;

    [Tooltip("스킵라인")]
    public string[] skipnum;*/
    //[Tooltip("이벤트번호")]
    //public string[] number;
    
    //나중에 대화의 분기점을 설정해주기위해 일단은 주석처리
}

[System.Serializable]
public class DialogueEvent
{
    //이벤트 이름
    public string name; 

    public Vector2 line;
    public Dialogue[] dialogues;
}
