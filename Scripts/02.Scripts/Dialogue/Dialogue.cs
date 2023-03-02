using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Dialogue //�ش� ��ũ��Ʈ�� ����Ƽ ������ ���� ������ �Է��Ҽ��� ������
                      //������ �������
                      //�����ϱ� ����
                      //�ϱ����� Csv������ Parse�ؼ� ����ϱ�� ����
{
    [Tooltip("ĳ���� �̸�")]
    public string name;

    [Tooltip("��� ����")]
    public string[] contexts;

    /*[Tooltip("�̺�Ʈ ��ȣ")]
    public string[] number;

    [Tooltip("��ŵ����")]
    public string[] skipnum;*/
    //[Tooltip("�̺�Ʈ��ȣ")]
    //public string[] number;
    
    //���߿� ��ȭ�� �б����� �������ֱ����� �ϴ��� �ּ�ó��
}

[System.Serializable]
public class DialogueEvent
{
    //�̺�Ʈ �̸�
    public string name; 

    public Vector2 line;
    public Dialogue[] dialogues;
}
