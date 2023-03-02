using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DialogueParser : MonoBehaviour
{
    public Dialogue[] Parse(string _CSVFileName)
    {
        List<Dialogue> dialogueList = new List<Dialogue>(); //��ȭ����Ʈ����
        TextAsset csvData = Resources.Load<TextAsset>(_CSVFileName); //csv���� �����ͼ� �����

        string[] data = csvData.text.Split(new char[]{ '\n' }); //�־��� csv����(����) ���ؿ��� �ϴ� ���ͱ������� �ɰ���
        for(int i = 1; i<data.Length;) //0��°���� ������ ����������� ID ���� �����ֱ⿡ 1���� ����
        {
            string[] row = data[i].Split(new char[] { ',' }); //ù��° ���� �ٽ� ��ǥ �������� row ����(���������� ��)�� �ɰ���
            
            Dialogue dialogue = new Dialogue(); //ó�� �����ߴ� Dialogue ��ü�� ����

            dialogue.name = row[1];  //name�� row[1]��°�ִ� string������ �־���(�̸�)
            List<string> contextList = new List<string>();//context ����Ʈ ����(���� �̸��� �޸� �ϳ����ƴ϶� �̸��ϳ��� �������ֱ⿡ ���� context ����Ʈ����)


            do //row������ ©�� ����Ʈ�� �־���
            {
                contextList.Add(row[2]); //���1���� ������������ dowhile������ ��縮��Ʈ�� row[2] �� csv����ù��°�ٿ��� ��ǥ������ ¥������ �ι�°�� �ִ�(ù��°���) �༮�� �����ͼ� ��縮��Ʈ�� �־���
                if (++i < data.Length)
                {
                   row = data[i].Split(new char[] { ',' });
                }
                else
                {
                    break;
                }
            } while (row[0].ToString() == "");

            dialogue.contexts = contextList.ToArray(); //�߶��� �༮���� DialogueŬ������ �ν��Ͻ�ȭ��Ų dialogue�� contexts�� �迭�� �����ؼ� �־���

            dialogueList.Add(dialogue);//�̸��� �� ������ �Ѽ�Ʈ�� �־��� �ϼ��� ��ȭ����� dialogueList�� �־���
        }
        return dialogueList.ToArray(); //List������ Dialogue[]�� ���������� �迭�������� �ٲ��ص� ����
    }
}
