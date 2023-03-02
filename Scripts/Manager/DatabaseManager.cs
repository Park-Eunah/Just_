using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager instance;

    public string csv_FileName; //csv���ϳ����� �ش� ��ũ��Ʈ�� �ν�����â���� �Է¹޴´�.

    Dictionary<int, Dialogue> dialogueDic = new Dictionary<int, Dialogue>(); 

    public static bool isFinish = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DialogueParser theParser = GetComponent<DialogueParser>();
            Dialogue[] dialogues = theParser.Parse(csv_FileName); //���������� DialogueParser�� �� csv_Filename�� �־��ִºκ��̴�.
            for(int i =0; i< dialogues.Length; i++)
            {
                dialogueDic.Add(i + 1,dialogues[i]); //Ű������ ���ڸ� �ָ� �� �ٿ� �´� ������ ã�ƿ�����.
            }
            isFinish = true;
        }
        else //�غ�
        {
            Destroy(this);
        }
    }

    public Dialogue[] GetDialogue(int _StartNum,int _EndNum)
    {
        List<Dialogue> dialogueList = new List<Dialogue>();
        for(int i=0;i<=_EndNum - _StartNum; i++) //1��°�ٰ� 3��°�ٱ��� ��3�� ��縦 ������������� 3-1+1��ŭ �ݺ��� �����ߵǱ⶧���� �ش���� ����
        {
            dialogueList.Add(dialogueDic[_StartNum + i]);
        }
        return dialogueList.ToArray();
    }

}
