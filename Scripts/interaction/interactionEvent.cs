using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interactionEvent : MonoBehaviour
{
    Outline outline = null;
    [SerializeField] public DialogueEvent[] dialogue;

    public bool showed = false; //대사가 한 번 보여지고 나서 다시 상호작용을 해올 때 다른 대사를 보여주기 위한 변수

    public Dialogue[] GetDialogue()
    {
        if (!showed) //처음 상호작용시 보여질 대사
        {
            dialogue[0].dialogues = DatabaseManager.instance.GetDialogue((int)dialogue[0].line.x, (int)dialogue[0].line.y); //InteratObj에 Ray가닿고 Q를 누르면 DialogueManager에 접근후 해당인수전달
            return dialogue[0].dialogues;
        }
        else //두 번 이상 상호작용시 보여질 대사, 처음 보여질 대사와 같을 시 하나만 설정하기 위해 인덱스를 dialogue.Length - 1로 함
        {
            dialogue[dialogue.Length - 1].dialogues = DatabaseManager.instance.GetDialogue((int)dialogue[dialogue.Length - 1].line.x, (int)dialogue[dialogue.Length - 1].line.y); //InteratObj에 Ray가닿고 Q를 누르면 DialogueManager에 접근후 해당인수전달
            return dialogue[dialogue.Length -1].dialogues;
        }
    }
    private void Awake()
    {
        outline = GetComponent<Outline>();
    }

    private void Start()
    {
        if (outline != null)
        {
            outline.enabled = false;
        }
    }
}
