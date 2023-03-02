using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interactionEvent : MonoBehaviour
{
    Outline outline = null;
    [SerializeField] public DialogueEvent[] dialogue;

    public bool showed = false; //��簡 �� �� �������� ���� �ٽ� ��ȣ�ۿ��� �ؿ� �� �ٸ� ��縦 �����ֱ� ���� ����

    public Dialogue[] GetDialogue()
    {
        if (!showed) //ó�� ��ȣ�ۿ�� ������ ���
        {
            dialogue[0].dialogues = DatabaseManager.instance.GetDialogue((int)dialogue[0].line.x, (int)dialogue[0].line.y); //InteratObj�� Ray����� Q�� ������ DialogueManager�� ������ �ش��μ�����
            return dialogue[0].dialogues;
        }
        else //�� �� �̻� ��ȣ�ۿ�� ������ ���, ó�� ������ ���� ���� �� �ϳ��� �����ϱ� ���� �ε����� dialogue.Length - 1�� ��
        {
            dialogue[dialogue.Length - 1].dialogues = DatabaseManager.instance.GetDialogue((int)dialogue[dialogue.Length - 1].line.x, (int)dialogue[dialogue.Length - 1].line.y); //InteratObj�� Ray����� Q�� ������ DialogueManager�� ������ �ش��μ�����
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
